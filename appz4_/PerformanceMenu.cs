using Microsoft.Extensions.DependencyInjection;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.ConsoleUI
{
    public class PerformanceMenu
    {
        private readonly PerformanceService _performanceService;
        private readonly AuthorService _authorService;
        private readonly GenreService _genreService;
        private readonly HallService _hallService;
        private readonly TicketService _ticketService;

        public PerformanceMenu(IServiceProvider serviceProvider)
        {
            _performanceService = serviceProvider.GetRequiredService<PerformanceService>();
            _authorService = serviceProvider.GetRequiredService<AuthorService>();
            _genreService = serviceProvider.GetRequiredService<GenreService>();
            _hallService = serviceProvider.GetRequiredService<HallService>();
            _ticketService = serviceProvider.GetRequiredService<TicketService>();
        }

        public void Show()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("======= УПРАВЛІННЯ ВИСТАВАМИ =======");
                Console.WriteLine("1. Переглянути всі вистави");
                Console.WriteLine("2. Переглянути майбутні вистави");
                Console.WriteLine("3. Переглянути вистави за жанром");
                Console.WriteLine("4. Переглянути вистави за автором");
                Console.WriteLine("5. Додати нову виставу");
                Console.WriteLine("6. Редагувати виставу");
                Console.WriteLine("7. Видалити виставу");
                Console.WriteLine("8. Створити квитки для вистави");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть опцію: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllPerformances();
                        break;
                    case "2":
                        ShowUpcomingPerformances();
                        break;
                    case "3":
                        ShowPerformancesByGenre();
                        break;
                    case "4":
                        ShowPerformancesByAuthor();
                        break;
                    case "5":
                        AddPerformance();
                        break;
                    case "6":
                        UpdatePerformance();
                        break;
                    case "7":
                        DeletePerformance();
                        break;
                    case "8":
                        CreateTicketsForPerformance();
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

        private void ShowAllPerformances()
        {
            Console.Clear();
            Console.WriteLine("======= СПИСОК ВИСТАВ =======");

            var performances = _performanceService.GetAllPerformancesWithDetails();

            if (!performances.Any())
            {
                Console.WriteLine("Список вистав порожній.");
            }
            else
            {
                DisplayPerformances(performances);
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void ShowUpcomingPerformances()
        {
            Console.Clear();
            Console.WriteLine("======= МАЙБУТНІ ВИСТАВИ =======");

            var performances = _performanceService.GetUpcomingPerformances();

            if (!performances.Any())
            {
                Console.WriteLine("Список майбутніх вистав порожній.");
            }
            else
            {
                DisplayPerformances(performances);
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void ShowPerformancesByGenre()
        {
            Console.Clear();
            Console.WriteLine("======= ВИСТАВИ ЗА ЖАНРОМ =======");

            var genres = _genreService.GetAllGenres();
            if (!genres.Any())
            {
                Console.WriteLine("Список жанрів порожній. Спочатку додайте жанр.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Оберіть жанр:");
            foreach (var genre in genres)
            {
                Console.WriteLine($"{genre.Id}. {genre.Name}");
            }

            Console.Write("Введіть ID жанру: ");
            if (!int.TryParse(Console.ReadLine(), out int genreId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var selectedGenre = genres.FirstOrDefault(g => g.Id == genreId);
            if (selectedGenre == null)
            {
                Console.WriteLine("Жанр з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"======= ВИСТАВИ ЖАНРУ: {selectedGenre.Name} =======");

            var performances = _performanceService.GetPerformancesByGenre(genreId);

            if (!performances.Any())
            {
                Console.WriteLine("Вистав для цього жанру не знайдено.");
            }
            else
            {
                DisplayPerformances(performances);
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void ShowPerformancesByAuthor()
        {
            Console.Clear();
            Console.WriteLine("======= ВИСТАВИ ЗА АВТОРОМ =======");

            var authors = _authorService.GetAllAuthors();
            if (!authors.Any())
            {
                Console.WriteLine("Список авторів порожній. Спочатку додайте автора.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Оберіть автора:");
            foreach (var author in authors)
            {
                Console.WriteLine($"{author.Id}. {author.FullName}");
            }

            Console.Write("Введіть ID автора: ");
            if (!int.TryParse(Console.ReadLine(), out int authorId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var selectedAuthor = authors.FirstOrDefault(a => a.Id == authorId);
            if (selectedAuthor == null)
            {
                Console.WriteLine("Автора з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"======= ВИСТАВИ АВТОРА: {selectedAuthor.FullName} =======");

            var performances = _performanceService.GetPerformancesByAuthor(authorId);

            if (!performances.Any())
            {
                Console.WriteLine("Вистав цього автора не знайдено.");
            }
            else
            {
                DisplayPerformances(performances);
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void AddPerformance()
        {
            Console.Clear();
            Console.WriteLine("======= ДОДАВАННЯ НОВОЇ ВИСТАВИ =======");

            // Перевіряємо наявність авторів
            var authors = _authorService.GetAllAuthors();
            if (!authors.Any())
            {
                Console.WriteLine("Список авторів порожній. Спочатку додайте автора.");
                Console.ReadKey();
                return;
            }

            // Перевіряємо наявність жанрів
            var genres = _genreService.GetAllGenres();
            if (!genres.Any())
            {
                Console.WriteLine("Список жанрів порожній. Спочатку додайте жанр.");
                Console.ReadKey();
                return;
            }

            // Перевіряємо наявність залів
            var halls = _hallService.GetAllHalls();
            if (!halls.Any())
            {
                Console.WriteLine("Список залів порожній. Спочатку додайте зал.");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть назву вистави: ");
            var title = Console.ReadLine();

            Console.Write("Введіть опис вистави (необов'язково): ");
            var description = Console.ReadLine();

            Console.Write("Введіть дату вистави (формат: ДД.ММ.РРРР): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime performanceDate))
            {
                Console.WriteLine("Неправильний формат дати.");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть тривалість вистави в хвилинах: ");
            if (!int.TryParse(Console.ReadLine(), out int durationMinutes) || durationMinutes <= 0)
            {
                Console.WriteLine("Неправильний формат тривалості.");
                Console.ReadKey();
                return;
            }
            var duration = TimeSpan.FromMinutes(durationMinutes);

            Console.Write("Введіть базову ціну квитка: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal basePrice) || basePrice < 0)
            {
                Console.WriteLine("Неправильний формат ціни.");
                Console.ReadKey();
                return;
            }

            // Вибір автора
            Console.WriteLine("\nОберіть автора:");
            foreach (var author in authors)
            {
                Console.WriteLine($"{author.Id}. {author.FullName}");
            }

            Console.Write("Введіть ID автора: ");
            if (!int.TryParse(Console.ReadLine(), out int authorId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            if (!authors.Any(a => a.Id == authorId))
            {
                Console.WriteLine("Автора з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            // Вибір жанру
            Console.WriteLine("\nОберіть жанр:");
            foreach (var genre in genres)
            {
                Console.WriteLine($"{genre.Id}. {genre.Name}");
            }

            Console.Write("Введіть ID жанру: ");
            if (!int.TryParse(Console.ReadLine(), out int genreId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            if (!genres.Any(g => g.Id == genreId))
            {
                Console.WriteLine("Жанр з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            // Вибір залу
            Console.WriteLine("\nОберіть зал:");
            foreach (var hall in halls)
            {
                Console.WriteLine($"{hall.Id}. {hall.Name} (Місткість: {hall.Capacity})");
            }

            Console.Write("Введіть ID залу: ");
            if (!int.TryParse(Console.ReadLine(), out int hallId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            if (!halls.Any(h => h.Id == hallId))
            {
                Console.WriteLine("Зал з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            var performance = new Performance
            {
                Title = title,
                Description = description,
                PerformanceDate = performanceDate,
                Duration = duration,
                BasePrice = basePrice,
                AuthorId = authorId,
                GenreId = genreId,
                HallId = hallId
            };

            _performanceService.AddPerformance(performance);

            Console.WriteLine("Виставу успішно додано!");

            Console.Write("Бажаєте створити квитки для цієї вистави зараз? (так/ні): ");
            var createTickets = Console.ReadLine()?.ToLower() == "так";

            if (createTickets)
            {
                // Отримуємо ID доданої вистави
                var addedPerformance = _performanceService.GetAllPerformancesWithDetails()
                    .FirstOrDefault(p => p.Title == title &&
                                     p.AuthorId == authorId &&
                                     p.GenreId == genreId &&
                                     p.HallId == hallId);

                if (addedPerformance != null)
                {
                    _ticketService.CreateTicketsForPerformance(addedPerformance.Id);
                    Console.WriteLine("Квитки для вистави успішно створено!");
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void UpdatePerformance()
        {
            Console.Clear();
            Console.WriteLine("======= РЕДАГУВАННЯ ВИСТАВИ =======");

            Console.Write("Введіть ID вистави для редагування: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var performance = _performanceService.GetPerformanceById(id);
            if (performance == null)
            {
                Console.WriteLine("Виставу з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Редагування вистави: {performance.Title}");

            Console.Write($"Введіть нову назву вистави (поточна: {performance.Title}): ");
            var title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                performance.Title = title;
            }

            Console.Write($"Введіть новий опис вистави (поточний: {performance.Description}): ");
            var description = Console.ReadLine();
            if (description != null) // дозволяємо встановити пустий опис
            {
                performance.Description = description;
            }

            Console.Write($"Введіть нову дату вистави (поточна: {performance.PerformanceDate.ToShortDateString()}, формат: ДД.ММ.РРРР): ");
            var dateStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dateStr) && DateTime.TryParse(dateStr, out DateTime performanceDate))
            {
                performance.PerformanceDate = performanceDate;
            }

            Console.Write($"Введіть нову тривалість вистави в хвилинах (поточна: {performance.Duration.TotalMinutes}): ");
            var durationStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(durationStr) && int.TryParse(durationStr, out int durationMinutes) && durationMinutes > 0)
            {
                performance.Duration = TimeSpan.FromMinutes(durationMinutes);
            }

            Console.Write($"Введіть нову базову ціну квитка (поточна: {performance.BasePrice}): ");
            var priceStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(priceStr) && decimal.TryParse(priceStr, out decimal basePrice) && basePrice >= 0)
            {
                performance.BasePrice = basePrice;
            }

            // Опціонально змінюємо автора
            Console.Write("Бажаєте змінити автора? (так/ні): ");
            if (Console.ReadLine()?.ToLower() == "так")
            {
                var authors = _authorService.GetAllAuthors();

                Console.WriteLine("\nОберіть нового автора:");
                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.Id}. {author.FullName}");
                }

                Console.Write("Введіть ID автора: ");
                if (int.TryParse(Console.ReadLine(), out int authorId) && authors.Any(a => a.Id == authorId))
                {
                    performance.AuthorId = authorId;
                }
                else
                {
                    Console.WriteLine("Невірний ID автора. Залишаємо поточного автора.");
                }
            }

            // Опціонально змінюємо жанр
            Console.Write("Бажаєте змінити жанр? (так/ні): ");
            if (Console.ReadLine()?.ToLower() == "так")
            {
                var genres = _genreService.GetAllGenres();

                Console.WriteLine("\nОберіть новий жанр:");
                foreach (var genre in genres)
                {
                    Console.WriteLine($"{genre.Id}. {genre.Name}");
                }

                Console.Write("Введіть ID жанру: ");
                if (int.TryParse(Console.ReadLine(), out int genreId) && genres.Any(g => g.Id == genreId))
                {
                    performance.GenreId = genreId;
                }
                else
                {
                    Console.WriteLine("Невірний ID жанру. Залишаємо поточний жанр.");
                }
            }

            // Опціонально змінюємо зал
            Console.Write("Бажаєте змінити зал? (так/ні): ");
            if (Console.ReadLine()?.ToLower() == "так")
            {
                var halls = _hallService.GetAllHalls();

                Console.WriteLine("\nОберіть новий зал:");
                foreach (var hall in halls)
                {
                    Console.WriteLine($"{hall.Id}. {hall.Name} (Місткість: {hall.Capacity})");
                }

                Console.Write("Введіть ID залу: ");
                if (int.TryParse(Console.ReadLine(), out int hallId) && halls.Any(h => h.Id == hallId))
                {
                    performance.HallId = hallId;
                }
                else
                {
                    Console.WriteLine("Невірний ID залу. Залишаємо поточний зал.");
                }
            }

            _performanceService.UpdatePerformance(performance);

            Console.WriteLine("Виставу успішно оновлено!");
            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void DeletePerformance()
        {
            Console.Clear();
            Console.WriteLine("======= ВИДАЛЕННЯ ВИСТАВИ =======");

            Console.Write("Введіть ID вистави для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var performance = _performanceService.GetPerformanceById(id);
            if (performance == null)
            {
                Console.WriteLine("Виставу з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Ви дійсно хочете видалити виставу '{performance.Title}'? (так/ні): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "так")
            {
                _performanceService.DeletePerformance(id);
                Console.WriteLine("Виставу успішно видалено!");
            }
            else
            {
                Console.WriteLine("Видалення скасовано.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void CreateTicketsForPerformance()
        {
            Console.Clear();
            Console.WriteLine("======= СТВОРЕННЯ КВИТКІВ ДЛЯ ВИСТАВИ =======");

            var performances = _performanceService.GetUpcomingPerformances();
            if (!performances.Any())
            {
                Console.WriteLine("Немає майбутніх вистав, для яких можна створити квитки.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Оберіть виставу для створення квитків:");
            foreach (var performance in performances)
            {
                Console.WriteLine($"{performance.Id}. {performance.Title} ({performance.PerformanceDate.ToShortDateString()})");
            }

            Console.Write("Введіть ID вистави: ");
            if (!int.TryParse(Console.ReadLine(), out int performanceId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var selectedPerformance = performances.FirstOrDefault(p => p.Id == performanceId);
            if (selectedPerformance == null)
            {
                Console.WriteLine("Виставу з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            try
            {
                _ticketService.CreateTicketsForPerformance(performanceId);
                Console.WriteLine("Квитки для вистави успішно створено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при створенні квитків: {ex.Message}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void DisplayPerformances(IEnumerable<Performance> performances)
        {
            foreach (var performance in performances)
            {
                Console.WriteLine(new string('=', 50));
                Console.WriteLine($"ID: {performance.Id}");
                Console.WriteLine($"Назва: {performance.Title}");

                if (!string.IsNullOrEmpty(performance.Description))
                {
                    Console.WriteLine($"Опис: {performance.Description}");
                }

                Console.WriteLine($"Дата: {performance.PerformanceDate.ToShortDateString()}");
                Console.WriteLine($"Час: {performance.PerformanceDate.ToShortTimeString()}");
                Console.WriteLine($"Тривалість: {performance.Duration.Hours}год {performance.Duration.Minutes}хв");
                Console.WriteLine($"Базова ціна: {performance.BasePrice} грн");

                if (performance.Author != null)
                {
                    Console.WriteLine($"Автор: {performance.Author.FullName}");
                }

                if (performance.Genre != null)
                {
                    Console.WriteLine($"Жанр: {performance.Genre.Name}");
                }

                if (performance.Hall != null)
                {
                    Console.WriteLine($"Зал: {performance.Hall.Name}");
                }
            }
        }
    }
}