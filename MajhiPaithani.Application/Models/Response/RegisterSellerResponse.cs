using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Response
{
    public class RegisterSellerResponse
    {
        public int iSellerId { get; set; }

        public int iUserId { get; set; }

        public string sShopName { get; set; }

        public bool bIsVerified { get; set; }

        public bool bIsActive { get; set; }

        public string sMessage { get; set; }
    }
}
