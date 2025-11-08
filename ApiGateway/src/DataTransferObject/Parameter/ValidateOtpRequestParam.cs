using System.ComponentModel.DataAnnotations;

namespace src.DataTransferObject.Parameter
{
    public class ValidateOtpRequestParam
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Otp { get; set; }
    }
}