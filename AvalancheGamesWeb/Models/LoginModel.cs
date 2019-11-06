using System.ComponentModel.DataAnnotations;

namespace AvalancheGamesWeb.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(Constants.MaxPasswordLength, ErrorMessage = "The {0} must be between {2} and {1} characters long.",
            MinimumLength = Constants.MinPasswordLength)]
        [RegularExpression(Constants.PasswordRequirements, ErrorMessage = Constants.PasswordRequirementsMessage)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string Message { get; set; }
        public string ReturnURL { get; set; }
    }
}