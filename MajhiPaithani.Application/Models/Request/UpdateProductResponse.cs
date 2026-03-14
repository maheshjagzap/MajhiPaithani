using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class UpdateProductResponse
    {
        public int ProductId { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Message { get; set; }
    }
}
