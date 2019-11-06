using System;
using System.ComponentModel.DataAnnotations;

namespace AvalancheGamesWeb.Models
{
    public class CreateUser
    {
        public int UserID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(Constants.MaxPasswordLength, ErrorMessage = "The {0} must be between {2} and {1} characters long.",
            MinimumLength = Constants.MinPasswordLength)]
        [RegularExpression(Constants.PasswordRequirements, ErrorMessage = Constants.PasswordRequirementsMessage)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Verify Password")]
        public string PasswordAgain { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        //public string Message { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}