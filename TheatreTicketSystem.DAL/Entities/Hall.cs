using System.ComponentModel.DataAnnotations;

namespace TheatreTicketSystem.DAL.Entities
{
    public class Hall
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(1, 1000)]
        public int Capacity { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Навігаційні властивості
        public virtual ICollection<Performance> Performances { get; set; } = new List<Performance>();
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}