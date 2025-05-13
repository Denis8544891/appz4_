
using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Entities;
using TheatreTicketSystem.DAL.Repositories;

namespace TheatreTicketSystem.BLL.Services
{
    public class SeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly TheatreDbContext _context;

        public SeatService(ISeatRepository seatRepository, TheatreDbContext context)
        {
            _seatRepository = seatRepository;
            _context = context;
        }

        public IEnumerable<Seat> GetAllSeats()
        {
            return _seatRepository.GetAll();
        }

        public Seat GetSeatById(int id)
        {
            return _seatRepository.SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<Seat> GetSeatsForHall(int hallId)
        {
            return _seatRepository.GetSeatsForHall(hallId);
        }

        public IEnumerable<Seat> GetAvailableSeatsForPerformance(int performanceId)
        {
            return _seatRepository.GetAvailableSeatsForPerformance(performanceId);
        }

        // Метод для створення місць в залі за схемою
        public void CreateSeatsForHall(int hallId, int rows, int seatsPerRow, List<(int row, int number)> vipSeats = null)
        {
            var seats = new List<Seat>();

            for (int row = 1; row <= rows; row++)
            {
                for (int number = 1; number <= seatsPerRow; number++)
                {
                    bool isVip = vipSeats != null && vipSeats.Contains((row, number));

                    seats.Add(new Seat
                    {
                        HallId = hallId,
                        Row = row,
                        Number = number,
                        IsVIP = isVip
                    });
                }
            }

            _seatRepository.AddRange(seats);
            _context.SaveChanges();
        }

        public void AddSeat(Seat seat)
        {
            _seatRepository.Add(seat);
            _context.SaveChanges();
        }

        public void UpdateSeat(Seat seat)
        {
            _seatRepository.Update(seat);
            _context.SaveChanges();
        }

        public void DeleteSeat(int id)
        {
            var seat = _seatRepository.SingleOrDefault(s => s.Id == id);
            if (seat != null)
            {
                _seatRepository.Remove(seat);
                _context.SaveChanges();
            }
        }
    }
}