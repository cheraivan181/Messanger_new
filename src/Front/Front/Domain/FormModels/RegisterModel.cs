using System.ComponentModel.DataAnnotations;

namespace Front.Domain.FormModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username must be filled required")]
        [MinLength(10, ErrorMessage = "Minimum username length 3")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password must be filled required")]
        [MinLength(6, ErrorMessage = "Minimum password length 6")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone is required field")]
        [Phone(ErrorMessage = "Enter correct phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Email is required field")]
        [EmailAddress(ErrorMessage = "Enter correct email")]
        public string Email { get; set; }
    }
}
