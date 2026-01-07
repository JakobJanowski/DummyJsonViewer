using System.ComponentModel.DataAnnotations;

namespace DummyJsonViewer.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string? title { get; set; }

        [Required]
        public string? description { get; set; }

        [Required]
        public string? category { get; set; }

        [Required]
        public double? price { get; set; }

        [Required]
        public double? discountPercentage { get; set; }

        [Required]
        public double? rating { get; set; }

        [Required]
        public int? stock { get; set; }

        [Required]
        public string[]? tags { get; set; }

        [Required]
        public string? brand { get; set; }

        [Required]
        public string? sku { get; set; }

        [Required]
        public int? weight { get; set; }


        [Required]
        public Dimensions? dimensions { get; set; }


        [Required]

        public string? warrantyInformation { get; set; }
        [Required]
        public string? shippingInformation { get; set; }

        [Required]
        public string? availabilityStatus { get; set; }

        [Required]
        public Reviews[]? reviews { get; set; }

        [Required]
        public string? returnPolicy { get; set; }

        [Required]
        public int? minimumOrderQuantity { get; set; }

        [Required]
        public Meta? meta { get; set; }
        [Required]
        public string? thumbnail { get; set; }
        [Required]
        public string[]? images { get; set; }






    }
}

