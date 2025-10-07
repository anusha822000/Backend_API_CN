using System;
using System.Collections.Generic;

namespace RoleManagementApi.Models;

public partial class Link
{
    public int LinkId { get; set; }

    public string? Url { get; set; }

    public string Type { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? LastCheckedAt { get; set; }

    public string? FilePath { get; set; }

    public string? FileName { get; set; }

    public string? ContentType { get; set; }

    public virtual ICollection<LinkCheck> LinkChecks { get; set; } = new List<LinkCheck>();
}
