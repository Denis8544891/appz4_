using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    // Інтерфейс для роботи з залами
    public interface IHallRepository : IRepository<Hall>
    {
        Hall GetWithSeats(int id);
        Hall GetWithPerformances(int id);
        Hall GetWithDetails(int id);
    }
}