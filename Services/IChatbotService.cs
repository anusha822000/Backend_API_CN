namespace RoleManagementApi.Services
{
    public interface IChatbotService
    {
        Task<string> GetResponseAsync(string message);
    }
}
 