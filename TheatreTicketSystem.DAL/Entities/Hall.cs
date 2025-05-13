using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TheatreTicketSystem.DAL.Entities
{
    public class Hall
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Range(1, 1000)]
        public int Capacity { get; set; }

        // Навігаційні властивості
        public ICollection<Performance> Performances { get; set; } = new List<Performance>();
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
