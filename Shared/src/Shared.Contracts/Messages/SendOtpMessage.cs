using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Messages
{
    public class SendOtpMessage
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string Purpose { get; set; }
    }
}
