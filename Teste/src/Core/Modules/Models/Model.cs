﻿using System.ComponentModel.DataAnnotations;

namespace Core.Modules.Models
{
    public class RegisterInputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]


        public string ConfirmPassword { get; set; } = null!;
    }

    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } = false;
    }

    public class ConfirmEmailModel
    {
        public string? Email { get; set; }
        public string? UserId { get; set; }
        public string? Code { get; set; }
    }

    public class UserDTO
    {
        public string? UserID { get; set; }
        public string? Email { get; set; }
    }

    public class SaveFile
    {
        public List<FileData>? Files { get; set; }
    }

    public class FileData
    {
        public byte[]? Data { get; set; }
        public string? FileType { get; set; }
        public long Size { get; set; }

    }
}
