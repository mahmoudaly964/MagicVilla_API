﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.DTO
{
    public class VillaDTOUpdate
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int Occupancy { get; set; }
        public string Amenity { get; set; }

        public int Area { get; set; }
    }
}
