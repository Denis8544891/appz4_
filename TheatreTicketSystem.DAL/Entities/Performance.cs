using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreTicketSystem.DAL.Entities
{
    public class Performance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public DateTime PerformanceDate { get; set; }

        public TimeSpan Duration { get; set; }

        [Range(0, 10000)]
        public decimal BasePrice { get; set; }

        // Зовнішні ключі
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public int HallId { get; set; }

        // Навігаційні властивості
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }

        [ForeignKey("HallId")]
        public virtual Hall Hall { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}