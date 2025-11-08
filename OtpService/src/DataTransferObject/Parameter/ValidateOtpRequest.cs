using System.ComponentModel.DataAnnotations;

namespace src.DataTransferObject.Parameter
{
    public class ValidateOtpRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Otp { get; set; }
    }
}