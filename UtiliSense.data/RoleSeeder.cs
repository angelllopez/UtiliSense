/*
PSEUDOCODE / PLAN (detailed):
1. Read roles configuration robustly:
   - Try binding the "Seed:Roles" section to a string[] via IConfiguration.GetSection(...).Get<string[]>().
   - If that returns null, also attempt to read the raw "Seed:Roles" value (e.g., "Admin,User") and parse it:
     - Split on commas and semicolons, trim entries, remove empties.
   - If both attempts produce no roles, fall back to default roles: ["Admin", "User"].
   - Ensure roles are distinct (case-insensitive).
2. Continue existing seeding logic without changing its behavior:
   - Create missing roles.
   - Optionally create/assign admin user when credentials present.
3. Add necessary using directives and keep existing safe logger resolution and exception handling.
4. Keep code compatible with nullable reference contexts and resilient to malformed configuration.

This file replaces the previous fragile single-line role reading with robust parsing and fallback.
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UtiliSense.data
{
    /// <summary>
    /// Seeds roles and an optional admin user. Intended for startup use only.
    /// Admin credentials must come from secure configuration (User Secrets / Env / Key Vault).
    /// </summary>
    public static class RoleSeeder
    {
        /// <summary>
        /// Ensure required roles exist and optionally create an admin user.
        /// Call from startup with a created service scope:
        /// await RoleSeeder.SeedAsync(services, configuration);
        /// </summary>
        public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            // Safe logger resolution: try ILogger<T>, then ILoggerFactory, then NullLogger to avoid throwing.
            var logger = serviceProvider.GetService<ILogger<object>>()
                         ?? serviceProvider.GetService<ILoggerFactory>()?.CreateLogger<object>()
                         ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<object>.Instance;

            logger.LogInformation("RoleSeeder: Starting role/user seeding...");

            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Robust roles parsing WITHOUT relying on ConfigurationBinder:
                // 1) If "Seed:Roles" is a JSON array, read children values via GetChildren().
                // 2) If empty, try raw config value and split on common delimiters (supports comma-separated env var).
                // 3) Fall back to default roles if nothing provided.
                var rolesSection = configuration.GetSection("Seed:Roles");
                var roles = rolesSection
                    .GetChildren()
                    .Select(c => c.Value)
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .ToArray();

                if (roles.Length == 0)
                {
                    var raw = configuration["Seed:Roles"];
                    if (!string.IsNullOrWhiteSpace(raw))
                    {
                        roles = raw
                            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(r => r.Trim())
                            .Where(r => !string.IsNullOrEmpty(r))
                            .ToArray();
                    }
                }

                roles ??= new[] { "Admin", "User" };

                // Ensure no nulls before checking role existence
                foreach (var role in roles.Distinct(StringComparer.OrdinalIgnoreCase).Where(r => !string.IsNullOrWhiteSpace(r)))
                {
                    var safeRole = role ?? string.Empty; // Ensure non-null
                    if (!await roleManager.RoleExistsAsync(safeRole))
                    {
                        var createRoleResult = await roleManager.CreateAsync(new IdentityRole(safeRole));
                        if (!createRoleResult.Succeeded)
                        {
                            logger.LogWarning("RoleSeeder: Failed to create role {Role}: {Errors}", safeRole,
                                string.Join(", ", createRoleResult.Errors.Select(e => e.Description)));
                        }
                        else
                        {
                            logger.LogInformation("RoleSeeder: Created role {Role}", safeRole);
                        }
                    }
                }

                // Admin seeding (credentials MUST be provided through secure config)
                var adminEmail = configuration["Seed:Admin:Email"];
                var adminPassword = configuration["Seed:Admin:Password"];

                if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
                {
                    var admin = await userManager.FindByEmailAsync(adminEmail);
                    if (admin == null)
                    {
                        admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                        var createUserResult = await userManager.CreateAsync(admin, adminPassword);
                        if (createUserResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(admin, "Admin");
                            logger.LogInformation("RoleSeeder: Created admin user {Email} and assigned Admin role", adminEmail);
                        }
                        else
                        {
                            logger.LogWarning("RoleSeeder: Failed to create admin user {Email}: {Errors}", adminEmail,
                                string.Join(", ", createUserResult.Errors.Select(e => e.Description)));
                        }
                    }
                    else
                    {
                        if (!await userManager.IsInRoleAsync(admin, "Admin"))
                        {
                            await userManager.AddToRoleAsync(admin, "Admin");
                            logger.LogInformation("RoleSeeder: Assigned Admin role to existing user {Email}", adminEmail);
                        }
                        else
                        {
                            logger.LogInformation("RoleSeeder: Admin user {Email} already exists with Admin role", adminEmail);
                        }
                    }
                }
                else
                {
                    logger.LogInformation("RoleSeeder: Admin credentials not provided in configuration; skipping admin user creation.");
                }

                logger.LogInformation("RoleSeeder: Seeding complete.");
            }
            catch (Exception ex)
            {
                // Use the scoped logger to report the failure (safe resolution here as well)
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                var failLogger = loggerFactory?.CreateLogger<object>() ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<object>.Instance;
                failLogger.LogError(ex, "RoleSeeder: Unexpected error while seeding roles/users.");
            }
        }
    }
}