using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetCustomerTypesResultData : BaseResultData
    {
        public List<GetCustomerTypesDataResult> Data { get; set; }
    }

    public class GetCustomerTypesDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // Children, U22, Adult,..
        public string RoleCondition { get; set; } // Height < 1.3m, Student ID, Age >= 55,...
    }
}
