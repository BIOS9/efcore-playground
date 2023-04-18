using System;
using System.Collections.Generic;

namespace EFPlayground;

public partial class Coursealias
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Target { get; set; }

    public bool? Hidden { get; set; }
}
