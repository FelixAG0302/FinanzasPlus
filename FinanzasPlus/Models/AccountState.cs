using System;
using System.Collections.Generic;

namespace FinanzasPlus.Models;

public partial class AccountState
{
    public int Id { get; set; }

    public int CardId { get; set; }

    public DateOnly InitialDate { get; set; }

    public decimal InitialBalance { get; set; }

    public DateOnly CutoffDate { get; set; }

    public decimal CutoffBalance { get; set; }

    public virtual Card Card { get; set; } = null!;
}
