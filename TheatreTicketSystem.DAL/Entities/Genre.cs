using System.ComponentModel.DataAnnotations;

namespace TheatreTicketSystem.DAL.Entities
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Навігаційна властивість для зв'язку з виставами
        public virtual ICollection<Performance> Performances { get; set; } = new List<Performance>();
    }
}