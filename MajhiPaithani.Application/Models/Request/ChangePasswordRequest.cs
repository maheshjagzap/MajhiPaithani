using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class ChangePasswordRequest
    {
        public int UserId { get; set; }
        public string oldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
