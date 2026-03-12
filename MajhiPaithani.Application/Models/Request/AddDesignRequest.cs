using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    using Microsoft.AspNetCore.Http;

    public class AddDesignRequest
    {
        public int SellerId { get; set; }

        public string DesignName { get; set; }

        public string DesignType { get; set; }

        public string Description { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
