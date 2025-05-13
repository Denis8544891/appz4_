using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Entities;
using TheatreTicketSystem.DAL.Repositories;

namespace TheatreTicketSystem.BLL.Services
{
    public class AuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly TheatreDbContext _context;

        public AuthorService(IAuthorRepository authorRepository, TheatreDbContext context)
        {
            _authorRepository = authorRepository;
            _context = context;
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _authorRepository.GetAll();
        }

        public Author GetAuthorById(int id)
        {
            return _authorRepository.SingleOrDefault(a => a.Id == id);
        }

        public Author GetAuthorWithPerformances(int id)
        {
            return _authorRepository.GetAuthorWithPerformances(id);
        }

        public void AddAuthor(Author author)
        {
            _authorRepository.Add(author);
            _context.SaveChanges();
        }

        public void UpdateAuthor(Author author)
        {
            _authorRepository.Update(author);
            _context.SaveChanges();
        }

        public void DeleteAuthor(int id)
        {
            var author = _authorRepository.SingleOrDefault(a => a.Id == id);
            if (author != null)
            {
                _authorRepository.Remove(author);
                _context.SaveChanges();
            }
        }
    }
}