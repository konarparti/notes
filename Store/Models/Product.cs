using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Product
    {
        public Guid ProductID { get; set; }
        [Required(ErrorMessage ="Please enter a product name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please enter a description")]
        public string? Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please specify a category")]
        public string? Category { get; set; }
    }
}
