using WorkManagementApp.Models;
using System.Threading.Tasks;

namespace WorkManagementApp.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
