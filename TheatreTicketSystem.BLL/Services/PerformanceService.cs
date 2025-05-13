using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Entities;
using TheatreTicketSystem.DAL.Repositories;

namespace TheatreTicketSystem.BLL.Services
{
    public class PerformanceService
    {
        private readonly IPerformanceRepository _performanceRepository;
        private readonly TheatreDbContext _context;

        public PerformanceService(IPerformanceRepository performanceRepository, TheatreDbContext context)
        {
            _performanceRepository = performanceRepository;
            _context = context;
        }

        public IEnumerable<Performance> GetAllPerformances()
        {
            return _performanceRepository.GetAll();
        }

        public IEnumerable<Performance> GetAllPerformancesWithDetails()
        {
            return _performanceRepository.GetPerformancesWithDetails();
        }

        public Performance GetPerformanceById(int id)
        {
            return _performanceRepository.SingleOrDefault(p => p.Id == id);
        }

        public Performance GetPerformanceWithDetails(int id)
        {
            return _performanceRepository.GetPerformanceWithDetails(id);
        }

        public IEnumerable<Performance> GetUpcomingPerformances()
        {
            return _performanceRepository.GetUpcomingPerformances();
        }

        public IEnumerable<Performance> GetPerformancesByGenre(int genreId)
        {
            return _performanceRepository.GetPerformancesByGenre(genreId);
        }

        public IEnumerable<Performance> GetPerformancesByAuthor(int authorId)
        {
            return _performanceRepository.GetPerformancesByAuthor(authorId);
        }

        public void AddPerformance(Performance performance)
        {
            _performanceRepository.Add(performance);
            _context.SaveChanges();
        }

        public void UpdatePerformance(Performance performance)
        {
            _performanceRepository.Update(performance);
            _context.SaveChanges();
        }

        public void DeletePerformance(int id)
        {
            var performance = _performanceRepository.SingleOrDefault(p => p.Id == id);
            if (performance != null)
            {
                _performanceRepository.Remove(performance);
                _context.SaveChanges();
            }
        }
    }
}