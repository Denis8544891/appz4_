using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    public interface IHallRepository : IRepository<Hall>
    {
        Hall GetWithSeats(int id);
        Hall GetWithPerformances(int id);
        Hall GetWithDetails(int id);
    }
}