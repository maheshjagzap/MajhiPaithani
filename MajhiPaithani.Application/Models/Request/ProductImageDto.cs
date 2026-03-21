using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class ProductImageDto
    {
        public int iProductId { get; set; }
        public int iSellerId { get; set; }
        public int iCategoryId { get; set; }
        public string sProductTitle { get; set; }
        public string sDescription { get; set; }
        public decimal dcBasePrice { get; set; }
        public string sColor { get; set; }
        public string sFabric { get; set; }
        public string sDesignType { get; set; }
        public bool bIsCustomizationAvailable { get; set; }
        public bool bIsActive { get; set; }
        public bool bIsDeleted { get; set; }
        public DateTime ProductCreatedDate { get; set; }
        public DateTime? ProductUpdatedDate { get; set; }


        public int? iImageId { get; set; }
        public string sImageUrl { get; set; }
        public bool? bIsPrimary { get; set; }
        public DateTime? ImageCreatedDate { get; set; }
    }
}
