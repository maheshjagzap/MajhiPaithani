using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Infrastructure.Entities
{

    public class UserAddress
    {
        [Key]
        public int AddressId { get; set; }  // Primary Key

        public int UserId { get; set; }     // Just a normal column (no FK attribute)

        public string? AddressLine1 { get; set; }
                     
        public string? AddressLine2 { get; set; }
                     
        public string? City { get; set; }
                     
        public string? State { get; set; }
                     
        public string? PostalCode { get; set; }
                     
        public string? Country { get; set; }
                     
        public string? AddressType { get; set; }

        public bool? IsDefault { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
