﻿using System;
using System.Collections.Generic;

namespace FinanzasPlus.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
