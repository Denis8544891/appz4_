using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    public interface ISeatRepository : IRepository<Seat>
    {
        IEnumerable<Seat> GetSeatsForHall(int hallId);
        IEnumerable<Seat> GetAvailableSeatsForPerformance(int performanceId);
    }
}