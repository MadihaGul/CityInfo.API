﻿using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage ="Plz provide name")] // customized error 
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
