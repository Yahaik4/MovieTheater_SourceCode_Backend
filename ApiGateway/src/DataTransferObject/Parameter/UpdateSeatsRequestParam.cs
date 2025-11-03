namespace src.DataTransferObject.Parameter
{
    public class UpdateSeatsRequestParam
    {
        public List<Guid> Ids { get; set; }
        public bool? IsActive { get; set; }
        public Guid? SeatTypeId { get; set; }
    }
}
