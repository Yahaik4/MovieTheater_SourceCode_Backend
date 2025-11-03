namespace src.DataTransferObject.Parameter
{
    public class UpdateSeatStatusParam
    {
        public List<Guid> Id { get; set; }
        public string Status { get; set; }
    }
}
