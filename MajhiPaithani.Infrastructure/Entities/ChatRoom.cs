using System;
using System.Collections.Generic;

namespace MajhiPaithani.Infrastructure.Entities;

public partial class ChatRoom
{
    public int IChatRoomId { get; set; }

    public int? ICustomerId { get; set; }

    public int? ISellerId { get; set; }

    public DateTime? DCreatedDate { get; set; }
}
