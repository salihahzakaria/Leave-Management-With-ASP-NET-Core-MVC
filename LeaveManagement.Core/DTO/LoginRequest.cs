using System.ComponentModel.DataAnnotations;

namespace LeaveManagement.Core.DTO
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "{0} cannot be blank")]
        [EmailAddress(ErrorMessage = "{0} should be in a valid email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
