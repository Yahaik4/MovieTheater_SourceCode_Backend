namespace src.DataTransferObject.ResultData
{
    public class UpdateGenreResultDTO : BaseResultDTO
    {
        public UpdateGenreDataResult Data { get; set; }
    }

    public class UpdateGenreDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
