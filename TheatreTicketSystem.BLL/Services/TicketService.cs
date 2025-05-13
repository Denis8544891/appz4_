using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Entities;
using TheatreTicketSystem.DAL.Repositories;

namespace TheatreTicketSystem.BLL.Services
{
    public class TicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IPerformanceRepository _performanceRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly TheatreDbContext _context;

        public TicketService(
            ITicketRepository ticketRepository,
            IPerformanceRepository performanceRepository,
            ISeatRepository seatRepository,
            TheatreDbContext context)
        {
            _ticketRepository = ticketRepository;
            _performanceRepository = performanceRepository;
            _seatRepository = seatRepository;
            _context = context;
        }

        public IEnumerable<Ticket> GetAllTickets()
        {
            return _ticketRepository.GetAll();
        }

        public Ticket GetTicketById(int id)
        {
            return _ticketRepository.SingleOrDefault(t => t.Id == id);
        }

        public Ticket GetTicketWithDetails(int id)
        {
            return _ticketRepository.GetTicketWithDetails(id);
        }

        public IEnumerable<Ticket> GetTicketsForPerformance(int performanceId)
        {
            return _ticketRepository.GetTicketsForPerformance(performanceId);
        }

        public IEnumerable<Ticket> GetAvailableTicketsForPerformance(int performanceId)
        {
            return _ticketRepository.GetAvailableTicketsForPerformance(performanceId);
        }

        public IEnumerable<Ticket> GetSoldTicketsForPerformance(int performanceId)
        {
            return _ticketRepository.GetSoldTicketsForPerformance(performanceId);
        }

        // Метод для створення квитків на виставу
        public void CreateTicketsForPerformance(int performanceId)
        {
            var performance = _performanceRepository.GetPerformanceWithDetails(performanceId);
            if (performance == null)
                throw new InvalidOperationException("Вистава не знайдена");

            var seats = _seatRepository.GetSeatsForHall(performance.HallId);
            var tickets = new List<Ticket>();

            foreach (var seat in seats)
            {
                var price = performance.BasePrice;
                // VIP-місця коштують дорожче
                if (seat.IsVIP)
                {
                    price *= 1.5m;
                }

                tickets.Add(new Ticket
                {
                    PerformanceId = performanceId,
                    SeatId = seat.Id,
                    Price = price,
                    IsSold = false,
                    PurchaseDate = null
                });
            }

            _ticketRepository.AddRange(tickets);
            _context.SaveChanges();
        }

        // Метод для продажу квитка
        public bool SellTicket(int ticketId)
        {
            var ticket = _ticketRepository.GetTicketWithDetails(ticketId);
            if (ticket == null || ticket.IsSold)
                return false;

            ticket.IsSold = true;
            ticket.PurchaseDate = DateTime.Now;
            _ticketRepository.Update(ticket);
            _context.SaveChanges();
            return true;
        }

        // Метод для повернення квитка
        public bool ReturnTicket(int ticketId)
        {
            var ticket = _ticketRepository.GetTicketWithDetails(ticketId);
            if (ticket == null || !ticket.IsSold)
                return false;

            // Перевіряємо, чи не пізніше ніж за день до вистави
            if (ticket.Performance.PerformanceDate.AddDays(-1) < DateTime.Now)
                return false;

            ticket.IsSold = false;
            ticket.PurchaseDate = DateTime.MinValue; // або використовуйте значення за замовчуванням замість null
            _ticketRepository.Update(ticket);
            _context.SaveChanges();
            return true;
        }

        public void AddTicket(Ticket ticket)
        {
            _ticketRepository.Add(ticket);
            _context.SaveChanges();
        }

        public void UpdateTicket(Ticket ticket)
        {
            _ticketRepository.Update(ticket);
            _context.SaveChanges();
        }

        public void DeleteTicket(int id)
        {
            var ticket = _ticketRepository.SingleOrDefault(t => t.Id == id);
            if (ticket != null)
            {
                _ticketRepository.Remove(ticket);
                _context.SaveChanges();
            }
        }
    }
}