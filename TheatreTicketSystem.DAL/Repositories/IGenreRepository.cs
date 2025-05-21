using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Genre GetWithPerformances(int id);
    }
}