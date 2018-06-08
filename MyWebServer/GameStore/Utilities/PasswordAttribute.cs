using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyWebServer.GameStore.Utilities
{
    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute()
        {
            this.ErrorMessage =
                "Password should be least 6 symbols long, containing 1 uppercase letter, 1 lowercase letter and 1 digit!";
        }

        public override bool IsValid(object value)
        {
            var password = value as string;

            if (password == null)
            {
                return true;
            }

            return password.Any(s => char.IsUpper(s))
                   && password.Any(s => char.IsLower(s))
                   && password.Any(s => char.IsDigit(s));
        }
    }
}