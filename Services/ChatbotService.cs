namespace RoleManagementApi.Services
{
    public class ChatbotService : IChatbotService
    {
        public Task<string> GetResponseAsync(string message)
        {
            // Dummy responses for now
            if (message.Contains("broken"))
                return Task.FromResult("This link appears broken. Please check the URL or try HTTPS.");
            else if (message.Contains("show broken"))
                return Task.FromResult("Here is a list of broken links from the database (dummy).");
            return Task.FromResult("Sorry, I can only give link suggestions for now.");
        }
    }
}