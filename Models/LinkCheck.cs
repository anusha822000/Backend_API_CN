using System;
using System.Collections.Generic;

namespace RoleManagementApi.Models;

public partial class LinkCheck
{
    public int LinkCheckId { get; set; }

    public int LinkId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CheckedAt { get; set; }

    public string? Message { get; set; }

    public virtual Link Link { get; set; } = null!;
}
