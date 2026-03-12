using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Response
{
    public class UpdateShopDetailsResponse
    {
        public int SellerId { get; set; }

        public string ShopName { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Message { get; set; }
    }
}
