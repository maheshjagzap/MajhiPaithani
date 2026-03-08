using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class User
{
    public int IUserId { get; set; }

    public string? SFirstName { get; set; }

    public string? SLastName { get; set; }

    public string? SEmail { get; set; }

    public string? SPhoneNumber { get; set; }

    public string? SPasswordHash { get; set; }

    public int IRoleId { get; set; }

    public bool? BIsActive { get; set; }

    public bool? BIsDeleted { get; set; }

    public DateTime? DCreatedDate { get; set; }

    public DateTime? DUpdatedDate { get; set; }

    public DateTime? DDeletedDate { get; set; }
}
