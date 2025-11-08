using System.ComponentModel.DataAnnotations;

namespace src.DataTransferObject.Parameter
{
    public class ChangePasswordRequestParam
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
