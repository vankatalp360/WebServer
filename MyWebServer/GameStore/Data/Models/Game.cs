using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyWebServer.GameStore.Common;

namespace MyWebServer.GameStore.Data.Models
{
    public class Game
    {
        public int Id { get; set; }

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
        
        public List<UserGame> Users { get; set; } = new List<UserGame>();
        public List<OrderGame> Orders { get; set; } = new List<OrderGame>();
    }
}