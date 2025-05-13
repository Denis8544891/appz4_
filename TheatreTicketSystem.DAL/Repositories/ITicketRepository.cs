using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        IEnumerable<Ticket> GetTicketsForPerformance(int performanceId);
        IEnumerable<Ticket> GetAvailableTicketsForPerformance(int performanceId);
        IEnumerable<Ticket> GetSoldTicketsForPerformance(int performanceId);
        Ticket GetTicketWithDetails(int ticketId);
    }
}