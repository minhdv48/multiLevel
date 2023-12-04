using System;
using System.Collections.Generic;

namespace Multi_LevelModels.Models;

public partial class TreeLevel
{
    public long Id { get; set; }

    public int? ProfileId { get; set; }

    public int? Levels { get; set; }

    public decimal? Value { get; set; }

    public int? ParentId { get; set; }
    public decimal? Benefit { get; set; }
}
