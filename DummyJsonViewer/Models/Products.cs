using System.ComponentModel.DataAnnotations;

namespace DummyJsonViewer.Models
{
    public class Products
    {
        [Required]
        public Product[]? products { get; set; }

        
        
    }
}
