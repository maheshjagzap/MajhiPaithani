using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class Notification
{
    public int INotificationId { get; set; }

    public int? IUserId { get; set; }

    public string? STitle { get; set; }

    public string? SMessage { get; set; }

    public string? SNotificationType { get; set; }

    public bool? BIsRead { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
