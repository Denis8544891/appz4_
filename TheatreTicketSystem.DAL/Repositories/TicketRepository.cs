using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(TheatreDbContext context) : base(context)
        {
        }

        public IEnumerable<Ticket> GetTicketsForPerformance(int performanceId)
        {
            return Context.Tickets
                .Include(t => t.Seat)
                .Where(t => t.PerformanceId == performanceId)
                .ToList();
        }

        public IEnumerable<Ticket> GetAvailableTicketsForPerformance(int performanceId)
        {
            return Context.Tickets
                .Include(t => t.Seat)
                .Where(t => t.PerformanceId == performanceId && !t.IsSold)
                .ToList();
        }

        public IEnumerable<Ticket> GetSoldTicketsForPerformance(int performanceId)
        {
            return Context.Tickets
                .Include(t => t.Seat)
                .Where(t => t.PerformanceId == performanceId && t.IsSold)
                .ToList();
        }

        public Ticket GetTicketWithDetails(int ticketId)
        {
            return Context.Tickets
                .Include(t => t.Performance)
                    .ThenInclude(p => p.Author)
                .Include(t => t.Performance)
                    .ThenInclude(p => p.Genre)
                .Include(t => t.Performance)
                    .ThenInclude(p => p.Hall)
                .Include(t => t.Seat)
                .SingleOrDefault(t => t.Id == ticketId);
        }
    }
}