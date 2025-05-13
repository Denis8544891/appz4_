using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    // Інтерфейс для роботи з місцями
    public interface ISeatRepository : IRepository<Seat>
    {
        IEnumerable<Seat> GetSeatsForHall(int hallId);
        IEnumerable<Seat> GetAvailableSeatsForPerformance(int performanceId);
    }
}