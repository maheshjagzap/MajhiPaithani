using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Infrastructure.Entities
{
    public partial class Design
    {
        public int IDesignId { get; set; }

        public int ISellerId { get; set; }

        public string? SDesignName { get; set; }

        public string? SDesignType { get; set; }

        public string? SDescription { get; set; }

        public string? SImageUrl { get; set; }

        public DateTime? DCreatedDate { get; set; }

        public DateTime? DUpdatedDate { get; set; }

        public bool? BIsDeleted { get; set; }

        // Navigation Property

        public virtual Seller? Seller { get; set; }
    }
}
