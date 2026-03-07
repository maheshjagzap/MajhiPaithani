using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class AddSellerBankDetailsRequest
    {
        public int iSellerId { get; set; }

        public string sAccountHolderName { get; set; }

        public string sAccountNumber { get; set; }

        public string sIFSCCode { get; set; }

        public string sBankName { get; set; }
    }
}
