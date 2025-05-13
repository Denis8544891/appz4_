using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheatreTicketSystem.DAL.Entities
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PerformanceId { get; set; }

        [Required]
        public int SeatId { get; set; }

        [Required]
        public bool IsSold { get; set; } = false;

        [Required]
        public decimal Price { get; set; }

        public DateTime? PurchaseDate { get; set; }

        // Навігаційні властивості
        public Performance Performance { get; set; }
        public Seat Seat { get; set; }
    }
}
