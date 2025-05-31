using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.PL.Models;

namespace TheatreTicketSystem.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallsController : ControllerBase
    {
        private readonly HallService _hallService;
        private readonly SeatService _seatService;
        private readonly IMapper _mapper;

        public HallsController(HallService hallService, SeatService seatService, IMapper mapper)
        {
            _hallService = hallService;
            _seatService = seatService;
            _mapper = mapper;
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
                var hallDtos = _mapper.Map<IEnumerable<HallListDto>>(halls);
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

                var hallDto = _mapper.Map<HallDto>(hall);
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
                var seatDtos = _mapper.Map<IEnumerable<SeatDto>>(seats);

                // Встановлюємо доступність місць (для залу без конкретної вистави всі місця доступні)
                foreach (var seatDto in seatDtos)
                {
                    seatDto.IsAvailable = true;
                }

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

                var hallDto = _mapper.Map<HallDto>(hall);
                var performanceDtos = _mapper.Map<IEnumerable<PerformanceListDto>>(hall.Performances);

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
    }
}