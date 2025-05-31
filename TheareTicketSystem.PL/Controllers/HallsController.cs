using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.PL.Models;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallsController : ControllerBase
    {
        private readonly HallService _hallService;
        private readonly SeatService _seatService;

        public HallsController(HallService hallService, SeatService seatService)
        {
            _hallService = hallService;
            _seatService = seatService;
        }

        /// <summary>
        /// Отримати всі зали
        /// </summary>
        /// <returns>Список всіх залів</returns>
        [HttpGet]
        public ActionResult<IEnumerable<HallListDto>> GetAllHalls()
        {
            try
            {
                var halls = _hallService.GetAllHalls();
                var hallDtos = halls.Select(MapHallToListDto);
                return Ok(hallDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати зал за ID
        /// </summary>
        /// <param name="id">ID залу</param>
        /// <returns>Детальна інформація про зал</returns>
        [HttpGet("{id}")]
        public ActionResult<HallDto> GetHallById(int id)
        {
            try
            {
                var hall = _hallService.GetHallById(id);
                if (hall == null)
                {
                    return NotFound(new { message = $"Зал з ID {id} не знайдений" });
                }

                var hallDto = MapHallToDto(hall);
                return Ok(hallDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати місця в залі
        /// </summary>
        /// <param name="id">ID залу</param>
        /// <returns>Список місць у залі</returns>
        [HttpGet("{id}/seats")]
        public ActionResult<IEnumerable<SeatDto>> GetHallSeats(int id)
        {
            try
            {
                var hall = _hallService.GetHallById(id);
                if (hall == null)
                {
                    return NotFound(new { message = $"Зал з ID {id} не знайдений" });
                }

                var seats = _seatService.GetSeatsForHall(id);
                var seatDtos = seats.Select(seat => MapSeatToDto(seat, true));

                return Ok(seatDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати зал з виставами
        /// </summary>
        /// <param name="id">ID залу</param>
        /// <returns>Зал з переліком вистав, що проходять у ньому</returns>
        [HttpGet("{id}/with-performances")]
        public ActionResult<object> GetHallWithPerformances(int id)
        {
            try
            {
                var hall = _hallService.GetHallWithPerformances(id);
                if (hall == null)
                {
                    return NotFound(new { message = $"Зал з ID {id} не знайдений" });
                }

                var hallDto = MapHallToDto(hall);
                var performanceDtos = hall.Performances?.Select(MapPerformanceToListDto) ?? new List<PerformanceListDto>();

                var result = new
                {
                    Hall = hallDto,
                    Performances = performanceDtos
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        // Приватні методи для маппінгу
        private HallListDto MapHallToListDto(Hall hall)
        {
            return new HallListDto
            {
                Id = hall.Id,
                Name = hall.Name ?? "Невідомий зал",
                Capacity = hall.Capacity,
                PerformancesCount = hall.Performances?.Count() ?? 0
            };
        }

        private HallDto MapHallToDto(Hall hall)
        {
            return new HallDto
            {
                Id = hall.Id,
                Name = hall.Name ?? "Невідомий зал",
                Capacity = hall.Capacity,
                Description = hall.Description ?? ""
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