using AutoMapper;
using TheatreTicketSystem.DAL.Entities;
using TheatreTicketSystem.PL.Models;

namespace TheatreTicketSystem.PL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Мапінг для Performance
            CreateMap<Performance, PerformanceListDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.HallName, opt => opt.MapFrom(src => src.Hall.Name));

            CreateMap<Performance, PerformanceDetailDto>()
                .ForMember(dest => dest.AvailableTickets, opt => opt.MapFrom(src => src.Tickets.Count(t => !t.IsSold)))
                .ForMember(dest => dest.SoldTickets, opt => opt.MapFrom(src => src.Tickets.Count(t => t.IsSold)))
                .ForMember(dest => dest.TotalTickets, opt => opt.MapFrom(src => src.Tickets.Count));

            // Мапінг для Author
            CreateMap<Author, AuthorDto>();
            CreateMap<Author, AuthorListDto>()
                .ForMember(dest => dest.PerformancesCount, opt => opt.MapFrom(src => src.Performances.Count));

            // Мапінг для Genre
            CreateMap<Genre, GenreDto>();
            CreateMap<Genre, GenreListDto>()
                .ForMember(dest => dest.PerformancesCount, opt => opt.MapFrom(src => src.Performances.Count));

            // Мапінг для Hall
            CreateMap<Hall, HallDto>();
            CreateMap<Hall, HallListDto>()
                .ForMember(dest => dest.PerformancesCount, opt => opt.MapFrom(src => src.Performances.Count));

            // Мапінг для Seat
            CreateMap<Seat, SeatDto>()
                .ForMember(dest => dest.IsAvailable, opt => opt.Ignore()); // Буде встановлено в контролері

            // Мапінг для Ticket
            CreateMap<Ticket, TicketDto>();
        }
    }
}