using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Response
{
    public class AddProductResponse
    {
        public int ProductId { get; set; }

        public int SellerId { get; set; }

        public string ProductName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Message { get; set; }
    }
}
