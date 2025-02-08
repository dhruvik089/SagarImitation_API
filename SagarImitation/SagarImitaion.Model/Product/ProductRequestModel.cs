using Microsoft.AspNetCore.Http;

namespace SagarImitaion.Model.Product
{
    public class ProductRequestModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ProductImageName { get; set; }
        public IFormFile? ProductImage { get; set; }        
        public long ProductCategoryId { get; set; }
    }
}
