namespace src.DataTransferObject.ResultData
{
    public class GetGenresResultDTO : BaseResultDTO
    {
        public List<GetGenresDataResult> Data { get; set; }
    }

    public class GetGenresDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
