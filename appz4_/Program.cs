using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheatreTicketSystem.ConsoleUI.Configuration;

namespace TheatreTicketSystem.ConsoleUI
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            try
            {
                // Налаштування консолі
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.Title = "Theatre Ticket Management System";

                // Налаштування DI контейнера
                _serviceProvider = ServiceConfiguration.ConfigureServices();

                Console.WriteLine("Application starting...");

                // Ініціалізація бази даних
                ServiceConfiguration.EnsureDatabaseCreated(_serviceProvider);

                // Запуск головного меню
                RunApplication();

                Console.WriteLine("Application finished successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            finally
            {
                DisposeServices();
            }
        }

        private static void RunApplication()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mainMenu = scope.ServiceProvider.GetRequiredService<MainMenu>();

                Console.WriteLine("Starting main menu...");
                mainMenu.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while running application: {ex.Message}");
                throw;
            }
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
                return;

            try
            {
                Console.WriteLine("Disposing services...");

                if (_serviceProvider is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during cleanup: {ex.Message}");
            }
        }
    }
}