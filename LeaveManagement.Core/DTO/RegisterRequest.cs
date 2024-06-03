using LeaveManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Core.DTO
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [EmailAddress(ErrorMessage = "{0} should be in a valid email address format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Phone Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} should contain numbers only")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and {0} should match")]
        public string ConfirmPassword { get; set; }

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.Employee;
    }
}
