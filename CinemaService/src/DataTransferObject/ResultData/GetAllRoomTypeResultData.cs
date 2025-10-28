using Shared.Contracts.ResultData;

namespace src.DataTransferObject.ResultData
{
    public class GetAllRoomTypeResultData : BaseResultData
    {
        public List<GetAllRoomTypesDataResult> Data { get; set; }
    }

    public class GetAllRoomTypesDataResult
    {
        public string Type { get; set; }
        public decimal BasePrice { get; set; }
    }
}
