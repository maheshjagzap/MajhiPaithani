using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class SellerDto
    {
        public int RequestedFor { get; set; }
        public int? Taskid { get; set; }
        public int iSellerId { get; set; }
        public int iUserId { get; set; }
        public string? sShopName { get; set; }
        public string? sShopDescription { get; set; }
        public int iLocationId { get; set; }
        public bool bIsVerified { get; set; }
        public bool bIsActive { get; set; }
        public bool bIsDeleted { get; set; }
    }
}
