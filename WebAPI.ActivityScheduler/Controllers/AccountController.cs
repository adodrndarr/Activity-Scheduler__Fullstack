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


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(ActivitySchedulerDbContext dbContext,
                                 UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 JWTAuthManager JWTManager,
                                 IMapper mapper                                       
                                )
        {
            this._db = dbContext;
            this._userManager = userManager;            
            this._signInManager = signInManager;
            this._JWTManager = JWTManager;
            this._mapper = mapper;
        }


        private readonly ActivitySchedulerDbContext _db;        
        private readonly UserManager<User> _userManager;        
        private readonly SignInManager<User> _signInManager;
        private readonly JWTAuthManager _JWTManager;
        private readonly IMapper _mapper;

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
                    var errorMessages = userCreation.Errors.Select(e => e.Description);
                    return StatusCode(
                        StatusCodes.Status400BadRequest, 
                        new RegistrationResponseDTO(isRegistrationSuccessful: false, errorMessages)
                    );
                }

                await _userManager.AddToRoleAsync(user, UserRoles.StandardUser);
                return StatusCode(
                    StatusCodes.Status201Created, 
                    $"Registration for {userToRegister.UserName} was successful."
                ); 
            }

            return StatusCode(
                StatusCodes.Status400BadRequest, 
                $"Could not register the user, please make sure you provided a valid user."
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

                    return StatusCode(
                        StatusCodes.Status201Created, 
                        $"Registration for {userToRegister.UserName} was successful."
                    );
                }
                
                var user = _mapper.Map<User>(userToRegister);
                var userCreation = await _userManager.CreateAsync(user, userToRegister.Password);

                if (!userCreation.Succeeded)
                {
                    var errorMessages = userCreation.Errors.Select(e => e.Description);
                    return StatusCode(
                        StatusCodes.Status400BadRequest,
                        new RegistrationResponseDTO(isRegistrationSuccessful: false, errorMessages)
                    );
                }

                var roles = new List<string> { UserRoles.Admin, UserRoles.StandardUser };
                await _userManager.AddToRolesAsync(user, roles);

                return StatusCode(
                    StatusCodes.Status201Created, 
                    $"Registration for {userToRegister.UserName} was successful."
                );
            }

            return StatusCode(
                StatusCodes.Status400BadRequest,
                $"Could not register the user, please make sure you provided a valid user."
            );
        }

        // POST: account/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserToLoginDTO userToLogin)
        {
            var userFound = await _userManager.FindByEmailAsync(userToLogin.Email);
            if (userFound != null)
            {
                var signInProcess = await _signInManager.PasswordSignInAsync(userFound.UserName, userToLogin.Password, false, false);
                if (signInProcess.Succeeded)
                {
                    var signinCredentials = _JWTManager.GetSigningCredentials();
                    var claims = await _JWTManager.GetClaimsAsync(userFound) as List<Claim>;
                    var tokenOptions = _JWTManager.GenerateTokenOptions(signinCredentials, claims);

                    string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    return StatusCode(
                        StatusCodes.Status200OK,
                        new LoginResponseDTO(isLoginSuccessful: true, token: token)
                    );
                }
            }

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
            return StatusCode(StatusCodes.Status200OK, $"Log out success.");
        }
    }
}
