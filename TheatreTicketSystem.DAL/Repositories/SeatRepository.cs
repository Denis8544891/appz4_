using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL.Repositories
{
    // Репозиторій для роботи з місцями
    public class SeatRepository : Repository<Seat>, ISeatRepository
    {
        public SeatRepository(TheatreDbContext context) : base(context)
        {
        }

        public IEnumerable<Seat> GetSeatsForHall(int hallId)
        {
            // Отримуємо всі місця для конкретного залу
            return Context.Seats
                .Where(s => s.HallId == hallId)
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Number)
                .ToList();
        }

        public IEnumerable<Seat> GetAvailableSeatsForPerformance(int performanceId)
        {
            // Отримуємо доступні місця для конкретної вистави
            // Спочатку знаходимо зал, де проводиться вистава
            var performance = Context.Performances.FirstOrDefault(p => p.Id == performanceId);
            if (performance == null)
                return Enumerable.Empty<Seat>();

            // Знаходимо всі місця в залі
            var hallSeats = Context.Seats
                .Where(s => s.HallId == performance.HallId)
                .ToList();

            // Знаходимо всі продані квитки для цієї вистави
            var soldTickets = Context.Tickets
                .Where(t => t.PerformanceId == performanceId && t.IsSold)
                .Select(t => t.SeatId)
                .ToList();

            // Повертаємо місця, для яких немає проданих квитків
            return hallSeats.Where(s => !soldTickets.Contains(s.Id)).ToList();
        }
    }
}