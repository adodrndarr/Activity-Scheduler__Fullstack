using ActivityScheduler.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ActivityScheduler.Services.HelperServices.Interfaces
{
    public interface IJWTAuthManager
    {
        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
        Task<IList<Claim>> GetClaimsAsync(User user);
        SigningCredentials GetSigningCredentials();
    }
}
