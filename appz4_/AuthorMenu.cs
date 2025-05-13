using Microsoft.Extensions.DependencyInjection;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.ConsoleUI
{
    public class AuthorMenu
    {
        private readonly AuthorService _authorService;

        public AuthorMenu(IServiceProvider serviceProvider)
        {
            _authorService = serviceProvider.GetRequiredService<AuthorService>();
        }

        public void Show()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("======= УПРАВЛІННЯ АВТОРАМИ =======");
                Console.WriteLine("1. Переглянути всіх авторів");
                Console.WriteLine("2. Додати нового автора");
                Console.WriteLine("3. Редагувати автора");
                Console.WriteLine("4. Видалити автора");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть опцію: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllAuthors();
                        break;
                    case "2":
                        AddAuthor();
                        break;
                    case "3":
                        UpdateAuthor();
                        break;
                    case "4":
                        DeleteAuthor();
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

        private void ShowAllAuthors()
        {
            Console.Clear();
            Console.WriteLine("======= СПИСОК АВТОРІВ =======");

            var authors = _authorService.GetAllAuthors();
            if (!authors.Any())
            {
                Console.WriteLine("Список авторів порожній.");
            }
            else
            {
                foreach (var author in authors)
                {
                    Console.WriteLine($"ID: {author.Id}, Ім'я: {author.FullName}, Дата народження: {author.BirthDate?.ToShortDateString() ?? "Невідомо"}");
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void AddAuthor()
        {
            Console.Clear();
            Console.WriteLine("======= ДОДАВАННЯ НОВОГО АВТОРА =======");

            Console.Write("Введіть повне ім'я автора: ");
            var fullName = Console.ReadLine();

            Console.Write("Введіть біографію (необов'язково): ");
            var biography = Console.ReadLine();

            DateTime? birthDate = null;
            Console.Write("Введіть дату народження (формат: ДД.ММ.РРРР, необов'язково): ");
            var birthDateStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(birthDateStr))
            {
                if (DateTime.TryParse(birthDateStr, out var date))
                {
                    birthDate = date;
                }
                else
                {
                    Console.WriteLine("Неправильний формат дати. Дата народження буде встановлена як невідома.");
                }
            }

            var author = new Author
            {
                FullName = fullName,
                Biography = biography,
                BirthDate = birthDate
            };

            _authorService.AddAuthor(author);

            Console.WriteLine("Автора успішно додано!");
            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void UpdateAuthor()
        {
            Console.Clear();
            Console.WriteLine("======= РЕДАГУВАННЯ АВТОРА =======");

            Console.Write("Введіть ID автора для редагування: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var author = _authorService.GetAuthorById(id);
            if (author == null)
            {
                Console.WriteLine("Автора з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Редагування автора: {author.FullName}");

            Console.Write($"Введіть нове повне ім'я автора (поточне: {author.FullName}): ");
            var fullName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                author.FullName = fullName;
            }

            Console.Write($"Введіть нову біографію (поточна: {author.Biography}): ");
            var biography = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(biography))
            {
                author.Biography = biography;
            }

            Console.Write($"Введіть нову дату народження (поточна: {author.BirthDate?.ToShortDateString() ?? "Невідомо"}, формат: ДД.ММ.РРРР): ");
            var birthDateStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(birthDateStr))
            {
                if (DateTime.TryParse(birthDateStr, out var date))
                {
                    author.BirthDate = date;
                }
                else
                {
                    Console.WriteLine("Неправильний формат дати. Дата народження залишиться без змін.");
                }
            }

            _authorService.UpdateAuthor(author);

            Console.WriteLine("Автора успішно оновлено!");
            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void DeleteAuthor()
        {
            Console.Clear();
            Console.WriteLine("======= ВИДАЛЕННЯ АВТОРА =======");

            Console.Write("Введіть ID автора для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var author = _authorService.GetAuthorById(id);
            if (author == null)
            {
                Console.WriteLine("Автора з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Ви дійсно хочете видалити автора {author.FullName}? (так/ні): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "так")
            {
                _authorService.DeleteAuthor(id);
                Console.WriteLine("Автора успішно видалено!");
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