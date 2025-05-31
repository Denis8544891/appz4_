using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.PL.Models;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerformancesController : ControllerBase
    {
        private readonly PerformanceService _performanceService;

        public PerformancesController(PerformanceService performanceService)
        {
            _performanceService = performanceService;
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
                var performanceDtos = performances.Select(MapPerformanceToListDto);
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

                var performanceDto = MapPerformanceToDetailDto(performance);
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
                var performanceDtos = performances.Select(MapPerformanceToListDto);
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
                var performanceDtos = performances.Select(MapPerformanceToListDto);
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
                var performanceDtos = performances.Select(MapPerformanceToListDto);
                return Ok(performanceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        // Приватні методи для маппінгу
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

        private PerformanceDetailDto MapPerformanceToDetailDto(Performance performance)
        {
            var totalTickets = performance.Tickets?.Count() ?? 0;
            var soldTickets = performance.Tickets?.Count(t => t.IsSold) ?? 0;

            return new PerformanceDetailDto
            {
                Id = performance.Id,
                Title = performance.Title ?? "Без назви",
                Description = performance.Description ?? "",
                PerformanceDate = performance.PerformanceDate,
                Duration = performance.Duration,
                BasePrice = performance.BasePrice,
                Author = performance.Author != null ? MapAuthorToDto(performance.Author) : null,
                Genre = performance.Genre != null ? MapGenreToDto(performance.Genre) : null,
                Hall = performance.Hall != null ? MapHallToDto(performance.Hall) : null,
                TotalTickets = totalTickets,
                SoldTickets = soldTickets,
                AvailableTickets = totalTickets - soldTickets
            };
        }

        private AuthorDto MapAuthorToDto(Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                FullName = author.FullName ?? "Невідомий автор",
                Biography = author.Biography ?? "",
                BirthDate = author.BirthDate
            };
        }

        private GenreDto MapGenreToDto(Genre genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name ?? "Невідомий жанр",
                Description = genre.Description ?? ""
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
    }
}