using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCoreApplication.Models.ViewModel
{
    public class SignupViewModel
    {

        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter Username")]
        [Display(Name ="Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter Valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Mobile number")]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = ("confirm password can't matched!"))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
