using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.PL.Models;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly SeatService _seatService;

        public TicketsController(TicketService ticketService, SeatService seatService)
        {
            _ticketService = ticketService;
            _seatService = seatService;
        }

        /// <summary>
        /// Отримати квиток за ID
        /// </summary>
        /// <param name="id">ID квитка</param>
        /// <returns>Детальна інформація про квиток</returns>
        [HttpGet("{id}")]
        public ActionResult<TicketDto> GetTicketById(int id)
        {
            try
            {
                var ticket = _ticketService.GetTicketWithDetails(id);
                if (ticket == null)
                {
                    return NotFound(new { message = $"Квиток з ID {id} не знайдений" });
                }

                var ticketDto = MapTicketToDto(ticket);
                return Ok(ticketDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати квитки для вистави
        /// </summary>
        /// <param name="performanceId">ID вистави</param>
        /// <returns>Список квитків для вказаної вистави</returns>
        [HttpGet("performance/{performanceId}")]
        public ActionResult<IEnumerable<TicketDto>> GetTicketsForPerformance(int performanceId)
        {
            try
            {
                var tickets = _ticketService.GetTicketsForPerformance(performanceId);
                var ticketDtos = tickets.Select(MapTicketToDto);
                return Ok(ticketDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати доступні квитки для вистави
        /// </summary>
        /// <param name="performanceId">ID вистави</param>
        /// <returns>Список доступних квитків для вказаної вистави</returns>
        [HttpGet("performance/{performanceId}/available")]
        public ActionResult<IEnumerable<TicketDto>> GetAvailableTicketsForPerformance(int performanceId)
        {
            try
            {
                var tickets = _ticketService.GetAvailableTicketsForPerformance(performanceId);
                var ticketDtos = tickets.Select(MapTicketToDto);
                return Ok(ticketDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати продані квитки для вистави
        /// </summary>
        /// <param name="performanceId">ID вистави</param>
        /// <returns>Список проданих квитків для вказаної вистави</returns>
        [HttpGet("performance/{performanceId}/sold")]
        public ActionResult<IEnumerable<TicketDto>> GetSoldTicketsForPerformance(int performanceId)
        {
            try
            {
                var tickets = _ticketService.GetSoldTicketsForPerformance(performanceId);
                var ticketDtos = tickets.Select(MapTicketToDto);
                return Ok(ticketDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати доступні місця для вистави
        /// </summary>
        /// <param name="performanceId">ID вистави</param>
        /// <returns>Список доступних місць для вказаної вистави</returns>
        [HttpGet("performance/{performanceId}/available-seats")]
        public ActionResult<IEnumerable<SeatDto>> GetAvailableSeatsForPerformance(int performanceId)
        {
            try
            {
                var seats = _seatService.GetAvailableSeatsForPerformance(performanceId);
                var seatDtos = seats.Select(seat => MapSeatToDto(seat, true));
                return Ok(seatDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати статистику продажу квитків для вистави
        /// </summary>
        /// <param name="performanceId">ID вистави</param>
        /// <returns>Статистика продажу квитків</returns>
        [HttpGet("performance/{performanceId}/statistics")]
        public ActionResult<object> GetTicketStatistics(int performanceId)
        {
            try
            {
                var allTickets = _ticketService.GetTicketsForPerformance(performanceId);
                var soldTickets = allTickets.Where(t => t.IsSold);

                var statistics = new
                {
                    TotalTickets = allTickets.Count(),
                    SoldTickets = soldTickets.Count(),
                    AvailableTickets = allTickets.Count() - soldTickets.Count(),
                    TotalRevenue = soldTickets.Sum(t => t.Price),
                    AverageTicketPrice = allTickets.Any() ? allTickets.Average(t => t.Price) : 0,
                    OccupancyRate = allTickets.Any() ? Math.Round((double)soldTickets.Count() / allTickets.Count() * 100, 2) : 0
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати загальну статистику продажів по всіх виставах
        /// </summary>
        /// <returns>Загальна статистика продажів</returns>
        [HttpGet("statistics/overall")]
        public ActionResult<object> GetOverallStatistics()
        {
            try
            {
                var allTickets = _ticketService.GetAllTickets();
                var soldTickets = allTickets.Where(t => t.IsSold);

                var statistics = new
                {
                    TotalTickets = allTickets.Count(),
                    SoldTickets = soldTickets.Count(),
                    AvailableTickets = allTickets.Count() - soldTickets.Count(),
                    TotalRevenue = soldTickets.Sum(t => t.Price),
                    AverageTicketPrice = allTickets.Any() ? allTickets.Average(t => t.Price) : 0,
                    OverallOccupancyRate = allTickets.Any() ? Math.Round((double)soldTickets.Count() / allTickets.Count() * 100, 2) : 0
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати квитки з фільтрацією за ціною
        /// </summary>
        /// <param name="performanceId">ID вистави</param>
        /// <param name="minPrice">Мінімальна ціна</param>
        /// <param name="maxPrice">Максимальна ціна</param>
        /// <returns>Список квитків у вказаному ціновому діапазоні</returns>
        [HttpGet("performance/{performanceId}/by-price")]
        public ActionResult<IEnumerable<TicketDto>> GetTicketsByPriceRange(int performanceId, [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null)
        {
            try
            {
                var tickets = _ticketService.GetTicketsForPerformance(performanceId);

                if (minPrice.HasValue)
                {
                    tickets = tickets.Where(t => t.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    tickets = tickets.Where(t => t.Price <= maxPrice.Value);
                }

                var ticketDtos = tickets.Select(MapTicketToDto);
                return Ok(ticketDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати квитки VIP класу для вистави
        /// </summary>
        /// <param name="performanceId">ID вистави</param>
        /// <returns>Список VIP квитків</returns>
        [HttpGet("performance/{performanceId}/vip")]
        public ActionResult<IEnumerable<TicketDto>> GetVIPTicketsForPerformance(int performanceId)
        {
            try
            {
                var tickets = _ticketService.GetTicketsForPerformance(performanceId);
                var vipTickets = tickets.Where(t => t.Seat.IsVIP);
                var ticketDtos = vipTickets.Select(MapTicketToDto);
                return Ok(ticketDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати квитки за рядом
        /// </summary>
        /// <param name="performanceId">ID вистави</param>
        /// <param name="row">Номер ряду</param>
        /// <returns>Список квитків у вказаному ряду</returns>
        [HttpGet("performance/{performanceId}/row/{row}")]
        public ActionResult<IEnumerable<TicketDto>> GetTicketsByRow(int performanceId, int row)
        {
            try
            {
                var tickets = _ticketService.GetTicketsForPerformance(performanceId);
                var rowTickets = tickets.Where(t => t.Seat.Row == row).OrderBy(t => t.Seat.Number);
                var ticketDtos = rowTickets.Select(MapTicketToDto);
                return Ok(ticketDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати детальну інформацію про схему залу з доступністю місць для вистави
        /// </summary>
        /// <param name="performanceId">ID вистави</param>
        /// <returns>Схема залу з позначенням доступних/зайнятих місць</returns>
        [HttpGet("performance/{performanceId}/seating-plan")]
        public ActionResult<object> GetSeatingPlan(int performanceId)
        {
            try
            {
                var allTickets = _ticketService.GetTicketsForPerformance(performanceId);

                var seatingPlan = allTickets
                    .GroupBy(t => t.Seat.Row)
                    .OrderBy(g => g.Key)
                    .Select(rowGroup => new
                    {
                        Row = rowGroup.Key,
                        Seats = rowGroup
                            .OrderBy(t => t.Seat.Number)
                            .Select(t => new
                            {
                                SeatNumber = t.Seat.Number,
                                IsVIP = t.Seat.IsVIP,
                                IsAvailable = !t.IsSold,
                                Price = t.Price,
                                TicketId = t.Id
                            })
                            .ToList()
                    })
                    .ToList();

                var summary = new
                {
                    PerformanceId = performanceId,
                    TotalSeats = allTickets.Count(),
                    AvailableSeats = allTickets.Count(t => !t.IsSold),
                    SoldSeats = allTickets.Count(t => t.IsSold),
                    VIPSeats = allTickets.Count(t => t.Seat.IsVIP),
                    SeatingPlan = seatingPlan
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        // Приватні методи для маппінгу
        private TicketDto MapTicketToDto(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                PurchaseDate = ticket.PurchaseDate,
                Price = ticket.Price,
                IsSold = ticket.IsSold,
                Seat = ticket.Seat != null ? MapSeatToDto(ticket.Seat) : null,
                Performance = ticket.Performance != null ? MapPerformanceToListDto(ticket.Performance) : null
            };
        }

        private SeatDto MapSeatToDto(Seat seat, bool isAvailable = false)
        {
            return new SeatDto
            {
                Id = seat.Id,
                Row = seat.Row,
                Number = seat.Number,
                IsVIP = seat.IsVIP,
                IsAvailable = isAvailable
            };
        }

        private PerformanceListDto MapPerformanceToListDto(Performance performance)
        {
            return new PerformanceListDto
            {
                Id = performance.Id,
                Title = performance.Title ?? "Без назви",
                Description = performance.Description ?? "",
                PerformanceDate = performance.PerformanceDate,
                BasePrice = performance.BasePrice,
                AuthorName = performance.Author?.FullName ?? "Невідомий автор",
                GenreName = performance.Genre?.Name ?? "Невідомий жанр",
                HallName = performance.Hall?.Name ?? "Невідомий зал"
            };
        }
    }
}