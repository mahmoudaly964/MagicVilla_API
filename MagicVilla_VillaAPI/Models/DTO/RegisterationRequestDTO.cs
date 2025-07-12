using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.DTO
{
    public class RegisterationRequestDTO
    {
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
