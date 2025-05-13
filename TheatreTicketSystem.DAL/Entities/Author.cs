using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TheatreTicketSystem.DAL.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Навігаційні властивості
        public ICollection<Performance> Performances { get; set; } = new List<Performance>();
    }
}
}
