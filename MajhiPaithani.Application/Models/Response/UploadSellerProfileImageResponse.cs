using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Response
{
    public class UploadSellerProfileImageResponse
    {
        public int SellerId { get; set; }

        public string ProfileImageUrl { get; set; }

        public DateTime UploadedDate { get; set; }

        public string Message { get; set; }
    }
}
