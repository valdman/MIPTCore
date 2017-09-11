﻿using System.ComponentModel.DataAnnotations;

namespace MIPTCore.Models
{
    public class Credentials
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}