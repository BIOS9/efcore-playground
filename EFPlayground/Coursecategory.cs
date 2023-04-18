using System;
using System.Collections.Generic;

namespace EFPlayground;

public partial class Coursecategory
{
    public ulong DiscordSnowflake { get; set; }

    public string? AutoImportPattern { get; set; }

    public int AutoImportPriority { get; set; }
}
