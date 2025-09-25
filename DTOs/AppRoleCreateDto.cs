using System.ComponentModel.DataAnnotations;

namespace RoleManagementApi.DTOs
{
    public class AppRoleCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? RoleDescription { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
