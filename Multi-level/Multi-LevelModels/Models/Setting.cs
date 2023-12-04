using System;
using System.Collections.Generic;

namespace Multi_LevelModels.Models;

public partial class Setting
{
    public int Id { get; set; }

    public decimal? RootValue { get; set; }

    public decimal? Profit { get; set; }
}
