using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class BankDto
    {
        public int RequestedFor { get; set; }
        public int? Taskid { get; set; }
        public int IBankDetailId { get; set; }
        public int? ISellerId { get; set; }
        public string? SAccountHolderName { get; set; }
        public string? SAccountNumber { get; set; }
        public string? SIFSCCode { get; set; }
        public string? SBankName { get; set; }
    }

}
