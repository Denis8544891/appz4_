using TheatreTicketSystem.DAL.UoW;
using TheatreTicketSystem.DAL.Entities;
using TheatreTicketSystem.DAL;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TheatreDbContext>();

        // Переконайтеся, що база створена
        context.Database.EnsureCreated();

        // Якщо дані вже є, не додавайте
        if (context.Authors.Any()) return;

        // Додаємо тестових авторів
        var authors = new[]
        {
            new Author { FullName = "Іван Франко", Biography = "Український письменник", BirthDate = new DateTime(1856, 8, 27) },
            new Author { FullName = "Леся Українка", Biography = "Українська поетеса", BirthDate = new DateTime(1871, 2, 25) },
            new Author { FullName = "Тарас Шевченко", Biography = "Великий український поет", BirthDate = new DateTime(1814, 3, 9) }
        };
        context.Authors.AddRange(authors);

        // Додаємо жанри
        var genres = new[]
        {
            new Genre { Name = "Драма", Description = "Драматичні вистави" },
            new Genre { Name = "Комедія", Description = "Комедійні вистави" },
            new Genre { Name = "Трагедія", Description = "Трагічні вистави" }
        };
        context.Genres.AddRange(genres);

        // Додаємо зали
        var halls = new[]
        {
            new Hall { Name = "Великий зал", Capacity = 500, Description = "Основний зал театру" },
            new Hall { Name = "Малий зал", Capacity = 150, Description = "Камерний зал" }
        };
        context.Halls.AddRange(halls);

        context.SaveChanges();

        // Додаємо вистави
        var performances = new[]
        {
            new Performance
            {
                Title = "Кайдашева сім'я",
                Description = "П'єса за твором Івана Нечуя-Левицького",
                PerformanceDate = DateTime.Now.AddDays(7),
                Duration = TimeSpan.FromHours(2),
                BasePrice = 250,
                AuthorId = authors[0].Id,
                GenreId = genres[0].Id,
                HallId = halls[0].Id
            },
            new Performance
            {
                Title = "Лісова пісня",
                Description = "Драма-феєрія Лесі Українки",
                PerformanceDate = DateTime.Now.AddDays(14),
                Duration = TimeSpan.FromHours(2.5),
                BasePrice = 300,
                AuthorId = authors[1].Id,
                GenreId = genres[0].Id,
                HallId = halls[1].Id
            }
        };
        context.Performances.AddRange(performances);
        context.SaveChanges();
    }
}