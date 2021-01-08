using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.ActivityScheduler.DataAccess;
using WebAPI.ActivityScheduler.Entities;
using WebAPI.ActivityScheduler.EntitiesDTO.Login;
using WebAPI.ActivityScheduler.EntitiesDTO.Registration;
using WebAPI.ActivityScheduler.JWTFeatures;
using WebAPI.ActivityScheduler.Services;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(
            ActivitySchedulerDbContext dbContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            JWTAuthManager JWTManager,
            IMapper mapper,
            ILoggerManager logger)
        {
            this._db = dbContext;
            this._userManager = userManager;            
            this._signInManager = signInManager;
            this._JWTManager = JWTManager;
            this._mapper = mapper;
            this._logger = logger;
        }


        private readonly ActivitySchedulerDbContext _db;        
        private readonly UserManager<User> _userManager;        
        private readonly SignInManager<User> _signInManager;
        private readonly JWTAuthManager _JWTManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        // POST: account/register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserToRegisterDTO userToRegister)
        {
            if (userToRegister != null)
            {
                var user = _mapper.Map<User>(userToRegister);
                var userCreation = await _userManager.CreateAsync(user, userToRegister.Password);

                if (!userCreation.Succeeded)
                {
                    this._logger.LogInfo("AccountController Register - User registration failed.");

                    var errorMessages = userCreation.Errors.Select(e => e.Description);
                    return StatusCode(
                        StatusCodes.Status400BadRequest, 
                        new RegistrationResponseDTO(isRegistrationSuccessful: false, errorMessages)
                    );
                }
                
                await _userManager.AddToRoleAsync(user, UserRoles.StandardUser);
                this._logger.LogInfo("AccountController Register - User registration was successful.");

                return StatusCode(
                    StatusCodes.Status201Created,
                    new RegistrationResponseDTO(
                        isRegistrationSuccessful: true, 
                        info: $"Registration for {userToRegister.UserName} was successful."
                    )
                ); 
            }

            this._logger.LogInfo("AccountController Register - User registration failed.");
            return StatusCode(
                StatusCodes.Status400BadRequest,
                new RegistrationResponseDTO(
                    isRegistrationSuccessful: false, 
                    info: $"Could not register the user, please make sure you provided a valid user."
                ) 
            );
        }

        // POST: account/register-admin
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("register-admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] UserToRegisterDTO userToRegister)
        {            
            if (userToRegister != null)
            {
                var userFound = await _userManager.FindByEmailAsync(userToRegister.Email);
                if (userFound != null)
                {                    
                    var hasher = new PasswordHasher<User>();
                    var PwdHash = hasher.HashPassword(userFound, userToRegister.Password);
                    userFound.PasswordHash = PwdHash;

                    await _userManager.AddToRoleAsync(userFound, UserRoles.Admin);                    
                    _db.SaveChanges();

                    this._logger.LogInfo("AccountController RegisterAdmin - User registration was successful.");
                    return StatusCode(
                        StatusCodes.Status201Created,
                        new RegistrationResponseDTO(
                            isRegistrationSuccessful: true,
                            info: $"Registration for {userToRegister.UserName} was successful."
                        )
                    );
                }
                
                var user = _mapper.Map<User>(userToRegister);
                var userCreation = await _userManager.CreateAsync(user, userToRegister.Password);

                if (!userCreation.Succeeded)
                {
                    this._logger.LogInfo("AccountController RegisterAdmin - User registration failed.");

                    var errorMessages = userCreation.Errors.Select(e => e.Description);
                    return StatusCode(
                        StatusCodes.Status400BadRequest,
                        new RegistrationResponseDTO(isRegistrationSuccessful: false, errorMessages)
                    );
                }

                var roles = new List<string> { UserRoles.Admin, UserRoles.StandardUser };
                await _userManager.AddToRolesAsync(user, roles);

                this._logger.LogInfo("AccountController RegisterAdmin - User registration was successful.");
                return StatusCode(
                    StatusCodes.Status201Created,
                    new RegistrationResponseDTO(
                        isRegistrationSuccessful: true,
                        info: $"Registration for {userToRegister.UserName} was successful."
                    )
                );
            }

            this._logger.LogInfo("AccountController RegisterAdmin - User registration failed.");
            return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new RegistrationResponseDTO(
                        isRegistrationSuccessful: false,
                        info: $"Could not register the user, please make sure you provided a valid user."
                    )
            );
        }

        // POST: account/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserToLoginDTO userToLogin)
        {
            var userFound = await _userManager.FindByEmailAsync(userToLogin.Email);
            if (userFound != null)
            {
                var signInProcess = await _signInManager
                    .PasswordSignInAsync(userFound.UserName, userToLogin.Password, false, false);
                if (signInProcess.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(userFound);
                    bool isAdmin = roles.Any(r => r == UserRoles.Admin);

                    var signinCredentials = _JWTManager.GetSigningCredentials();
                    var claims = await _JWTManager.GetClaimsAsync(userFound) as List<Claim>;
                    var tokenOptions = _JWTManager.GenerateTokenOptions(signinCredentials, claims);
                    
                    string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    
                    Response.Cookies.Append("X-Access-Token", token,  // --
                        new CookieOptions() 
                        { 
                            HttpOnly = true, 
                            SameSite = SameSiteMode.Strict 
                        });
                    Response.Cookies.Append("X-Username", userFound.UserName, 
                        new CookieOptions() 
                        { 
                            HttpOnly = true, 
                            SameSite = SameSiteMode.Strict 
                        });
                    
                    var response = new LoginResponseDTO(
                            isLoginSuccessful: true,
                            isAdmin: isAdmin,
                            email: userFound.Email,
                            id: userFound.Id,
                            token: token,
                            tokenOptions.ValidTo
                        );

                    this._logger.LogInfo("AccountController Login - User login was successful.");
                    return StatusCode(StatusCodes.Status200OK, response);
                }

                this._logger.LogInfo("AccountController Login - User login failed.");
            }

            this._logger.LogInfo("AccountController Login - User login failed.");
            return StatusCode(
                StatusCodes.Status401Unauthorized,
                new LoginResponseDTO(isLoginSuccessful: false, errorMessage: "Invalid email or password")
            );
        }

        // GET: account/logout
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            this._logger.LogInfo("AccountController Logout - User logged out.");
            return StatusCode(StatusCodes.Status200OK, $"Log out success.");
        }
    }
}
