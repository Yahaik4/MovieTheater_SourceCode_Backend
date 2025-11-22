public class VerifyAccountRequestParam
{
    public Guid UserId { get; set; }
    public string Code { get; set; } = string.Empty;
}