using System;
using System.Collections.Generic;

namespace EFPlayground;

public partial class Verificationhistory
{
    public int Id { get; set; }

    public ulong DiscordSnowflake { get; set; }

    public byte[] EncryptedUsername { get; set; } = null!;

    public long VerificationTime { get; set; }
}
