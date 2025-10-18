using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class Cinema : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TimeOnly Open_Time { get; set; }
        public TimeOnly Close_Time { get; set; }
        public int TotalRoom { get; set; } = 0;
        public string Status { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
