using AutoMapper;
using UtiliSense.shared.DTOs;
using UtiliSense.data.Models;

namespace UtiliSense.shared.Profiles
{
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
}
