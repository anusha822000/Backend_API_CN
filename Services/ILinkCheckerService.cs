using System.Threading.Tasks;
 
namespace RoleManagementApi.Services;
 
public interface ILinkCheckerService
{
    Task<string> CheckLinkStatusAsync(string url, string type);
}
 