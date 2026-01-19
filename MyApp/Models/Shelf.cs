using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyApp.Areas.Identity.Data;

namespace MyApp.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public MyAppUser? User { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
