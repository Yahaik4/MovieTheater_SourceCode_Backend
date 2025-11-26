namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetBookingHistoryResultDTO : BaseResultDTO
    {
        public List<CreateBookingDataResult> Data { get; set; }
    }
}
