using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ActivityScheduler.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> RemoveAdminRoleAsync(User user, string role);
        Task<IdentityResult> AddUserRoleAsync(User user, string role);

        Task<IdentityResult> CreateAsync(UserToRegisterDTO userToRegister, bool admin);
        Task<ResultDetails> Register(UserToRegisterDTO userToRegister);
        Task<ResultDetails> RegisterAdmin(UserToRegisterDTO userToRegister);
        Task<ResultDetails> Login(UserToLoginDTO userToLogin, HttpResponse response);

        Task<LoginResponseDTO> CreateLoginResponse(User user, IList<string> roles);
        void AppendCookies(string token, string userName, HttpResponse response);
        Task<User> FindByEmailAsync(string email);
        string GetCurrentUserId(HttpContext context);

        Task<IList<string>> GetRolesAsync(User user);
        string HashPassword(User user, string password);
        Task<SignInResult> PasswordSignInAsync(string userName, string password);

        Task SignOutAsync();
        Task<ResultDetails> Logout();
    }
}
