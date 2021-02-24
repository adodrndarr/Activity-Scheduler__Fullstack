using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Domain.Structs;
using ActivityScheduler.Presentation.EntitiesDTO;
using ActivityScheduler.Services.HelperServices.Interfaces;
using ActivityScheduler.Services.Interfaces;
using AutoMapper;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ActivityScheduler.Services
{
    public class AccountService : IAccountService
    {
        public AccountService(
            IRepositoryContainer repositoryContainer,
            IJWTAuthManager JWTManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            ILoggerManager logger)
        {
            _repos = repositoryContainer;
            _JWTManager = JWTManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }


        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTAuthManager _JWTManager;
        private readonly IMapper _mapper;
        private IRepositoryContainer _repos;
        private ILoggerManager _logger;

        public string HashPassword(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var pwdHash = hasher.HashPassword(user, password);

            return pwdHash;
        }

        public Task<IdentityResult> CreateAsync(UserToRegisterDTO userToRegister, bool admin = false)
        {
            var user = _mapper.Map<User>(userToRegister);
            user.IsAdmin = admin;
            var userCreation = _userManager.CreateAsync(user, userToRegister.Password);

            return userCreation;
        }

        public async Task<ResultDetails> Register(UserToRegisterDTO userToRegister)
        {
            if (userToRegister != null)
            {
                var userCreation = await CreateAsync(userToRegister, admin: false);
                if (!userCreation.Succeeded)
                {
                    _logger.LogInfo("AccountService Register - User registration failed.");

                    var errorMessages = userCreation.Errors.Select(e => e.Description);
                    return new ResultDetails
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        IsSuccessful = false,
                        Infos = errorMessages
                    };
                }

                var createdUser = await FindByEmailAsync(userToRegister.Email);
                await AddUserRoleAsync(createdUser, UserRoles.StandardUser);

                _logger.LogInfo("AccountService Register - User registration was successful.");
                return new ResultDetails
                {
                    StatusCode = StatusCodes.Status201Created,
                    IsSuccessful = true,
                    Info = $"Registration for {userToRegister.UserName} was successful."
                };
            }

            _logger.LogInfo("AccountService Register - User registration failed.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccessful = false,
                Info = $"Could not register the user, please make sure you provided a valid user."
            };
        }

        public async Task<ResultDetails> RegisterAdmin(UserToRegisterDTO userToRegister)
        {
            if (userToRegister != null)
            {
                var userFound = await FindByEmailAsync(userToRegister.Email);
                if (userFound != null)
                {
                    var pwdHash = HashPassword(userFound, userToRegister.Password);
                    userFound.PasswordHash = pwdHash;

                    await AddUserRoleAsync(userFound, UserRoles.Admin);

                    _logger.LogInfo("AccountService RegisterAdmin - User registration was successful.");
                    return new ResultDetails
                    {
                        StatusCode = StatusCodes.Status201Created,
                        IsSuccessful = true,
                        Info = $"Registration for {userToRegister.UserName} was successful."
                    }; 
                }

                var userCreation = await Register(userToRegister);
                if (!userCreation.IsSuccessful)
                {
                    return new ResultDetails
                    {
                        StatusCode = userCreation.StatusCode,
                        IsSuccessful = userCreation.IsSuccessful,
                        Infos = userCreation.Infos
                    };
                }

                var createdUser = await FindByEmailAsync(userToRegister.Email);
                await AddUserRoleAsync(createdUser, UserRoles.Admin);

                _logger.LogInfo("AccountService RegisterAdmin - User registration was successful.");
                return new ResultDetails
                {
                    StatusCode = StatusCodes.Status201Created,
                    IsSuccessful = true,
                    Info = $"Registration for {userToRegister.UserName} was successful."
                };
            }

            _logger.LogInfo("AccountService RegisterAdmin - User registration failed.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccessful = false,
                Info = $"Could not register the user, please make sure you provided a valid user."
            };
        }

        public async Task<ResultDetails> Login(UserToLoginDTO userToLogin, HttpResponse response)
        {
            var userFound = await FindByEmailAsync(userToLogin.Email);
            if (userFound != null)
            {
                var signInProcess = await PasswordSignInAsync(userFound.UserName, userToLogin.Password);
                if (signInProcess.Succeeded)
                {
                    var roles = await GetRolesAsync(userFound);
                    var responseDTO = await CreateLoginResponse(userFound, roles);

                    AppendCookies(responseDTO.Token, userFound.UserName, response);

                    _logger.LogInfo("AccountService Login - User login was successful.");
                    return new ResultDetails
                    {
                        StatusCode = StatusCodes.Status200OK,
                        IsSuccessful = true,
                        Payload = responseDTO
                    };
                }
            }

            _logger.LogInfo("AccountService Login - User login failed.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                IsSuccessful = false,
                Info = "Invalid email or password"
            };
        }

        public async Task<ResultDetails> Logout()
        {
            await SignOutAsync();
            this._logger.LogInfo("AccountService Logout - User logged out.");

            return new ResultDetails
            {
                StatusCode = StatusCodes.Status200OK,
                Info = $"Log out success."
            };
        }

        public Task<IdentityResult> AddUserRoleAsync(User user, string role)
        {
            if (role == UserRoles.Admin)
            {
                user.IsAdmin = true;
                _repos.SaveChanges();
            }
            var roleCreation = _userManager.AddToRoleAsync(user, role);

            return roleCreation;
        }

        public Task<IdentityResult> RemoveAdminRoleAsync(User user, string role)
        {
            user.IsAdmin = false;
            _repos.SaveChanges();

            var roleProcess = _userManager.RemoveFromRoleAsync(user, role);

            return roleProcess;
        }

        public Task<User> FindByEmailAsync(string email)
        {
            var userSearch = _userManager.FindByEmailAsync(email);

            return userSearch;
        }

        public Task<SignInResult> PasswordSignInAsync(string userName, string password)
        {
            var signInProcess = _signInManager.PasswordSignInAsync(userName, password, false, false);

            return signInProcess;
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            var rolesTask = _userManager.GetRolesAsync(user);

            return rolesTask;
        }

        public async Task<LoginResponseDTO> CreateLoginResponse(User user, IList<string> roles)
        {
            bool hasAdminRole = roles.Any(r => r == UserRoles.Admin);

            var signinCredentials = _JWTManager.GetSigningCredentials();
            var claims = await _JWTManager.GetClaimsAsync(user) as List<Claim>;
            var tokenOptions = _JWTManager.GenerateTokenOptions(signinCredentials, claims);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            var response = new LoginResponseDTO
            {
                IsLoginSuccessful = true,
                IsAdmin = hasAdminRole,
                Email = user.Email,
                UserName = user.UserName,
                LastName = user.LastName,
                UserId = user.Id,
                Token = token,
                TokenExpirationDate = tokenOptions.ValidTo
            };

            return response;
        }

        public string GetCurrentUserId(HttpContext context)
        {
            return _userManager.GetUserId(context.User);
        }

        public void AppendCookies(string token, string userName, HttpResponse response)
        {
            response.Cookies.Append("X-Access-Token", token,  // --
                new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });

            response.Cookies.Append("X-Username", userName,
                new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });
        }

        public Task SignOutAsync()
        {
            var signOutProcess = _signInManager.SignOutAsync();

            return signOutProcess;
        }
    }
}
