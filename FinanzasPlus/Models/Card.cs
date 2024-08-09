using System;
using System.Collections.Generic;

namespace FinanzasPlus.Models;

public partial class Card
{
    public int Id { get; set; }

    public string CardNumber { get; set; } = null!;

    public int CutoffDay { get; set; }

    public int UserId { get; set; }

    public string Cvv { get; set; } = null!;

    public decimal? Amount { get; set; }

    public virtual ICollection<AccountState> AccountStates { get; set; } = new List<AccountState>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
