using System.ComponentModel.DataAnnotations;

namespace DummyJsonViewer.Models
{
    public class Meta
    {
        [Required]
        public string? createdAt { get; set; }
        [Required]
        public string? updatedAt { get; set; }
        [Required]
        public string? barcode { get; set; }
        [Required]
        public string? qrCode { get; set; }
    }
}
