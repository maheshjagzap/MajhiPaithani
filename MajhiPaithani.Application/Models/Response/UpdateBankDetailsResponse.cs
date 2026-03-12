using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Response.UpdateBankDetailsResponse
{
    public class UpdateBankDetailsResponse
    {
        public int BankDetailId { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Message { get; set; }
    }
}
