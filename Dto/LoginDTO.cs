﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Dto
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
