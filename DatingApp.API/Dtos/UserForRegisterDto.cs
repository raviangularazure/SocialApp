using System.ComponentModel.DataAnnotations;
using System;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 16 characters")]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

    }
}