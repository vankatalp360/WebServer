using System;
using System.ComponentModel.DataAnnotations;
using MyWebServer.GameStore.Common;

namespace MyWebServer.GameStore.ViewModels
{
    public class GameViewModel
    {

        [Required]
        [MinLength(ValidationConstants.Game.TitleMinLength)]
        [MaxLength(ValidationConstants.Game.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [Range(typeof(decimal), ValidationConstants.Game.PriceMinLength, ValidationConstants.Game.PriceMaxLength)]
        public decimal Price { get; set; }

        [Required]
        [Range(typeof(decimal), ValidationConstants.Game.SizeMinLength, ValidationConstants.Game.SizeMaxLength)]
        public decimal Size { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.TrailerMinLength)]
        [MaxLength(ValidationConstants.Game.TrailerMaxLength)]
        public string TrailerId { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.ThumbnailUrlMinLength)]
        [MaxLength(ValidationConstants.Game.ThumbnailUrlMaxLength)]
        public string ThumbnailURL { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.DescriptionMinLength)]
        public string Desctription { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }
    }
}