using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsAggregator.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please write correct email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Login { get; set; }
        [Required(ErrorMessage = "Please write correct password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Incorrect password")]
        public string PasswordConfirmation { get; set; }
    }
}
