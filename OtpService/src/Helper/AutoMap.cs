using AutoMapper;
using OTPGrpc;
using src.DataTransferObject.ResultData;

namespace src.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap() 
        {
            // LoginMapping
            CreateMap<GenerateOtpResult, GenerateOtpResultGrpc>();
            CreateMap<GenerateOtpData, GenerateOtpDataGrpc>();

            // RefreshTokenMapping
            CreateMap<ValidateOtpResult, ValidateOtpResultGrpc>();
            CreateMap<ValidateOtpData, ValidateOtpDataGrpc>();

        }
    }
}
