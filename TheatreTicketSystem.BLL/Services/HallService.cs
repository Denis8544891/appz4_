
using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Entities;
using TheatreTicketSystem.DAL.Repositories;

namespace TheatreTicketSystem.BLL.Services
{
    public class HallService
    {
        private readonly IHallRepository _hallRepository;
        private readonly TheatreDbContext _context;

        public HallService(IHallRepository hallRepository, TheatreDbContext context)
        {
            _hallRepository = hallRepository;
            _context = context;
        }

        public IEnumerable<Hall> GetAllHalls()
        {
            return _hallRepository.GetAll();
        }

        public Hall GetHallById(int id)
        {
            return _hallRepository.SingleOrDefault(h => h.Id == id);
        }

        public Hall GetHallWithSeats(int id)
        {
            return _hallRepository.GetWithSeats(id);
        }

        public Hall GetHallWithPerformances(int id)
        {
            return _hallRepository.GetWithPerformances(id);
        }

        public Hall GetHallWithDetails(int id)
        {
            return _hallRepository.GetWithDetails(id);
        }

        public void AddHall(Hall hall)
        {
            _hallRepository.Add(hall);
            _context.SaveChanges();
        }

        public void UpdateHall(Hall hall)
        {
            _hallRepository.Update(hall);
            _context.SaveChanges();
        }

        public void DeleteHall(int id)
        {
            var hall = _hallRepository.SingleOrDefault(h => h.Id == id);
            if (hall != null)
            {
                _hallRepository.Remove(hall);
                _context.SaveChanges();
            }
        }
    }
}