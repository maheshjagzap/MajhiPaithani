using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class ChatMessage
{
    public int IMessageId { get; set; }

    public int? IChatRoomId { get; set; }

    public int? ISenderUserId { get; set; }

    public int? IReceiverUserId { get; set; }

    public string? SMessage { get; set; }

    public string? SAttachmentUrl { get; set; }

    public bool BIsDelivered { get; set; } = false;

    public bool BIsRead { get; set; } = false;

    public DateTime? DDeliveredDate { get; set; }

    public DateTime? DReadDate { get; set; }

    public DateTime? DSentDate { get; set; }
}
