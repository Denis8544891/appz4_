using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace TheatreTicketSystem.DAL.Entities
{
    public class Performance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int HallId { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public int AuthorId { get; set; }

        // Навігаційні властивості
        public Hall Hall { get; set; }
        public Genre Genre { get; set; }
        public Author Author { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}