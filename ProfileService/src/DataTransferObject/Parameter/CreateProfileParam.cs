using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class CreateProfileParam : IParam
    {
        public string FullName { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }
}
