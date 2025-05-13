using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreTicketSystem.DAL.Entities
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 100)]
        public int Row { get; set; }

        [Range(1, 100)]
        public int Number { get; set; }

        public bool IsVIP { get; set; }

        // Зовнішній ключ
        public int HallId { get; set; }

        // Навігаційні властивості
        [ForeignKey("HallId")]
        public virtual Hall Hall { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}