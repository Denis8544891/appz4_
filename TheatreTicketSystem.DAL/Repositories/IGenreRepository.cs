using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    // Інтерфейс для роботи з жанрами
    public interface IGenreRepository : IRepository<Genre>
    {
        Genre GetWithPerformances(int id);
    }
}