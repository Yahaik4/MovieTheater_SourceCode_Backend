using System.ComponentModel.DataAnnotations;
using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class ConfirmChangePasswordParam : IParam
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Otp { get; set; }
    }
}
