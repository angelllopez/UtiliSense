using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UtiliSense.data;
using UtiliSense.service;
using UtiliSense.service.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Configuration keys used:
// - ConnectionStrings:DefaultConnection
// - Jwt:Key
// - Jwt:Issuer
// - Jwt:Audience
// - Seed:Admin:Email (optional)
// - Seed:Admin:Password (optional)

// 1. DbContext
builder.Services.AddDbContext<UtiliSenseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity (stores use UtiliSenseDbContext)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Adjust identity options as needed
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<UtiliSenseDbContext>()
.AddDefaultTokenProviders();

// FluentValidation
builder.Services.AddScoped<IValidator<UtiliSense.shared.DTOs.GasMeterReadingDto>, UtiliSense.shared.Validation.GasMeterReadingDtoValidator>();
builder.Services.AddScoped<IValidator<UtiliSense.shared.DTOs.GasBillingDetailDto>, UtiliSense.shared.Validation.GasBillingDetailDtoValidator>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

// 3. Authentication - JWT Bearer
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
{
    // In production you must provide a key via secure configuration (Key Vault / env var).
    throw new InvalidOperationException("JWT configuration missing: Jwt:Key must be set.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// 4. Authorization policies (RBAC)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUser", policy => policy.RequireRole("User", "Admin"));
});

// 5. Application services
builder.Services.AddScoped<IGasDataService, GasDataService>();

// 6. MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UtiliSense API", Version = "v1" });

    // JWT bearer in Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer token as: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new[] { "Bearer" } }
    });
});

var app = builder.Build();

// Seed roles (and optional admin user) at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            var exists = await roleManager.RoleExistsAsync(role);
            if (!exists)
            {
                var createResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (!createResult.Succeeded)
                {
                    logger.LogWarning("Role creation failed for {Role}: {Errors}", role, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }
            }
        }

        // Optional admin seeding from configuration
        var adminEmail = builder.Configuration["Seed:Admin:Email"];
        var adminPassword = builder.Configuration["Seed:Admin:Password"];
        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
        {
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (createUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Seeded admin user: {Email}", adminEmail);
                }
                else
                {
                    logger.LogWarning("Failed to create admin user: {Errors}", string.Join(", ", createUserResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                // Ensure admin role assigned
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
    catch (Exception ex)
    {
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        loggerFactory.CreateLogger<Program>().LogError(ex, "An error occurred seeding roles.");
    }
}

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
