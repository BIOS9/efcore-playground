using System;
using System.Collections.Generic;

namespace EFPlayground;

public partial class User
{
    public ulong DiscordSnowflake { get; set; }

    public byte[]? EncryptedUsername { get; set; }

    public sbyte DisallowCourseJoin { get; set; }

    public virtual ICollection<Course> CourseNames { get; set; } = new List<Course>();
}
