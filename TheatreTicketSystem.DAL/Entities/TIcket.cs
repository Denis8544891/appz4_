using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreTicketSystem.DAL.Entities
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public DateTime? PurchaseDate { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }

        public bool IsSold { get; set; }

        // Зовнішні ключі
        public int PerformanceId { get; set; }
        public int SeatId { get; set; }

        // Навігаційні властивості
        [ForeignKey("PerformanceId")]
        public virtual Performance Performance { get; set; }

        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; }
    }
}