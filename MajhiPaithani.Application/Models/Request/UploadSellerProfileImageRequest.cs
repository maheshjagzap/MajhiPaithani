using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{

    public class UploadSellerProfileImageRequest
    {
        public int SellerId { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
