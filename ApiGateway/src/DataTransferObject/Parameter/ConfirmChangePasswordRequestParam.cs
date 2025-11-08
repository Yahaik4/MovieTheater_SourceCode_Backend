using System.ComponentModel.DataAnnotations;

namespace src.DataTransferObject.Parameter
{
    public class ConfirmChangePasswordRequestParam
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Otp { get; set; }
    }
}
