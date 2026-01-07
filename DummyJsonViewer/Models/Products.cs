using System.ComponentModel.DataAnnotations;

namespace DummyJsonViewer.Models
{
    //When doing a call to https://dummyjson.com/products it will give the products in a array also called products
    //As in "products": [...] instead of [...]
    //Therefore for this call we need this to store the resulting json
    public class Products
    {
        [Required]
        public Product[]? products { get; set; }

        
        
    }
}
