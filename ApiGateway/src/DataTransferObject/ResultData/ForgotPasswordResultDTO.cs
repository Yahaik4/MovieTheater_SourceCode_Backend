using ApiGateway.DataTransferObject.ResultData;
using AuthenticationGrpc;

public class ForgotPasswordResultDTO : BaseResultDTO { }

public class VerifyPasswordResultDTO : BaseResultDTO
{
    public ResetpassWordReply? Data { get; set; }
}