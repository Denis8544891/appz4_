using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(TheatreDbContext context) : base(context)
        {
        }

        public IEnumerable<Author> GetAuthorsWithPerformances()
        {
            // Жадібне завантаження - одразу завантажуємо пов'язані вистави
            return Context.Authors.Include(a => a.Performances).ToList();
        }

        public Author GetAuthorWithPerformances(int id)
        {
            // Жадібне завантаження - одразу завантажуємо пов'язані вистави для конкретного автора
            return Context.Authors
                .Include(a => a.Performances)
                .SingleOrDefault(a => a.Id == id);
        }
    }
}