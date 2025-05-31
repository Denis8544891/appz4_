using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.PL.Models;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenresController(GenreService genreService)
        {
            _genreService = genreService;
        }

        /// <summary>
        /// Отримати всі жанри
        /// </summary>
        /// <returns>Список всіх жанрів</returns>
        [HttpGet]
        public ActionResult<IEnumerable<GenreListDto>> GetAllGenres()
        {
            try
            {
                var genres = _genreService.GetAllGenres();
                var genreDtos = genres.Select(MapGenreToListDto);
                return Ok(genreDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати жанр за ID
        /// </summary>
        /// <param name="id">ID жанру</param>
        /// <returns>Детальна інформація про жанр</returns>
        [HttpGet("{id}")]
        public ActionResult<GenreDto> GetGenreById(int id)
        {
            try
            {
                var genre = _genreService.GetGenreById(id);
                if (genre == null)
                {
                    return NotFound(new { message = $"Жанр з ID {id} не знайдений" });
                }

                var genreDto = MapGenreToDto(genre);
                return Ok(genreDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати жанр з виставами
        /// </summary>
        /// <param name="id">ID жанру</param>
        /// <returns>Жанр з переліком вистав цього жанру</returns>
        [HttpGet("{id}/with-performances")]
        public ActionResult<object> GetGenreWithPerformances(int id)
        {
            try
            {
                var genre = _genreService.GetGenreWithPerformances(id);
                if (genre == null)
                {
                    return NotFound(new { message = $"Жанр з ID {id} не знайдений" });
                }

                var genreDto = MapGenreToDto(genre);
                var performanceDtos = genre.Performances?.Select(MapPerformanceToListDto) ?? new List<PerformanceListDto>();

                var result = new
                {
                    Genre = genreDto,
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
        private GenreListDto MapGenreToListDto(Genre genre)
        {
            return new GenreListDto
            {
                Id = genre.Id,
                Name = genre.Name ?? "Невідомий жанр",
                Description = genre.Description ?? "",
                PerformancesCount = genre.Performances?.Count() ?? 0
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