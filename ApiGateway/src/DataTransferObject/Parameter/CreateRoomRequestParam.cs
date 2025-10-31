using System;
using System.ComponentModel.DataAnnotations;

namespace src.DataTransferObject.Parameter
{
    public class CreateRoomRequestParam
    {
        [Required(ErrorMessage = "RoomNumber is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "RoomNumber must be greater than 0.")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(Active|Inactive|Maintenance)$",
            ErrorMessage = "Status must be 'Active', 'Inactive' or 'Maintenance'.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Total_Column is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total_Column must be greater than 0.")]
        public int TotalColumn { get; set; }

        [Required(ErrorMessage = "Total_Row is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total_Row must be greater than 0.")]
        public int TotalRow { get; set; }

        [Required(ErrorMessage = "RoomTypeId is required.")]
        public Guid RoomTypeId { get; set; }

        [Required(ErrorMessage = "CinemaId is required.")]
        public Guid CinemaId { get; set; }
    }
}
