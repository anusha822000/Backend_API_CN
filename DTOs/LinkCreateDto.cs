namespace RoleManagementApi.Dtos
{
    public class LinkCreateDto
    {
        public string Url { get; set; } = null!;       // e.g., "https://example.com"
        public string Type { get; set; } = "External"; // Internal, External
    }
}