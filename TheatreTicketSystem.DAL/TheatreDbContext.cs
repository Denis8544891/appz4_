using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.DAL
{
    public class TheatreDbContext : DbContext
    {
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }

        public TheatreDbContext(DbContextOptions<TheatreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфігурація зв'язків між сутностями

            // Зв'язок Performance - Hall (багато до одного)
            modelBuilder.Entity<Performance>()
                .HasOne(p => p.Hall)
                .WithMany(h => h.Performances)
                .HasForeignKey(p => p.HallId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок Performance - Genre (багато до одного)
            modelBuilder.Entity<Performance>()
                .HasOne(p => p.Genre)
                .WithMany(g => g.Performances)
                .HasForeignKey(p => p.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок Performance - Author (багато до одного)
            modelBuilder.Entity<Performance>()
                .HasOne(p => p.Author)
                .WithMany(a => a.Performances)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок Ticket - Performance (багато до одного)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Performance)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PerformanceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Зв'язок Ticket - Seat (багато до одного)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок Seat - Hall (багато до одного)
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Hall)
                .WithMany(h => h.Seats)
                .HasForeignKey(s => s.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}