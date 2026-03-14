using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class UpdateProductRequest
    {
        public string ProductTitle { get; set; }

        public string Description { get; set; }

        public decimal BasePrice { get; set; }

        public string Color { get; set; }

        public string Fabric { get; set; }

        public string DesignType { get; set; }

        public bool IsCustomizationAvailable { get; set; }
    }
}
