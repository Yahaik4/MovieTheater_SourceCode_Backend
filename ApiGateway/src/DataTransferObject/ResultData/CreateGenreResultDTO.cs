namespace src.DataTransferObject.ResultData
{
    public class CreateGenreResultDTO : BaseResultDTO
    {
        public CreateGenreDataResult Data { get; set; }
    }

    public class CreateGenreDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
