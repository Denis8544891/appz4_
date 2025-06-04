using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL
{
    public class TheatreDbContext : DbContext
    {
        public TheatreDbContext(DbContextOptions<TheatreDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Performance> Performances { get; set; } = null!;
        public DbSet<Hall> Halls { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Виправляємо помилки з decimal полями
            modelBuilder.Entity<Performance>()
                .Property(p => p.BasePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");

            // Налаштування зв'язків
            modelBuilder.Entity<Performance>()
                .HasOne(p => p.Author)
                .WithMany(a => a.Performances)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Performance>()
                .HasOne(p => p.Genre)
                .WithMany(g => g.Performances)
                .HasForeignKey(p => p.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Performance>()
                .HasOne(p => p.Hall)
                .WithMany(h => h.Performances)
                .HasForeignKey(p => p.HallId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Hall)
                .WithMany(h => h.Seats)
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Performance)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PerformanceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // Початкові дані
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Автори
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FullName = "Іван Франко", Biography = "Український письменник, поет, публіцист", BirthDate = new DateTime(1856, 8, 27) },
                new Author { Id = 2, FullName = "Леся Українка", Biography = "Українська поетеса, драматург", BirthDate = new DateTime(1871, 2, 25) },
                new Author { Id = 3, FullName = "Микола Гоголь", Biography = "Український та російський письменник", BirthDate = new DateTime(1809, 3, 20) }
            );

            // Жанри
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Драма", Description = "Серйозний жанр з глибокими темами" },
                new Genre { Id = 2, Name = "Комедія", Description = "Легкий розважальний жанр" },
                new Genre { Id = 3, Name = "Трагедія", Description = "Жанр з трагічним завершенням" }
            );

            // Зали
            modelBuilder.Entity<Hall>().HasData(
                new Hall { Id = 1, Name = "Великий зал", Capacity = 500, Description = "Основний зал театру" },
                new Hall { Id = 2, Name = "Малий зал", Capacity = 150, Description = "Камерний зал для невеликих вистав" }
            );

            // Місця для першого залу (перші 10 місць)
            for (int i = 1; i <= 10; i++)
            {
                modelBuilder.Entity<Seat>().HasData(
                    new Seat { Id = i, Row = (i - 1) / 5 + 1, Number = ((i - 1) % 5) + 1, HallId = 1, IsVIP = false }
                );
            }

            // Вистави
            modelBuilder.Entity<Performance>().HasData(
                new Performance
                {
                    Id = 1,
                    Title = "Кайдашева сім'я",
                    Description = "Комедія за повістю Івана Нечуя-Левицького",
                    PerformanceDate = DateTime.Now.AddDays(30),
                    Duration = TimeSpan.FromHours(2),
                    BasePrice = 250.00m,
                    AuthorId = 1,
                    GenreId = 2,
                    HallId = 1
                },
                new Performance
                {
                    Id = 2,
                    Title = "Лісова пісня",
                    Description = "Драма-феєрія Лесі Українки",
                    PerformanceDate = DateTime.Now.AddDays(45),
                    Duration = TimeSpan.FromHours(2.5),
                    BasePrice = 300.00m,
                    AuthorId = 2,
                    GenreId = 1,
                    HallId = 1
                }
            );
        }
    }
}