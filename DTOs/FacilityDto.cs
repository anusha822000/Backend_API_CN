using System.Collections.Generic;

namespace RoleManagementApi.DTO
{
    public class FacilityDto
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

        // For hierarchy
        public List<FacilityDto> Children { get; set; } = new();
    }
}
