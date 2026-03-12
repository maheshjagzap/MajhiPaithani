using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class UpdateBankDetailsRequest
    {
        
        public int BankDetailId { get; set; }

        public string AccountHolderName { get; set; }

        public string BankName { get; set; }

        public string AccountNumber { get; set; }

        public string IFSCCode { get; set; }

        public string UPIId { get; set; }
    }
}
