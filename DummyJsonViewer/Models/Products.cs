using System.ComponentModel.DataAnnotations;

namespace DummyJsonViewer.Models
{
    public class Products
    {
        [Required]
        public Product[]? products { get; set; }

        public int? skipped {  get; set; }
        
    }
}
