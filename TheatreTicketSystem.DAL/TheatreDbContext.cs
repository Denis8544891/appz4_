using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL
{
    public class TheatreDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public TheatreDbContext(DbContextOptions<TheatreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Налаштування зв'язків між сутностями

            // Зв'язок Performance - Author (M:1)
            modelBuilder.Entity<Performance>()
                .HasOne(p => p.Author)
                .WithMany(a => a.Performances)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict); // Змінено з Cascade на Restrict

            // Зв'язок Performance - Genre (M:1)
            modelBuilder.Entity<Performance>()
                .HasOne(p => p.Genre)
                .WithMany(g => g.Performances)
                .HasForeignKey(p => p.GenreId)
                .OnDelete(DeleteBehavior.Restrict); // Змінено з Cascade на Restrict

            // Зв'язок Performance - Hall (M:1)
            modelBuilder.Entity<Performance>()
                .HasOne(p => p.Hall)
                .WithMany(h => h.Performances)
                .HasForeignKey(p => p.HallId)
                .OnDelete(DeleteBehavior.Restrict); // Змінено з Cascade на Restrict

            // Зв'язок Seat - Hall (M:1)
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Hall)
                .WithMany(h => h.Seats)
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Restrict); // Змінено з Cascade на Restrict

            // Зв'язок Ticket - Performance (M:1)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Performance)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PerformanceId)
                .OnDelete(DeleteBehavior.Restrict); // Змінено з Cascade на Restrict

            // Зв'язок Ticket - Seat (M:1)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Restrict); // Змінено з Cascade на Restrict
        }
    }
}