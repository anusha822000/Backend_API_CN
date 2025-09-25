namespace RoleManagementApi.DTOs
{
    public class AppRoleReadDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? RoleDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
