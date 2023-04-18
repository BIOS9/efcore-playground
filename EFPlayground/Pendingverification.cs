using System;
using System.Collections.Generic;

namespace EFPlayground;

public partial class Pendingverification
{
    public string Token { get; set; } = null!;

    public byte[] EncryptedUsername { get; set; } = null!;

    public ulong DiscordSnowflake { get; set; }

    public long CreationTime { get; set; }
}
