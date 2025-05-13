
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreTicketSystem.DAL.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(1000)]
        public string Biography { get; set; }

        public DateTime? BirthDate { get; set; }

        // Навігаційна властивість для зв'язку з виставами
        public virtual ICollection<Performance> Performances { get; set; } = new List<Performance>();
    }
}