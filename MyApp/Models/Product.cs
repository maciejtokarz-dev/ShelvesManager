using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        [Required]
        public int ShelfId { get; set; }
        [ForeignKey("ShelfId")]
        public Shelf? Shelf { get; set; }
    }
}
