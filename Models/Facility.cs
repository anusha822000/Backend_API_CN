using System;
using System.Collections.Generic;

namespace RoleManagementApi.Models;

public partial class Facility
{
    public int FacilityId { get; set; }

    public int? OrganisationId { get; set; }

    public int? ParentFacilityId { get; set; }

    public string? UnitNumber { get; set; }

    public string? FacilityName { get; set; }

    public string? FacilityExtra { get; set; }

    public string? Status { get; set; }

    public string? Image { get; set; }

    public bool IsSingleEntity { get; set; }

    public int? FacilityUnitNumber { get; set; }

    public virtual ICollection<Facility> InverseParentFacility { get; set; } = new List<Facility>();

    public virtual Organisation? Organisation { get; set; }

    public virtual Facility? ParentFacility { get; set; }
}
