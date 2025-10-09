namespace src.DataTransferObject.Parameter
{
    public class LoginParam : IParam
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? IpAddress { get; set; } = string.Empty;
        public string? Device { get; set; } = string.Empty;
    }
}
