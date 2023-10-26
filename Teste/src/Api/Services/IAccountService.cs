﻿using Api.Components.Pages.Account;
using Api.Data;
using Api.Identity;
using Core.Modules.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Api.Services
{
    public interface IAccountService
    {
        Task<string> Register(ApplicationUser user);
        Task<SignInResult> Login(LoginInputModel loginInput);
        Task<string> ForgotPassword(ApplicationUser user, string? email);
        Task<string> ChangeEmail(ApplicationUser user, string? newEmail);
    }

    public class AccountService : IAccountService
    {
        readonly SignInManager<ApplicationUser> _signInManager;
        readonly ILogger<Register> _logger;
        readonly IdentityRedirectManager _redirectManager;
        readonly UserManager<ApplicationUser> _userManager;
        readonly NavigationManager _navigationManager;
        readonly IUserStore<ApplicationUser> _userStore;


        [SupplyParameterFromQuery] public string ReturnUrl { get; set; } = "";


        public AccountService(SignInManager<ApplicationUser> signInManager,
                               ILogger<Register> logger,
                               IdentityRedirectManager identityRedirectManager,
                               UserManager<ApplicationUser> userManager,
                               NavigationManager navigationManager,
                               IUserStore<ApplicationUser> userStore)
        {
            _signInManager = signInManager;
            _logger = logger;
            _redirectManager = identityRedirectManager;
            _userManager = userManager;
            _navigationManager = navigationManager;
            _userStore = userStore;
        }

        public async Task<string> Register(ApplicationUser user)
        {
            _logger.LogInformation("User created a new account with password.");

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = _navigationManager.GetUriWithQueryParameters("http://10.0.2.2:5225/Account/ConfirmEmail",
                new Dictionary<string, object?> { { "userId", userId }, { "code", code }, { "returnUrl", ReturnUrl } }); //TODO: VERIFICAR MELHOR FORMA PARA OBTER BASEURI

            return callbackUrl;
        }

        public async Task<SignInResult> Login(LoginInputModel inputModel)
        {
            return await _signInManager
                .PasswordSignInAsync(inputModel.Email, inputModel.Password, inputModel.RememberMe, lockoutOnFailure: false);
        }

        public async Task<string> ForgotPassword(ApplicationUser user, string? email)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = _navigationManager.GetUriWithQueryParameters($"http://10.0.2.2:5225/Account/ResetPassword",
                new Dictionary<string, object?> { { "code", code } });  //TODO: VERIFICAR MELHOR FORMA PARA OBTER BASEURI

            return callbackUrl;
        }

        public async Task<string> ChangeEmail(ApplicationUser user, string? newEmail)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail!);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = _navigationManager.GetUriWithQueryParameters($"http://10.0.2.2:5225/Account/ConfirmEmailChange",
               new Dictionary<string, object?> { { "userId", userId }, { "email", newEmail }, { "code", code }, { "isFrontend", true } }); //TODO: VERIFICAR MELHOR FORMA PARA OBTER BASEURI

            return callbackUrl;
        }
    }
}
