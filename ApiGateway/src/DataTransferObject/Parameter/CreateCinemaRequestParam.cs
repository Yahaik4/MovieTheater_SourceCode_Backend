using System.ComponentModel.DataAnnotations;

namespace src.DataTransferObject.Parameter
{
    public class CreateCinemaRequestParam
    {
        [Required(ErrorMessage = "Cinema name is required.")]
        [StringLength(100, ErrorMessage = "Cinema name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Open time is required.")]
        public string Open_Time { get; set; }

        [Required(ErrorMessage = "Close time is required.")]
        public string Close_Time { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(Active|Inactive|Maintenance)$",
            ErrorMessage = "Status must be 'Active', 'Inactive' or 'Maintenance'.")]
        public string Status { get; set; }
    }
}
