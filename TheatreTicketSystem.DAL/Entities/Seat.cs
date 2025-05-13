using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TheatreTicketSystem.DAL.Entities
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HallId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Row { get; set; }

        [Required]
        [Range(1, 100)]
        public int Number { get; set; }

        // Навігаційні властивості
        public Hall Hall { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
