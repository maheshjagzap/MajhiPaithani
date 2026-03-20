using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class UploadProductDto
    {
        public int iSellerId { get; set; }
        public int iCategoryId { get; set; }
        public string sProductTitle { get; set; } = string.Empty;
        public string sDescription { get; set; } = string.Empty;
        public decimal dcBasePrice { get; set; }
        public string sColor { get; set; } = string.Empty;
        public string sFabric { get; set; } = string.Empty;
        public string sDesignType { get; set; } = string.Empty;
        public bool bIsCustomizationAvailable { get; set; }
        public int iStock { get; set; }

        // Images
        public List<IFormFile>? ProductImages { get; set; }
    }
}
