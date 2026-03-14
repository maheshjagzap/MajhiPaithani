using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Response
{
    public class DeleteDesignResponse
    {
        public int DesignId { get; set; }

        public DateTime DeletedDate { get; set; }

        public string Message { get; set; }
    }
}
