using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    // Репозиторій для роботи з залами
    public class HallRepository : Repository<Hall>, IHallRepository
    {
        public HallRepository(TheatreDbContext context) : base(context)
        {
        }

        public Hall GetWithSeats(int id)
        {
            // Жадібне завантаження - одразу завантажуємо пов'язані місця
            return Context.Halls
                .Include(h => h.Seats)
                .SingleOrDefault(h => h.Id == id);
        }

        public Hall GetWithPerformances(int id)
        {
            // Жадібне завантаження - одразу завантажуємо пов'язані вистави
            return Context.Halls
                .Include(h => h.Performances)
                .SingleOrDefault(h => h.Id == id);
        }

        public Hall GetWithDetails(int id)
        {
            // Жадібне завантаження - одразу завантажуємо всі пов'язані дані
            return Context.Halls
                .Include(h => h.Seats)
                .Include(h => h.Performances)
                .SingleOrDefault(h => h.Id == id);
        }
    }
}