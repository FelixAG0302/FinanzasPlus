using System;
using System.Collections.Generic;

namespace FinanzasPlus.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TransactionType { get; set; }

    public int PaymentType { get; set; }

    public DateOnly? CreationDate { get; set; }

    public int CardId { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }

    public virtual Card Card { get; set; } = null!;

    public virtual PaymentType PaymentTypeNavigation { get; set; } = null!;

    public virtual TransactionType TransactionTypeNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
