using Microsoft.Extensions.DependencyInjection;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL.Entities;

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
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("======= СИСТЕМА УПРАВЛІННЯ ТЕАТРАЛЬНИМИ КВИТКАМИ =======");
                Console.WriteLine("1. Управління авторами");
                Console.WriteLine("2. Управління жанрами");
                Console.WriteLine("3. Управління залами");
                Console.WriteLine("4. Управління виставами");
                Console.WriteLine("5. Управління квитками");
                Console.WriteLine("0. Вихід");
                Console.Write("Оберіть опцію: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        new AuthorMenu(_serviceProvider).Show();
                        break;
                    case "2":
                        new GenreMenu(_serviceProvider).Show();
                        break;
                    case "3":
                        new HallMenu(_serviceProvider).Show();
                        break;
                    case "4":
                        new PerformanceMenu(_serviceProvider).Show();
                        break;
                    case "5":
                        new TicketMenu(_serviceProvider).Show();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Натисніть будь-яку клавішу, щоб продовжити...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}