using System.ComponentModel.DataAnnotations;
using MyWebServer.GameStore.Common;
using MyWebServer.GameStore.Utilities;

namespace MyWebServer.GameStore.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(ValidationConstants.Account.EmailMinLength, 
            ErrorMessage = ValidationConstants.InvalidMinLenghtErrorMessage)]
        [MaxLength(ValidationConstants.Account.EmailMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLenghtErrorMessage)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Full name")]
        [Required]
        [MinLength(ValidationConstants.Account.NameMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLenghtErrorMessage)]
        [MaxLength(ValidationConstants.Account.NameMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLenghtErrorMessage)]
        public string Name { get; set; }

        [Required]
        [MinLength(ValidationConstants.Account.PasswordMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLenghtErrorMessage)]
        [MaxLength(ValidationConstants.Account.PasswordMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLenghtErrorMessage)]
        [Password]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}