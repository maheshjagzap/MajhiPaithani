using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Models.Request
{
    public class RegisterRequest
    {
        public string sFirstName { get; set; }
        public string sLastName { get; set; }
        public string sEmail { get; set; }
        public string sPhoneNumber { get; set; }
        public string sPassword { get; set; }

        public int RoleId { get; set; }

    }
}
