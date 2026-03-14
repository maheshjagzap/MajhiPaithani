using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Response
{
    public class GetSellerDesignsResponse
    {
        public int DesignId { get; set; }

        public int SellerId { get; set; }

        public string DesignName { get; set; }

        public string DesignType { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
