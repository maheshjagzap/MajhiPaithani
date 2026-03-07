using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class RegisterSellerRequest
    {
        public int? UserId { get; set; }

        public string ShopName { get; set; }

        public string ShopAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pincode { get; set; }

        public string Description { get; set; }

        public string AccountHolderName { get; set; }

        public string AccountNumber { get; set; }

        public string IFSCCode { get; set; }

        public string AadharNumber { get; set; }

        public string PanNumber { get; set; }
    }
}
