using ActivityScheduler.Domain.Structs;
using ActivityScheduler.Presentation.EntitiesDTO;
using ActivityScheduler.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(
            IAccountService accountService,
            ILoggerManager logger)
        {
            this._logger = logger;
            this._accountService = accountService;
        }


        private readonly ILoggerManager _logger;
        private readonly IAccountService _accountService;
        
        // POST: account/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserToRegisterDTO userToRegister)
        {
            var registerProcess = await _accountService.Register(userToRegister);
            if (registerProcess.IsSuccessful)
            {
                return StatusCode(
                        registerProcess.StatusCode,
                        new RegistrationResponseDTO
                        {
                            IsRegistrationSuccessful = registerProcess.IsSuccessful,
                            Info = registerProcess.Info,
                            ErrorMessages = registerProcess.Infos
                        }
                    );
            }

            return StatusCode(
                    registerProcess.StatusCode,
                    new RegistrationResponseDTO
                    {
                        IsRegistrationSuccessful = registerProcess.IsSuccessful,
                        Info = registerProcess.Info,
                        ErrorMessages = registerProcess.Infos
                    }
                );
        }

        //POST: account/register-admin
       [Authorize(Roles = UserRoles.Admin)]
       [HttpPost("register-admin")]
        public async Task<ActionResult<RegistrationResponseDTO>> RegisterAdmin(UserToRegisterDTO userToRegister)
        {
            var registerProcess = await _accountService.RegisterAdmin(userToRegister);
            if (registerProcess.IsSuccessful)
            {
                return StatusCode(
                        registerProcess.StatusCode,
                        new RegistrationResponseDTO
                        {
                            IsRegistrationSuccessful = registerProcess.IsSuccessful,
                            Info = registerProcess.Info,
                            ErrorMessages = registerProcess.Infos
                        }
                    );
            }

            return StatusCode(
                    registerProcess.StatusCode,
                    new RegistrationResponseDTO
                    {
                        IsRegistrationSuccessful = registerProcess.IsSuccessful,
                        Info = registerProcess.Info,
                        ErrorMessages = registerProcess.Infos
                    }
                );
        }

        // POST: account/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(UserToLoginDTO userToLogin)
        {
            var loginProcess = await _accountService.Login(userToLogin, Response);
            if (loginProcess.IsSuccessful)
            {
                return StatusCode(loginProcess.StatusCode, loginProcess.Payload);
            }

            return StatusCode(
                    loginProcess.StatusCode,
                    new LoginResponseDTO
                    {
                        IsLoginSuccessful = loginProcess.IsSuccessful,
                        ErrorMessage = loginProcess.Info
                    }
                );
        }

        // GET: account/logout
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            var signOutProcess = await _accountService.Logout();
            return StatusCode(
                signOutProcess.StatusCode,
                new InfoResponseDTO
                {
                    Info = signOutProcess.Info
                });
        }
    }
}
