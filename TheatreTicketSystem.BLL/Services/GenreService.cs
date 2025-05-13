using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Entities;
using TheatreTicketSystem.DAL.Repositories;

namespace TheatreTicketSystem.BLL.Services
{
    public class GenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly TheatreDbContext _context;

        public GenreService(IGenreRepository genreRepository, TheatreDbContext context)
        {
            _genreRepository = genreRepository;
            _context = context;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            return _genreRepository.GetAll();
        }

        public Genre GetGenreById(int id)
        {
            return _genreRepository.SingleOrDefault(g => g.Id == id);
        }

        public Genre GetGenreWithPerformances(int id)
        {
            return _genreRepository.GetWithPerformances(id);
        }

        public void AddGenre(Genre genre)
        {
            _genreRepository.Add(genre);
            _context.SaveChanges();
        }

        public void UpdateGenre(Genre genre)
        {
            _genreRepository.Update(genre);
            _context.SaveChanges();
        }

        public void DeleteGenre(int id)
        {
            var genre = _genreRepository.SingleOrDefault(g => g.Id == id);
            if (genre != null)
            {
                _genreRepository.Remove(genre);
                _context.SaveChanges();
            }
        }
    }
}