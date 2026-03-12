using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Response
{
    public class AddDesignResponse
    {
        public int DesignId { get; set; }

        public int SellerId { get; set; }

        public string DesignName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Message { get; set; }
    }
}
