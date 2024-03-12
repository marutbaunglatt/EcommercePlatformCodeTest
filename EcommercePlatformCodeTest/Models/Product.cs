using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePlatformCodeTest.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Precision(16, 2)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        //[NotMapped]
        [Required]
        public string ImageData { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }
    }
}
