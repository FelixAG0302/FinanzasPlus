﻿using System;
using System.Collections.Generic;

namespace FinanzasPlus.Models;

public partial class TransactionType
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
