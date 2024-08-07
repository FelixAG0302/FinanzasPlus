﻿using System;
using System.Collections.Generic;

namespace FinanzasPlus.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? IdCard { get; set; }

    public string? PersonType { get; set; }

    public DateOnly Cutoff { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
