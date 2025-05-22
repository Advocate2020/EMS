using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DisplayName("Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}