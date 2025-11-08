using System.ComponentModel.DataAnnotations;

namespace src.DataTransferObject.Parameter
{
    public class GenerateOtpRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
    }
}