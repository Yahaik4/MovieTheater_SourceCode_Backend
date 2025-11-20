namespace ApiGateway.DataTransferObject.ResultData
{
    public class RegisterResultDTO : BaseResultDTO
    {
        public RegisterDataResult Data { get; set; }
    }

    public class RegisterDataResult
    {
        public Guid UserId { get; set; }
    }
}
