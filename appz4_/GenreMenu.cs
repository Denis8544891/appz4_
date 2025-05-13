using Microsoft.Extensions.DependencyInjection;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.ConsoleUI
{
    public class GenreMenu
    {
        private readonly GenreService _genreService;

        public GenreMenu(IServiceProvider serviceProvider)
        {
            _genreService = serviceProvider.GetRequiredService<GenreService>();
        }

        public void Show()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("======= УПРАВЛІННЯ ЖАНРАМИ =======");
                Console.WriteLine("1. Переглянути всі жанри");
                Console.WriteLine("2. Додати новий жанр");
                Console.WriteLine("3. Редагувати жанр");
                Console.WriteLine("4. Видалити жанр");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть опцію: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllGenres();
                        break;
                    case "2":
                        AddGenre();
                        break;
                    case "3":
                        UpdateGenre();
                        break;
                    case "4":
                        DeleteGenre();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Натисніть будь-яку клавішу, щоб продовжити...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowAllGenres()
        {
            Console.Clear();
            Console.WriteLine("======= СПИСОК ЖАНРІВ =======");

            var genres = _genreService.GetAllGenres();
            if (!genres.Any())
            {
                Console.WriteLine("Список жанрів порожній.");
            }
            else
            {
                foreach (var genre in genres)
                {
                    Console.WriteLine($"ID: {genre.Id}, Назва: {genre.Name}");
                    if (!string.IsNullOrEmpty(genre.Description))
                    {
                        Console.WriteLine($"Опис: {genre.Description}");
                    }
                    Console.WriteLine(new string('-', 30));
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void AddGenre()
        {
            Console.Clear();
            Console.WriteLine("======= ДОДАВАННЯ НОВОГО ЖАНРУ =======");

            Console.Write("Введіть назву жанру: ");
            var name = Console.ReadLine();

            Console.Write("Введіть опис жанру (необов'язково): ");
            var description = Console.ReadLine();

            var genre = new Genre
            {
                Name = name,
                Description = description
            };

            _genreService.AddGenre(genre);

            Console.WriteLine("Жанр успішно додано!");
            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void UpdateGenre()
        {
            Console.Clear();
            Console.WriteLine("======= РЕДАГУВАННЯ ЖАНРУ =======");

            Console.Write("Введіть ID жанру для редагування: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var genre = _genreService.GetGenreById(id);
            if (genre == null)
            {
                Console.WriteLine("Жанр з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Редагування жанру: {genre.Name}");

            Console.Write($"Введіть нову назву жанру (поточна: {genre.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                genre.Name = name;
            }

            Console.Write($"Введіть новий опис жанру (поточний: {genre.Description}): ");
            var description = Console.ReadLine();
            if (description != null) // дозволяємо встановити пустий опис
            {
                genre.Description = description;
            }

            _genreService.UpdateGenre(genre);

            Console.WriteLine("Жанр успішно оновлено!");
            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void DeleteGenre()
        {
            Console.Clear();
            Console.WriteLine("======= ВИДАЛЕННЯ ЖАНРУ =======");

            Console.Write("Введіть ID жанру для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var genre = _genreService.GetGenreById(id);
            if (genre == null)
            {
                Console.WriteLine("Жанр з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Ви дійсно хочете видалити жанр {genre.Name}? (так/ні): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "так")
            {
                _genreService.DeleteGenre(id);
                Console.WriteLine("Жанр успішно видалено!");
            }
            else
            {
                Console.WriteLine("Видалення скасовано.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }
    }
}