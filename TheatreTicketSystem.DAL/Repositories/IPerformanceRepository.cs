using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    public interface IPerformanceRepository : IRepository<Performance>
    {
        IEnumerable<Performance> GetPerformancesWithDetails();
        Performance GetPerformanceWithDetails(int id);
        IEnumerable<Performance> GetPerformancesByGenre(int genreId);
        IEnumerable<Performance> GetPerformancesByAuthor(int authorId);
        IEnumerable<Performance> GetUpcomingPerformances();
    }
}