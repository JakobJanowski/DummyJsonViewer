using System.ComponentModel.DataAnnotations;

namespace DummyJsonViewer.Models
{
    public class Reviews
    {
        [Required]
        public int? rating { get; set; }
        [Required]
        public string? comment { get; set; }
        [Required]
        public string? date { get; set; }
        [Required]
        public string? reviewerName { get; set; }
    
        [Required]
        public string? reviewerEmail { get; set; }
    
}
}
