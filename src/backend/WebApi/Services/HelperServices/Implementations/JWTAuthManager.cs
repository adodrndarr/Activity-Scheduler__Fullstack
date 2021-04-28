using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Services.HelperServices.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ActivityScheduler.Services.HelperServices
{
    public class JWTAuthManager : IJWTAuthManager
    {
        public JWTAuthManager(IConfiguration configuration, UserManager<User> userManager)
        {
            this._configuration = configuration;
            this._jwtSettings = _configuration.GetSection("JWTSettings");
            this._userManager = userManager;
        }


        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly UserManager<User> _userManager;

        public SigningCredentials GetSigningCredentials()
        {
            var secret = _jwtSettings.GetSection("securityKey").Value;
            var secretBytes = Encoding.UTF8.GetBytes(secret);

            var securityKey = new SymmetricSecurityKey(secretBytes);
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user)
        {
            var claims = await _userManager.GetClaimsAsync(user) as List<Claim>;

            claims.AddRange(new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            });

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            string minutes = _jwtSettings.GetSection("expiryInMinutes").Value;
            double expiryInMinutes = Convert.ToDouble(minutes);

            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("validIssuer").Value,
                audience: _jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryInMinutes),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
    }
}
