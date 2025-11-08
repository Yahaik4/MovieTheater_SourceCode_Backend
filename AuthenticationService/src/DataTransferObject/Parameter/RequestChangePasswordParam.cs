using System.ComponentModel.DataAnnotations;
using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class RequestChangePasswordParam : IParam
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

    }
}
