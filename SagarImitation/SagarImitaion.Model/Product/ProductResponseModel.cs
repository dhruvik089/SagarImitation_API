using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagarImitaion.Model.Product
{
    public class ProductResponseModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ProductImageName { get; set; }
        public IFormFile? ProductImage { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }              
    }
}
