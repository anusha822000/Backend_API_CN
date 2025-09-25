using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleManagementApi.Data;
using RoleManagementApi.Models;
using RoleManagementApi.DTOs;

namespace RoleManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppRolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/approles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppRoleReadDto>>> GetRoles()
        {
            return await _context.AppRoles
                .Select(r => new AppRoleReadDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    RoleDescription = r.RoleDescription,
                    IsActive = r.IsActive
                })
                .ToListAsync();
        }

        // GET: api/approles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AppRoleReadDto>> GetRole(int id)
        {
            var role = await _context.AppRoles.FindAsync(id);
            if (role == null) return NotFound();

            return new AppRoleReadDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
                IsActive = role.IsActive
            };
        }

        // POST: api/approles
        [HttpPost]
        public async Task<ActionResult<AppRoleReadDto>> CreateRole(AppRoleCreateDto dto)
        {
            if (await _context.AppRoles.AnyAsync(r => r.RoleName == dto.RoleName))
                return BadRequest(new { message = "Role name must be unique" });

            var role = new AppRole
            {
                RoleName = dto.RoleName,
                RoleDescription = dto.RoleDescription,
                IsActive = dto.IsActive
            };

            _context.AppRoles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRole), new { id = role.RoleId }, new AppRoleReadDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
                IsActive = role.IsActive
            });
        }

        // PUT: api/approles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, AppRoleUpdateDto dto)
        {
            var role = await _context.AppRoles.FindAsync(id);
            if (role == null) return NotFound();

            if (await _context.AppRoles.AnyAsync(r => r.RoleName == dto.RoleName && r.RoleId != id))
                return BadRequest(new { message = "Role name must be unique" });

            role.RoleName = dto.RoleName;
            role.RoleDescription = dto.RoleDescription;
            role.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/approles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.AppRoles.FindAsync(id);
            if (role == null) return NotFound();

            _context.AppRoles.Remove(role);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
