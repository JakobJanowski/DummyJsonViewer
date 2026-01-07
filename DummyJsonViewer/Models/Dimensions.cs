using System.ComponentModel.DataAnnotations;

namespace DummyJsonViewer.Models
{
    public class Dimensions
    {
        [Required]
        public double? width { get; set; }
        [Required]
        public double? height { get; set; }
        [Required]
        public double? depth { get; set; }
        
    }
}
