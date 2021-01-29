using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Data
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTimeOffset CreatedUtc { get; set; }
        public DateTimeOffset? ModifiedUtc { get; set; }
        public virtual List<Note> Notes { get; set; } = new List<Note>();



    }
}
