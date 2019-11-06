using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace AvalancheGamesWeb.Models
{
    public class RegistrationModel
    {
        //registrationUserStuff
        public int UserID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("PasswordAgain", ErrorMessage = "Passwords do not Match")]
        [Required]
        [StringLength(Constants.MaxPasswordLength, ErrorMessage = "The {0} must be between {2} and {1} characters long.",
            MinimumLength = Constants.MinPasswordLength)]
        [RegularExpression(Constants.PasswordRequirements, ErrorMessage = Constants.PasswordRequirementsMessage)]  
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage ="Passwords do not Match")]
        [DataType(DataType.Password)]
        [Display(Name = "Verify Password")]
        public string PasswordAgain { get; set; }


        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        public string Message { get; set; }

        //CommentStuff
        public int CommentID { get; set; }

        public int GameID { get; set; }
        public bool Liked { get; set; }

        //GameStuff
        //public List<SelectListItem> GameName { get; set; }
        [Display(Name = "Game")]
        public string GameName { get; set; }
    }

}