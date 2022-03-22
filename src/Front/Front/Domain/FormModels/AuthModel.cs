using System.ComponentModel.DataAnnotations;

namespace Front.Domain.FormModels
{
    public class AuthModel
    {
        [Required(ErrorMessage = "Username field is required")]
        [MinLength(10, ErrorMessage = "Login must be greater then 3 symbhol")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password field is required")]
        [MinLength(6, ErrorMessage = "Password must be greater then 6 symbhol")]
        public string Password { get; set; }
    }
}
