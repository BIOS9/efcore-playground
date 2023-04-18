using System;
using System.Collections.Generic;

namespace EFPlayground;

public partial class Course
{
    public string Name { get; set; } = null!;

    public ulong DiscordChannelSnowflake { get; set; }

    public virtual ICollection<User> UserDiscordSnowflakes { get; set; } = new List<User>();
}
