using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyWebServer.GameStore.Common;

namespace MyWebServer.GameStore.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.Account.EmailMinLength)]
        [MaxLength(ValidationConstants.Account.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MinLength(ValidationConstants.Account.PasswordMinLength)]
        [MaxLength(ValidationConstants.Account.PasswordMaxLength)]
        public string Password { get; set; }
        
        [Required]
        [MinLength(ValidationConstants.Account.NameMinLength)]
        [MaxLength(ValidationConstants.Account.NameMaxLength)]
        public string Name { get; set; }

        public List<UserGame> Games { get; set; } = new List<UserGame>();
        public List<Order> Orders { get; set; } = new List<Order>();

        public bool IsAdministrator { get; set; } = false;
    }
}