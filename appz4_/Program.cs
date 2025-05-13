using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Repositories;

namespace TheatreTicketSystem.ConsoleUI
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            RegisterServices();

            var mainMenu = new MainMenu(_serviceProvider);
            mainMenu.Show();

            DisposeServices();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();

            // Реєстрація DbContext
            services.AddDbContext<TheatreDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TheatreTicketSystem;Trusted_Connection=True;"));

            // Реєстрація репозиторіїв
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IHallRepository, HallRepository>();
            services.AddScoped<IPerformanceRepository, PerformanceRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            // Реєстрація сервісів
            services.AddScoped<AuthorService>();
            services.AddScoped<GenreService>();
            services.AddScoped<HallService>();
            services.AddScoped<PerformanceService>();
            services.AddScoped<SeatService>();
            services.AddScoped<TicketService>();

            _serviceProvider = services.BuildServiceProvider();

            // Створення бази даних при першому запуску
            var dbContext = _serviceProvider.GetRequiredService<TheatreDbContext>();

            // Видаляємо базу даних, якщо вона існує, і створюємо знову
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }

            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}