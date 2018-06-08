using System;

namespace MyWebServer.GameStore.Common
{
    public class ValidationConstants
    {
        public const string InvalidMinLenghtErrorMessage = "{0} must be at least {1} symbols.";
        public const string InvalidMaxLenghtErrorMessage = "{0} cannot be more than {1} symbols.";
        public class Account
        {
            public const int EmailMinLength = 6;
            public const int EmailMaxLength = 30;

            public const int NameMinLength = 2;
            public const int NameMaxLength = 30;

            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 50;
        }

        public class Game
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 100;
            
            public const string PriceMinLength = "0";
            public const string PriceMaxLength = "79228162514264337593543950335";
            
            public const string SizeMinLength = "0";
            public const string SizeMaxLength = "79228162514264337593543950335";

            public const int TrailerMinLength = 11;
            public const int TrailerMaxLength = 11;
            
            public const int ThumbnailUrlMinLength = 6;
            public const int ThumbnailUrlMaxLength = 200;

            public const int DescriptionMinLength = 20;


        }
    }
}