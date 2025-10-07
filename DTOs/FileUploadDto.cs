namespace RoleManagementApi.Dtos
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; } = null!;  // The uploaded file
        public string Type { get; set; } = "File";    // Always "File"
    }
}