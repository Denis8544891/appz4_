using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.PL.Models;

namespace TheatreTicketSystem.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerformancesController : ControllerBase
    {
        private readonly PerformanceService _performanceService;
        private readonly IMapper _mapper;

        public PerformancesController(PerformanceService performanceService, IMapper mapper)
        {
            _performanceService = performanceService;
            _mapper = mapper;
        }

        /// <summary>
        /// Отримати всі вистави
        /// </summary>
        /// <returns>Список всіх вистав</returns>
        [HttpGet]
        public ActionResult<IEnumerable<PerformanceListDto>> GetAllPerformances()
        {
            try
            {
                var performances = _performanceService.GetAllPerformancesWithDetails();
                var performanceDtos = _mapper.Map<IEnumerable<PerformanceListDto>>(performances);
                return Ok(performanceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати виставу за ID
        /// </summary>
        /// <param name="id">ID вистави</param>
        /// <returns>Детальна інформація про виставу</returns>
        [HttpGet("{id}")]
        public ActionResult<PerformanceDetailDto> GetPerformanceById(int id)
        {
            try
            {
                var performance = _performanceService.GetPerformanceWithDetails(id);
                if (performance == null)
                {
                    return NotFound(new { message = $"Вистава з ID {id} не знайдена" });
                }

                var performanceDto = _mapper.Map<PerformanceDetailDto>(performance);
                return Ok(performanceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати майбутні вистави
        /// </summary>
        /// <returns>Список майбутніх вистав</returns>
        [HttpGet("upcoming")]
        public ActionResult<IEnumerable<PerformanceListDto>> GetUpcomingPerformances()
        {
            try
            {
                var performances = _performanceService.GetUpcomingPerformances();
                var performanceDtos = _mapper.Map<IEnumerable<PerformanceListDto>>(performances);
                return Ok(performanceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати вистави за жанром
        /// </summary>
        /// <param name="genreId">ID жанру</param>
        /// <returns>Список вистав вказаного жанру</returns>
        [HttpGet("by-genre/{genreId}")]
        public ActionResult<IEnumerable<PerformanceListDto>> GetPerformancesByGenre(int genreId)
        {
            try
            {
                var performances = _performanceService.GetPerformancesByGenre(genreId);
                var performanceDtos = _mapper.Map<IEnumerable<PerformanceListDto>>(performances);
                return Ok(performanceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати вистави за автором
        /// </summary>
        /// <param name="authorId">ID автора</param>
        /// <returns>Список вистав вказаного автора</returns>
        [HttpGet("by-author/{authorId}")]
        public ActionResult<IEnumerable<PerformanceListDto>> GetPerformancesByAuthor(int authorId)
        {
            try
            {
                var performances = _performanceService.GetPerformancesByAuthor(authorId);
                var performanceDtos = _mapper.Map<IEnumerable<PerformanceListDto>>(performances);
                return Ok(performanceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }
    }
}