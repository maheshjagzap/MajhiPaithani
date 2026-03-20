using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class ProductDto
    {
        public int RequestedFor { get; set; }
        public int? Taskid { get; set; }
        public int? iProductId { get; set; }
        public int? iSellerId { get; set; }
        public int? iCategoryId { get; set; }
        public string sProductTitle { get; set; }
        public string sDescription { get; set; }
        public decimal? dcBasePrice { get; set; }
        public string? sColor { get; set; }
        public string? sFabric { get; set; }
        public string? sDesignType { get; set; }
        public bool? bIsCustomizationAvailable { get; set; }
        public int? iStock { get; set; }
    }
}
