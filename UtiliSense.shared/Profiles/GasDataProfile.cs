using AutoMapper;
using UtiliSense.shared.DTOs;
using UtiliSense.data.Models;

namespace UtiliSense.shared.Profiles;

/// <summary>
/// Provides AutoMapper configuration for mapping between GasMeterReading and GasMeterReadingDto types.
/// </summary>
/// <remarks>This profile defines mappings for converting gas meter reading domain entities to their corresponding
/// data transfer objects and vice versa. It should be registered with AutoMapper to enable automatic object mapping in
/// applications that process gas meter readings.</remarks>
public class GasMeterReadingProfile : Profile
{
    public GasMeterReadingProfile()
    {
        CreateMap<GasMeterReading, GasMeterReadingDto>()
            .ForMember(d => d.MeterReadDate, opt => opt.MapFrom(s => s.MeterReadDate))
            .ForMember(d => d.Consumption, opt => opt.MapFrom(s => (decimal)s.Consumption))
            .ForMember(d => d.AvgTemperature, opt => opt.MapFrom(s => s.AvgTemperature));

        CreateMap<GasMeterReadingDto, GasMeterReading>()
            .ForMember(d => d.MeterReadDate, opt => opt.MapFrom(s => s.MeterReadDate))
            .ForMember(d => d.Consumption, opt => opt.MapFrom(s => (double)s.Consumption))
            .ForMember(d => d.AvgTemperature, opt => opt.MapFrom(s => s.AvgTemperature));
    }
}
