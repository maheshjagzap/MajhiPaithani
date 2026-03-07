using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class ChatMessage
{
    public int IMessageId { get; set; }

    public int? IChatRoomId { get; set; }

    public int? ISenderUserId { get; set; }

    public string? SMessage { get; set; }

    public string? SAttachmentUrl { get; set; }

    public DateTime? DSentDate { get; set; }
}
