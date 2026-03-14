using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class AddProductRequest
    {
        public int SellerId { get; set; }

        public int CategoryId { get; set; }

        public string ProductTitle { get; set; }

        public string Description { get; set; }

        public decimal BasePrice { get; set; }

        public string Color { get; set; }

        public string Fabric { get; set; }

        public string DesignType { get; set; }

        public bool IsCustomizationAvailable { get; set; }
    }
}
