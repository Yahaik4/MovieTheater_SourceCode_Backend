using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class CreateCinemaParam : IParam
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }
        public string? Status { get; set; }
        public string CreateBy { get; set; }
    }
}
