using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.PL.Models;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorsController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Отримати всіх авторів
        /// </summary>
        /// <returns>Список всіх авторів</returns>
        [HttpGet]
        public ActionResult<IEnumerable<AuthorListDto>> GetAllAuthors()
        {
            try
            {
                var authors = _authorService.GetAllAuthors();
                var authorDtos = authors.Select(MapAuthorToListDto);
                return Ok(authorDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати автора за ID
        /// </summary>
        /// <param name="id">ID автора</param>
        /// <returns>Детальна інформація про автора</returns>
        [HttpGet("{id}")]
        public ActionResult<AuthorDto> GetAuthorById(int id)
        {
            try
            {
                var author = _authorService.GetAuthorById(id);
                if (author == null)
                {
                    return NotFound(new { message = $"Автор з ID {id} не знайдений" });
                }

                var authorDto = MapAuthorToDto(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка сервера", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати автора з виставами
        /// </summary>
        /// <param name="id">ID автора</param>
        /// <returns>Автор з переліком його вистав</returns>
        [HttpGet("{id}/with-performances")]
        public ActionResult<object> GetAuthorWithPerformances(int id)
        {
            try
            {
                var author = _authorService.GetAuthorWithPerformances(id);
                if (author == null)
                {
                    return NotFound(new { message = $"Автор з ID {id} не знайдений" });
                }

                var authorDto = MapAuthorToDto(author);
                var performanceDtos = author.Performances?.Select(MapPerformanceToListDto) ?? new List<PerformanceListDto>();

                var result = new
                {
                    Author = authorDto,
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
        private AuthorListDto MapAuthorToListDto(Author author)
        {
            return new AuthorListDto
            {
                Id = author.Id,
                FullName = author.FullName ?? "Невідомий автор",
                BirthDate = author.BirthDate,
                PerformancesCount = author.Performances?.Count() ?? 0
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