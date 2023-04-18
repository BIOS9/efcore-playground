using System;
using System.Collections.Generic;

namespace EFPlayground;

public partial class Servermessage
{
    public ulong MessageId { get; set; }

    public ulong ChannelId { get; set; }

    public string Content { get; set; } = null!;

    public long Created { get; set; }

    public ulong Creator { get; set; }

    public long LastEdited { get; set; }

    public ulong Editor { get; set; }

    public string Name { get; set; } = null!;
}
