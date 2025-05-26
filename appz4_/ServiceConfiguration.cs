using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Repositories;
using TheatreTicketSystem.DAL.UoW;

namespace TheatreTicketSystem.ConsoleUI.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Connection string прямо в коді
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TheatreTicketSystem;Trusted_Connection=True;TrustServerCertificate=True;";

            // Реєстрація DbContext без логування
            services.AddDbContext<TheatreDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Реєстрація репозиторіїв
            RegisterRepositories(services);

            // Реєстрація Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Реєстрація сервісів бізнес-логіки
            RegisterBusinessServices(services);

            // Реєстрація UI компонентів
            RegisterUIServices(services);

            return services.BuildServiceProvider();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            // Базовий репозиторій
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Спеціалізовані репозиторії
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IHallRepository, HallRepository>();
            services.AddScoped<IPerformanceRepository, PerformanceRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
        }

        private static void RegisterBusinessServices(IServiceCollection services)
        {
            // Сервіси бізнес-логіки
            services.AddScoped<AuthorService>();
            services.AddScoped<GenreService>();
            services.AddScoped<HallService>();
            services.AddScoped<PerformanceService>();
            services.AddScoped<SeatService>();
            services.AddScoped<TicketService>();
        }

        private static void RegisterUIServices(IServiceCollection services)
        {
            // Реєстрація меню та UI компонентів
            services.AddTransient<MainMenu>();
            services.AddTransient<AuthorMenu>();
            services.AddTransient<GenreMenu>();
            services.AddTransient<HallMenu>();
            services.AddTransient<PerformanceMenu>();
            services.AddTransient<TicketMenu>();
        }

        public static void EnsureDatabaseCreated(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TheatreDbContext>();

            try
            {
                Console.WriteLine("Recreating database...");
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                Console.WriteLine("Database created successfully");

                SeedData(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the database: {ex.Message}");
                throw;
            }
        }

        private static void SeedData(TheatreDbContext context)
        {
            try
            {
                Console.WriteLine("Seeding initial data...");

                if (!context.Authors.Any())
                {
                    var authors = new[]
                    {
                        new TheatreTicketSystem.DAL.Entities.Author
                        {
                            FullName = "Леся Українка",
                            Biography = "Українська письменниця, поетеса, драматург",
                            BirthDate = new DateTime(1871, 2, 25)
                        },
                        new TheatreTicketSystem.DAL.Entities.Author
                        {
                            FullName = "Іван Франко",
                            Biography = "Український письменник, поет, публіцист",
                            BirthDate = new DateTime(1856, 8, 27)
                        },
                        new TheatreTicketSystem.DAL.Entities.Author
                        {
                            FullName = "Микола Куліш",
                            Biography = "Український драматург і театральний діяч",
                            BirthDate = new DateTime(1892, 12, 19)
                        }
                    };
                    context.Authors.AddRange(authors);
                }

                if (!context.Genres.Any())
                {
                    var genres = new[]
                    {
                        new TheatreTicketSystem.DAL.Entities.Genre { Name = "Драма", Description = "Серйозні п'єси з глибоким змістом" },
                        new TheatreTicketSystem.DAL.Entities.Genre { Name = "Комедія", Description = "Веселі та розважальні вистави" },
                        new TheatreTicketSystem.DAL.Entities.Genre { Name = "Трагедія", Description = "Серйозні п'єси з трагічним закінченням" },
                        new TheatreTicketSystem.DAL.Entities.Genre { Name = "Музичний", Description = "Вистави з музичним супроводом" },
                        new TheatreTicketSystem.DAL.Entities.Genre { Name = "Дитячий", Description = "Вистави для дітей та сімейного перегляду" }
                    };
                    context.Genres.AddRange(genres);
                }

                if (!context.Halls.Any())
                {
                    var halls = new[]
                    {
                        new TheatreTicketSystem.DAL.Entities.Hall { Name = "Великий зал", Capacity = 500, Description = "Основний зал театру" },
                        new TheatreTicketSystem.DAL.Entities.Hall { Name = "Малий зал", Capacity = 150, Description = "Камерний зал для невеликих вистав" },
                        new TheatreTicketSystem.DAL.Entities.Hall { Name = "Експериментальний зал", Capacity = 80, Description = "Зал для експериментальних постановок" }
                    };
                    context.Halls.AddRange(halls);
                }

                context.SaveChanges();
                Console.WriteLine("Initial data seeded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding data: {ex.Message}");
            }
        }
    }
}