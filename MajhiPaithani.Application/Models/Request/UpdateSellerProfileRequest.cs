using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class UpdateSellerProfileRequest
    {
        public string sShopName { get; set; }

        public string sShopDescription { get; set; }

        public int iLocationId { get; set; }

        public string sPhoneNumber { get; set; }
    }
}
