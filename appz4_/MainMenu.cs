using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TheatreTicketSystem.ConsoleUI
{
    public class MainMenu
    {
        private readonly IServiceProvider _serviceProvider;

        public MainMenu(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Show()
        {
            Console.WriteLine("Main menu started");

            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("===========================================");
                    Console.WriteLine("   СИСТЕМА УПРАВЛІННЯ ТЕАТРАЛЬНИМИ КВИТКАМИ");
                    Console.WriteLine("===========================================");
                    Console.WriteLine();
                    Console.WriteLine("1. Управління авторами");
                    Console.WriteLine("2. Управління жанрами");
                    Console.WriteLine("3. Управління залами");
                    Console.WriteLine("4. Управління виставами");
                    Console.WriteLine("5. Управління квитками");
                    Console.WriteLine("0. Вихід");
                    Console.WriteLine();
                    Console.Write("Оберіть опцію: ");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ShowAuthorMenu();
                            break;
                        case "2":
                            ShowGenreMenu();
                            break;
                        case "3":
                            ShowHallMenu();
                            break;
                        case "4":
                            ShowPerformanceMenu();
                            break;
                        case "5":
                            ShowTicketMenu();
                            break;
                        case "0":
                            Console.WriteLine("Дякуємо за використання системи!");
                            Console.WriteLine("User exited application");
                            return;
                        default:
                            Console.WriteLine("Невірний вибір. Натисніть будь-яку клавішу для продовження...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                    Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                    Console.ReadKey();
                }
            }
        }

        private void ShowAuthorMenu()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var authorMenu = scope.ServiceProvider.GetRequiredService<AuthorMenu>();
                authorMenu.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відкритті меню авторів: {ex.Message}");
                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        private void ShowGenreMenu()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var genreMenu = scope.ServiceProvider.GetRequiredService<GenreMenu>();
                genreMenu.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відкритті меню жанрів: {ex.Message}");
                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        private void ShowHallMenu()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var hallMenu = scope.ServiceProvider.GetRequiredService<HallMenu>();
                hallMenu.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відкритті меню залів: {ex.Message}");
                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        private void ShowPerformanceMenu()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var performanceMenu = scope.ServiceProvider.GetRequiredService<PerformanceMenu>();
                performanceMenu.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відкритті меню вистав: {ex.Message}");
                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }

        private void ShowTicketMenu()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var ticketMenu = scope.ServiceProvider.GetRequiredService<TicketMenu>();
                ticketMenu.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відкритті меню квитків: {ex.Message}");
                Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }
    }
}