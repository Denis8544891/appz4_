using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    // Репозиторій для роботи з жанрами
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(TheatreDbContext context) : base(context)
        {
        }

        public Genre GetWithPerformances(int id)
        {
            // Жадібне завантаження - одразу завантажуємо пов'язані вистави
            return Context.Genres
                .Include(g => g.Performances)
                .SingleOrDefault(g => g.Id == id);
        }
    }
}