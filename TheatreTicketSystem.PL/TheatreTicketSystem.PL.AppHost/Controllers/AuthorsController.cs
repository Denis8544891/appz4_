using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.PL.Models;

namespace TheatreTicketSystem.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorService _authorService;
        private readonly IMapper _mapper;

        public AuthorsController(AuthorService authorService, IMapper mapper)
        {
            _authorService = authorService;
            _mapper = mapper;
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
                var authorDtos = _mapper.Map<IEnumerable<AuthorListDto>>(authors);
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

                var authorDto = _mapper.Map<AuthorDto>(author);
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

                var authorDto = _mapper.Map<AuthorDto>(author);
                var performanceDtos = _mapper.Map<IEnumerable<PerformanceListDto>>(author.Performances);

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
    }
}