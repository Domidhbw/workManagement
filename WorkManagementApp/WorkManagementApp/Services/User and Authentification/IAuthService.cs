using Microsoft.IdentityModel.Tokens;
using WorkManagementApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace WorkManagementApp.Services.Authentification
{
    public interface IAuthService
    {
        Task<string> GenerateJwtToken(User user);
    }
}
