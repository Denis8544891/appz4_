using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    public class PerformanceRepository : Repository<Performance>, IPerformanceRepository
    {
        public PerformanceRepository(TheatreDbContext context) : base(context)
        {
        }

        public IEnumerable<Performance> GetPerformancesWithDetails()
        {
            // Жадібне завантаження - одразу завантажуємо всі пов'язані сутності
            return Context.Performances
                .Include(p => p.Author)
                .Include(p => p.Genre)
                .Include(p => p.Hall)
                .ToList();
        }

        public Performance GetPerformanceWithDetails(int id)
        {
            // Жадібне завантаження - одразу завантажуємо всі пов'язані сутності для конкретної вистави
            return Context.Performances
                .Include(p => p.Author)
                .Include(p => p.Genre)
                .Include(p => p.Hall)
                .Include(p => p.Tickets)
                    .ThenInclude(t => t.Seat)
                .SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Performance> GetPerformancesByGenre(int genreId)
        {
            return Context.Performances
                .Include(p => p.Author)
                .Include(p => p.Hall)
                .Where(p => p.GenreId == genreId)
                .OrderBy(p => p.PerformanceDate)
                .ToList();
        }

        public IEnumerable<Performance> GetPerformancesByAuthor(int authorId)
        {
            return Context.Performances
                .Include(p => p.Genre)
                .Include(p => p.Hall)
                .Where(p => p.AuthorId == authorId)
                .OrderBy(p => p.PerformanceDate)
                .ToList();
        }

        public IEnumerable<Performance> GetUpcomingPerformances()
        {
            var today = DateTime.Today;
            return Context.Performances
                .Include(p => p.Author)
                .Include(p => p.Genre)
                .Include(p => p.Hall)
                .Where(p => p.PerformanceDate >= today)
                .OrderBy(p => p.PerformanceDate)
                .ToList();
        }
    }
}