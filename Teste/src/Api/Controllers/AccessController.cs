using Api.Components.Pages.Account;
using Api.Data;
using Core.Modules.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly IUserStore<ApplicationUser> _userStore;
        SignInManager<ApplicationUser> _signInManager;
        readonly ILogger<Register> _logger;
        readonly IEmailSender _emailSender;

        IEnumerable<IdentityError>? identityErrors;
        string? Message => identityErrors is null ? null : "Error: " + string.Join(", ", identityErrors.Select(error => error.Description));

        public AccessController(UserManager<ApplicationUser> userManager,
                                IUserStore<ApplicationUser> userStore,
                                SignInManager<ApplicationUser> signInManager,
                                ILogger<Register> logger,
                                IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _logger = logger;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel inputModel)
        {
            var user = CreateUser();

            try
            {
                await _userStore.SetUserNameAsync(user, inputModel.Email, CancellationToken.None);
                var emailStore = GetEmailStore();
                await emailStore.SetEmailAsync(user, inputModel.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, inputModel.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    ConfirmEmailModel confirmEmail = new()
                    {
                        UserId = userId,
                        Code = code
                    };

                    //var callbackUrl = NavigationManager.GetUriWithQueryParameters(
                    //NavigationManager.ToAbsoluteUri("/Account/ConfirmEmail").AbsoluteUri,
                    //    new Dictionary<string, object?> { { "userId", userId }, { "code", code }, { "returnUrl", ReturnUrl } });

                    //await EmailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    return Ok(new GenericResponse<ConfirmEmailModel> { Successful = true, Data = confirmEmail, StatusCode = Ok().StatusCode, Message = inputModel.Email });

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    //RedirectManager.RedirectTo(
                    //    //    "/Account/RegisterConfirmation",
                    //    //    new() { ["Email"] = Input.Email, ["ReturnUrl"] = ReturnUrl });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    //RedirectManager.RedirectTo(ReturnUrl);
                    //}
                }
                else
                {
                    identityErrors = result.Errors;
                    return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Message = Message, Error = identityErrors.ToString()! });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Error = ex.ToString() });
            }
        }

        [HttpPost("registerconfirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirmation([FromBody] ConfirmEmailModel confirmEmailModel)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(confirmEmailModel.Email!);
                if (user == null)
                {
                    // Need a way to trigger a 404 from Blazor: https://github.com/dotnet/aspnetcore/issues/45654
                    //statusMessage = $"Error finding user for unspecified email";
                    return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Message = "Error finding user for unspecified email" });

                }
                else if (_emailSender is NoOpEmailSender)
                {
                    // Once you add a real email sender, you should remove this code that lets you confirm the account

                    var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailModel.Code));
                    var result = await _userManager.ConfirmEmailAsync(user, code);
                    if (!result.Succeeded)
                        return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Message = "Error confirming your email." });

                    return Ok(new GenericResponse { Successful = true, StatusCode = Ok().StatusCode, Message = "Sucesso!" });
                }
                else
                {
                    return Ok(new GenericResponse { Successful = true, StatusCode = Ok().StatusCode, Message = "Email já confirmado!" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Error = ex.ToString() });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginInputModel inputModel)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(inputModel.Email, inputModel.Password, inputModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    var user = await _userManager.FindByEmailAsync(inputModel.Email!);

                    UserDTO userDTO = new()
                    {
                        UserID = user!.Id,
                        Email = user.Email
                    };

                    return Ok(new GenericResponse<UserDTO> { Successful = true,/* Token = token,*/ Data = userDTO, StatusCode = Ok().StatusCode, Message = inputModel.Email });

                    //RedirectManager.RedirectTo(ReturnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Message = "RequiresTwoFactor" });

                    //RedirectManager.RedirectTo(
                    //    "/Account/LoginWith2fa",
                    //    new() { ["ReturnUrl"] = ReturnUrl, ["RememberMe"] = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    //RedirectManager.RedirectTo("/Account/Lockout");
                    return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Message = "User account locked out." });
                }
                else
                {
                    //errorMessage = "Error: Invalid login attempt.";
                    return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Message = "Error: Invalid login attempt." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Error = ex.ToString() });
            }
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var user = HttpContext.User;

                if (_signInManager.IsSignedIn(user))
                {
                    await _signInManager.SignOutAsync();
                    _logger.LogInformation("User logout.");
                    return Ok(new GenericResponse { Successful = true,/* Token = token, Data = usuario,*/ StatusCode = Ok().StatusCode, Message = user.Identity.Name });
                }
                return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Error = "Error logout" });
            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Error = ex.ToString() });
            }
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                if (user is null || !isEmailConfirmed)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    //RedirectManager.RedirectTo("/Account/ForgotPasswordConfirmation");

                    var message = user is null ? "Usuário não encontrado" : (!isEmailConfirmed ? "Email foi enviado porém não confirmado" : "Errro desconhecido");

                    //Email foi enviado porém não confirmadoo. Orientar a verificar email. 

                    return Ok(new GenericResponse<string> { Successful = true, StatusCode = Ok().StatusCode, Message = $"{message}" }); ;
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                //enviar email
                //    await EmailSender.SendEmailAsync(
                //Input.Email,
                //"Reset Password",
                //$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return Ok(new GenericResponse<string> { Successful = true, Data = code, StatusCode = Ok().StatusCode, Message = "Verifique sua caixa de email!" });

            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Error = ex.ToString() });
            }
        }

        [HttpPost("newpassword/{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> NewPassword([FromBody] RegisterInputModel inputModel, string? token)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(inputModel.Email);
                if (user is null)
                {
                    // Don't reveal that the user does not exist
                    //RedirectManager.RedirectTo("/Account/ResetPasswordConfirmation");
                    return Ok(new GenericResponse { Successful = true, StatusCode = Ok().StatusCode, Message = "Erro, porem não revelar" });
                }

                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

                var result = await _userManager.ResetPasswordAsync(user, code, inputModel.Password);
                if (result.Succeeded)
                {
                    //RedirectManager.RedirectTo("/Account/ResetPasswordConfirmation");
                    return Ok(new GenericResponse { Successful = true, StatusCode = Ok().StatusCode, Message = "Sucesso, seguir para login" });
                }

                identityErrors = result.Errors;

                return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Error = identityErrors.ToString(), Message = Message });

            }
            catch (Exception ex)
            {
                return BadRequest(new GenericResponse { StatusCode = BadRequest().StatusCode, Error = ex.ToString() });
            }
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
