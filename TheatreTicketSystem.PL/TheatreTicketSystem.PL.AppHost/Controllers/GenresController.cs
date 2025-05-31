using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.PL.Models;

namespace TheatreTicketSystem.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly GenreService _genreService;
        private readonly IMapper _mapper;

        public GenresController(GenreService genreService, IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
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
                var genreDtos = _mapper.Map<IEnumerable<GenreListDto>>(genres);
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

                var genreDto = _mapper.Map<GenreDto>(genre);
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

                var genreDto = _mapper.Map<GenreDto>(genre);
                var performanceDtos = _mapper.Map<IEnumerable<PerformanceListDto>>(genre.Performances);

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
    }
}