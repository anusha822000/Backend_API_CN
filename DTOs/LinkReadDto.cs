namespace RoleManagementApi.Dtos
{
    public class LinkReadDto
    {
        public int LinkId { get; set; }
        public string? Url { get; set; }
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime? LastCheckedAt { get; set; }
 
        // File-specific properties
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}