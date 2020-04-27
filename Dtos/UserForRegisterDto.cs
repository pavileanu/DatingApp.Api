using System.ComponentModel.DataAnnotations;

namespace MyApi.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password between 4 and 8 chars")]
        public string password {get; set;}
    }
}