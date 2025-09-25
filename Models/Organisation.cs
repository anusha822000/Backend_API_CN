using System;
using System.Collections.Generic;

namespace RoleManagementApi.Models;

public partial class Organisation
{
    public int OrganisationId { get; set; }

    public string Name { get; set; } = null!;

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Status { get; set; }

    public string? Logo { get; set; }

    public virtual ICollection<Facility> Facilities { get; set; } = new List<Facility>();
}
