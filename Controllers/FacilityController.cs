using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleManagementApi.Data;
using RoleManagementApi.DTO;
using RoleManagementApi.Models;

namespace RoleManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FacilityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Facility
        [HttpGet]
        public async Task<IActionResult> GetAllFacilities()
        {
            var facilities = await _context.Facilities
                .Include(f => f.Organisation)
                .Include(f => f.ParentFacility)
                .ToListAsync();

            var dtos = facilities.Select(f => new FacilityDto
            {
                FacilityId = f.FacilityId,
                FacilityName = f.FacilityName,
                IsSingleEntity = f.IsSingleEntity,
                ParentFacilityId = f.ParentFacilityId,
                OrganisationId = f.OrganisationId
            }).ToList();

            return Ok(dtos);
        }

        // GET: api/Facility/hierarchy
        [HttpGet("hierarchy")]
        public async Task<IActionResult> GetFacilityHierarchy()
        {
            var facilities = await _context.Facilities.ToListAsync();

            var dtoDict = facilities.ToDictionary(
                f => f.FacilityId,
                f => new FacilityDto
                {
                    FacilityId = f.FacilityId,
                    FacilityName = f.FacilityName,
                    IsSingleEntity = f.IsSingleEntity,
                    ParentFacilityId = f.ParentFacilityId,
                    OrganisationId = f.OrganisationId
                });

            var rootFacilities = new List<FacilityDto>();

            foreach (var f in facilities)
            {
                var dto = dtoDict[f.FacilityId];
                if (f.ParentFacilityId == null)
                {
                    rootFacilities.Add(dto);
                }
                else if (dtoDict.TryGetValue(f.ParentFacilityId.Value, out var parent))
                {
                    parent.Children.Add(dto);
                }
            }

            return Ok(rootFacilities);
        }

        // GET: api/Facility/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacilityById(int id)
        {
            var facility = await _context.Facilities
                .Include(f => f.Organisation)
                .Include(f => f.ParentFacility)
                .FirstOrDefaultAsync(f => f.FacilityId == id);

            if (facility == null)
                return NotFound();

            var dto = new FacilityDto
            {
                FacilityId = facility.FacilityId,
                FacilityName = facility.FacilityName,
                IsSingleEntity = facility.IsSingleEntity,
                ParentFacilityId = facility.ParentFacilityId,
                OrganisationId = facility.OrganisationId
            };

            return Ok(dto);
        }

        // GET: api/Facility/byOrganisation/5
        [HttpGet("byOrganisation/{organisationId}")]
        public async Task<IActionResult> GetFacilitiesByOrganisation(int organisationId)
        {
            var facilities = await _context.Facilities
                .Where(f => f.OrganisationId == organisationId)
                .ToListAsync();

            var dtos = facilities.Select(f => new FacilityDto
            {
                FacilityId = f.FacilityId,
                FacilityName = f.FacilityName,
                IsSingleEntity = f.IsSingleEntity,
                ParentFacilityId = f.ParentFacilityId,
                OrganisationId = f.OrganisationId
            }).ToList();

            return Ok(dtos);
        }
    }
}
