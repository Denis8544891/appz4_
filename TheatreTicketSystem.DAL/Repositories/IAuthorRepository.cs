using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        IEnumerable<Author> GetAuthorsWithPerformances();
        Author GetAuthorWithPerformances(int id);
    }
}