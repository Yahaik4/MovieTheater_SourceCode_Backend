using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.DataTransferObject.ResultData;
using ApiGateway.Helper;
using ApiGateway.ServiceConnector.AuthenticationService;
using ApiGateway.ServiceConnector.CinemaService;
using ApiGateway.ServiceConnector.OTPService;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Contracts.Constants;
using Shared.Contracts.Enums;
using Shared.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api")]
    public class ManagerController : ControllerBase
    {
        private readonly AuthenticationServiceConnector _authenticationConnector;
        private readonly ICurrentUserService _currentUserService;
        private readonly CinemaServiceConnector _cinemaServiceConnector;

        public ManagerController(AuthenticationServiceConnector authenticationServiceConnector, CinemaServiceConnector cinemaServiceConnector, ICurrentUserService currentUserService)
        {
            _authenticationConnector = authenticationServiceConnector;
            _currentUserService = currentUserService;
            _cinemaServiceConnector = cinemaServiceConnector;
        }

        [HttpPost("customer")]
        [Authorize(Policy = "OperationsManagerOnly")]
        public async Task<RegisterResultDTO> AddCustomer([FromBody] AddCustomerRequestParam param)
        {
            try
            {
                var result = await _authenticationConnector.AddUser(
                    fullName: param.FullName,
                    email: param.Email,
                    password: param.Password,
                    role: UserRoleEnum.Customer,
                    phoneNumber: null,
                    dayOfBirth: null,
                    gender: null,
                    cinemaId: null,
                    position: null,
                    salary: null
                );

                return new RegisterResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data == null
                        ? null
                        : new RegisterDataResult
                        {
                            UserId = Guid.Parse(result.Data.UserId)
                        }
                };
            }
            catch (Grpc.Core.RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new RegisterResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        private int GetPositionLevel(string? position)
        {
            if (string.IsNullOrWhiteSpace(position))
                return 0;

            position = position.ToLowerInvariant();

            return position switch
            {
                StaffPositionEnum.Staff => 1,
                StaffPositionEnum.CinemaManager => 2,
                StaffPositionEnum.OperationsManager => 3,
                _ => 0
            };
        }

        [HttpPost("staff")]
        [Authorize(Policy = "CinemaManagerOrHigher")]
        public async Task<RegisterResultDTO> AddStaff([FromBody] AddStaffRequestParam param)
        {
            try
            {
                var callerRole = _currentUserService.Role;
                var callerPosition = _currentUserService.Position;

                if (callerRole != UserRoleEnum.Admin)
                {
                    int callerLevel = GetPositionLevel(callerPosition);
                    int targetLevel = GetPositionLevel(param.Position);

                    if (callerLevel <= targetLevel)
                    {
                        return new RegisterResultDTO
                        {
                            Result = false,
                            Message = "You cannot create a staff member with same or higher position.",
                            StatusCode = (int)StatusCodeEnum.Forbidden
                        };
                    }
                }
                
                var result = await _authenticationConnector.AddUser(
                    fullName: param.FullName,
                    email: param.Email,
                    password: param.Password,
                    role: UserRoleEnum.Staff,        // luôn là staff
                    phoneNumber: param.PhoneNumber,
                    dayOfBirth: param.DayOfBirth,
                    gender: param.Gender,
                    cinemaId: param.CinemaId,
                    position: param.Position,
                    salary: param.Salary.ToString()
                );

                return new RegisterResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data == null
                        ? null
                        : new RegisterDataResult
                        {
                            UserId = Guid.Parse(result.Data.UserId)
                        }
                };
            }
            catch (Grpc.Core.RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new RegisterResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("customers")]
        public async Task<GetCustomersResultDTO> GetCustomers([FromQuery] Guid? userId)
        {
            try
            {
                var grpcResult = await _authenticationConnector.GetCustomers(
                    userId: userId?.ToString()
                );

                return new GetCustomersResultDTO
                {
                    Result = grpcResult.Result,
                    Message = grpcResult.Message,
                    StatusCode = grpcResult.StatusCode,
                    Data = grpcResult.Customers
                        .Select(c => new CustomerDTO
                        {
                            UserId = Guid.Parse(c.User.UserId),
                            FullName = c.User.FullName,
                            Email = c.User.Email,
                            Role = c.User.Role,
                            IsVerified = c.User.IsVerified,
                            PhoneNumber = c.PhoneNumber,
                            DayOfBirth = c.DayOfBirth,
                            Gender = c.Gender,
                            Points = c.Points
                        })
                        .ToList()
                };
            }
            catch (Grpc.Core.RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new GetCustomersResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                    Data = new List<CustomerDTO>()
                };
            }
        }

        [HttpGet("staffs")]
        [Authorize(Policy = "StaffOrHigher")]
        public async Task<GetStaffsResultDTO> GetStaffs(
            [FromQuery] Guid? userId,
            [FromQuery] Guid? cinemaId   // NEW
        )
        {
            try
            {
                var grpcResult = await _authenticationConnector.GetStaffs(
                    userId: userId?.ToString(),
                    cinemaId: cinemaId        // NEW
                );

                return new GetStaffsResultDTO
                {
                    Result = grpcResult.Result,
                    Message = grpcResult.Message,
                    StatusCode = grpcResult.StatusCode,
                    Data = grpcResult.Staffs
                        .Select(s => new StaffDTO
                        {
                            UserId = Guid.Parse(s.User.UserId),
                            FullName = s.User.FullName,
                            Email = s.User.Email,
                            Role = s.User.Role,
                            IsVerified = s.User.IsVerified,
                            PhoneNumber = s.PhoneNumber,
                            DayOfBirth = s.DayOfBirth,
                            Gender = s.Gender,
                            CinemaId = Guid.Parse(s.CinemaId),
                            Position = s.Position,
                            Salary = decimal.Parse(s.Salary)
                        })
                        .ToList()
                };
            }
            catch (Grpc.Core.RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new GetStaffsResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                    Data = new List<StaffDTO>()
                };
            }
        }

        [HttpDelete("users/{userId:guid}")]
        [Authorize(Policy = "CinemaManagerOrHigher")]
        public async Task<DeleteUserResultDTO> DeleteUser([FromRoute] Guid userId)
        {
            try
            {
                var callerRole = _currentUserService.Role ?? string.Empty;
                var callerPosition = _currentUserService.Position;

                var result = await _authenticationConnector.DeleteUser(
                    targetUserId: userId,
                    callerRole: callerRole,
                    callerPosition: callerPosition
                );

                return new DeleteUserResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }
            catch (Grpc.Core.RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new DeleteUserResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPut("customers/{userId:guid}")]
        [Authorize(Policy = "OperationsManagerOnly")]
        public async Task<UpdateUserResultDTO> UpdateCustomer(
            [FromRoute] Guid userId,
            [FromBody] UpdateCustomerRequestParam param)
        {
            try
            {
                var callerRole = _currentUserService.Role ?? string.Empty;
                var callerPosition = _currentUserService.Position;

                var result = await _authenticationConnector.UpdateCustomer(
                    targetUserId: userId,
                    callerRole: callerRole,
                    callerPosition: callerPosition,
                    fullName: param.FullName,
                    phoneNumber: param.PhoneNumber,
                    dayOfBirth: param.DayOfBirth,
                    gender: param.Gender,
                    points: param.Points
                );

                return new UpdateUserResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }
            catch (Grpc.Core.RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new UpdateUserResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPut("staffs/{userId:guid}")]
        [Authorize(Policy = "CinemaManagerOrHigher")]
        public async Task<UpdateUserResultDTO> UpdateStaff(
            [FromRoute] Guid userId,
            [FromBody] UpdateStaffRequestParam param)
        {
            try
            {
                var callerRole = _currentUserService.Role ?? string.Empty;
                var callerPosition = _currentUserService.Position;

                var result = await _authenticationConnector.UpdateStaff(
                    targetUserId: userId,
                    callerRole: callerRole,
                    callerPosition: callerPosition,
                    fullName: param.FullName,
                    phoneNumber: param.PhoneNumber,
                    dayOfBirth: param.DayOfBirth,
                    gender: param.Gender,
                    cinemaId: param.CinemaId,
                    position: param.Position,
                    salary: param.Salary
                );

                return new UpdateUserResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }
            catch (Grpc.Core.RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                return new UpdateUserResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "StaffOrHigher")]
        [HttpPost("staff-booking")]
        public async Task<CreateBookingResultDTO> CreateBooking(CreateStaffBookingRequestParam param)
        {
            try
            {

                //foreach (var claim in User.Claims) { Console.WriteLine($"{claim.Type}: {claim.Value}"); }
                if(param.UserId == null)
                {
                    return new CreateBookingResultDTO
                    {
                        Result = false,
                        Message = "UserId is required",
                        StatusCode = 401
                    };
                }
                var result = await _cinemaServiceConnector.CreateBooking(param.UserId.ToString(), param.ShowtimeId, param.ShowtimeSeatIds, param.FoodDrinkItems);

                return new CreateBookingResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateBookingDataResult
                    {
                        BookingId = Guid.Parse(result.Data.BookingId),
                        CinemaName = result.Data.CinemaName,
                        NumberOfSeats = result.Data.NumberOfSeats,
                        MovieName = result.Data.MovieName,
                        StartTime = DateTime.Parse(result.Data.StartTime),
                        EndTime = DateTime.Parse(result.Data.EndTime),
                        RoomNumber = result.Data.RoomNumber,
                        TotalPrice = decimal.Parse(result.Data.TotalPrice),
                        BookingSeats = result.Data.BookingSeats.Select(bs => new BookingSeatsDataResult
                        {
                            SeatId = Guid.Parse(bs.SeatId),
                            SeatCode = bs.SeatCode,
                            SeatType = bs.SeatType,
                            Label = bs.Label,
                            Price = decimal.Parse(bs.Price),
                        }).ToList(),
                        BookingFoodDrinks = result.Data.BookingFoodDrinks
                        .Select(f => new BookingFoodDrinkDataResult
                        {
                            FoodDrinkId = Guid.Parse(f.FoodDrinkId),
                            Name        = f.Name,
                            Type        = f.Type,
                            Size        = f.Size,
                            Quantity    = f.Quantity,
                            UnitPrice   = decimal.Parse(f.UnitPrice),
                            TotalPrice  = decimal.Parse(f.TotalPrice)
                        }).ToList()
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new CreateBookingResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [Authorize(Policy = "StaffOrHigher")]
        [HttpPost("check-in")]
        public async Task<CheckInBookingResultDTO> CheckInBooking(CheckInBookingRequestParam param)
        {
            try
            {
                var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

                var role = User.FindFirstValue(ClaimTypes.Role);

                if (userId == null)
                {
                    throw new Exception("Not found userId in Token");
                }

                Console.WriteLine("User: " + userId);
                Console.WriteLine("Role: " + role);

                //if (currentUserId == null)
                //{
                //    return new CheckInBookingResultDTO
                //    {
                //        Result = false,
                //        Message = "User not found in token",
                //        StatusCode = 401,
                //        Data = null
                //    };
                //}

                // Không cho customer tự check-in
                if (role == "customer")
                {
                    return new CheckInBookingResultDTO
                    {
                        Result = false,
                        Message = "Customer cannot check-in bookings.",
                        StatusCode = 403,
                        Data = null
                    };
                }

                var grpcResult = await _cinemaServiceConnector.CheckInBooking(param.BookingId);

                return new CheckInBookingResultDTO
                {
                    Result = grpcResult.Result,
                    Message = grpcResult.Message,
                    StatusCode = grpcResult.StatusCode,
                    Data = grpcResult.Data == null
                        ? null
                        : new CheckInBookingDataResult
                        {
                            BookingId = Guid.Parse(grpcResult.Data.BookingId),
                            Status = grpcResult.Data.Status,
                            CinemaId = Guid.Parse(grpcResult.Data.CinemaId),
                            CinemaName = grpcResult.Data.CinemaName,
                            ShowtimeStartTime = DateTime.Parse(grpcResult.Data.ShowtimeStartTime),
                            ShowtimeEndTime = DateTime.Parse(grpcResult.Data.ShowtimeEndTime),
                            NumberOfSeats = grpcResult.Data.NumberOfSeats
                        }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"CheckInBooking Error: {message}");

                return new CheckInBookingResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                    Data = null
                };
            }
        }

        [Authorize(Policy = "StaffOrHigher")]
        [HttpGet("holidays")]
        public async Task<GetHolidaysResultDTO> GetHolidays([FromQuery] GetHolidaysRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetHolidays(param.Name, param.StartDate, param.EndDate);

                return new GetHolidaysResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(ct => new GetHolidaysDataResult
                    {
                        Id = Guid.Parse(ct.Id),
                        Name = ct.Name,
                        Day = ct.Day,
                        Month = ct.Month,
                        ExtraPrice = decimal.Parse(ct.ExtraPrice),
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new GetHolidaysResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPost("holidays")]
        public async Task<CreateHolidayResultDTO> CreateHoliday(CreateHolidayRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.CreateHoliday(param.Name, param.Day, param.Month, param.ExtraPrice);

                return new CreateHolidayResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateHolidayDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        Day = result.Data.Day,
                        Month = result.Data.Month,
                        ExtraPrice = decimal.Parse(result.Data.ExtraPrice),
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new CreateHolidayResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPut("holidays/{id}")]
        public async Task<UpdateHolidayResultDTO> UpdateHoliday(Guid id, [FromBody] UpdateHolidayRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateHoliday(id, param.Name, param.Day, param.Month, param.ExtraPrice);

                return new UpdateHolidayResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateHolidayDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        Day = result.Data.Day,
                        Month = result.Data.Month,
                        ExtraPrice = decimal.Parse(result.Data.ExtraPrice),
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new UpdateHolidayResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [Authorize(Policy = "StaffOrHigher")]
        [HttpGet("customer-types")]
        public async Task<GetCustomerTypesResultDTO> GetCustomerTypes([FromQuery] GetCustomerTypesRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetCustomerTypes(param.Id, param.Name, param.RoleCondition);

                return new GetCustomerTypesResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(ct => new GetCustomerTypesDataResult
                    {
                        Id = Guid.Parse(ct.Id),
                        Name = ct.Name,
                        RoleCondition = ct.RoleCondition,
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new GetCustomerTypesResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPost("customer-types")]
        public async Task<CreateCustomerTypeResultDTO> CreateCustomerTypes(CreateCustomerTypeRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.CreateCustomerType(param.Name, param.RoleCondition);

                return new CreateCustomerTypeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateCustomerTypeDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        RoleCondition = result.Data.RoleCondition,
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new CreateCustomerTypeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPut("customer-types/{id}")]
        public async Task<UpdateCustomerTypeResultDTO> UpdateCustomerType(Guid id, [FromBody] UpdateCustomerTypeRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateCustomerType(id, param.Name, param.RoleCondition);

                return new UpdateCustomerTypeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateCustomerTypeDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        RoleCondition = result.Data.RoleCondition,
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new UpdateCustomerTypeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpGet("promotions")]
        public async Task<GetPromotionsResultDTO> GetPromotions([FromQuery] GetPromotionsRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetPromotions(param.Id, param.Code , param.StartDate, param.EndDate, param.DiscountType, param.IsActive);

                return new GetPromotionsResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(p => new GetPromotionsDataResult
                    {
                        Id = Guid.Parse(p.Id),
                        Code = p.Code,
                        Description = p.Description,
                        DiscountType = p.DiscountType,
                        DiscountValue = decimal.Parse(p.DiscountValue),
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        LimitPerUser = p.LimitPerUser,
                        LimitTotalUse = p.LimitTotalUse,
                        MinOrderValue = p.MinOrderValue != null ? decimal.Parse(p.MinOrderValue) : null,
                        UsedCount = p.UsedCount,
                        IsActive = p.IsActive
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new GetPromotionsResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPost("promotions")]
        public async Task<CreatePromotionResultDTO> CreatePromotion(CreatePromotionRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.CreatePromotion(param.Code, param.Description, param.StartDate, param.EndDate, param.DiscountType, param.DiscountValue, param.LimitPerUser, param.LimitTotalUse, param.MinOrderValue, param.IsActive);

                return new CreatePromotionResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreatePromotionDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Code = result.Data.Code,
                        Description = result.Data.Description,
                        DiscountType = result.Data.DiscountType,
                        DiscountValue = decimal.Parse(result.Data.DiscountValue),
                        StartDate = result.Data.StartDate,
                        EndDate = result.Data.EndDate,
                        LimitPerUser = result.Data.LimitPerUser,
                        LimitTotalUse = result.Data.LimitTotalUse,
                        MinOrderValue = result.Data.MinOrderValue != null ? decimal.Parse(result.Data.MinOrderValue) : null,
                        UsedCount = result.Data.UsedCount,
                        IsActive = result.Data.IsActive
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new CreatePromotionResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPut("promotions/{id}")]
        public async Task<UpdatePromotionResultDTO> UpdatePromotion(Guid id, [FromBody] UpdatePromotionRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdatePromotion(id, param.Code, param.Description, param.StartDate, param.EndDate, param.DiscountType, param.DiscountValue, param.LimitPerUser, param.LimitTotalUse, param.MinOrderValue,param.UsedCount, param.IsActive);

                return new UpdatePromotionResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdatePromotionDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Code = result.Data.Code,
                        Description = result.Data.Description,
                        DiscountType = result.Data.DiscountType,
                        DiscountValue = decimal.Parse(result.Data.DiscountValue),
                        StartDate = result.Data.StartDate,
                        EndDate = result.Data.EndDate,
                        LimitPerUser = result.Data.LimitPerUser,
                        LimitTotalUse = result.Data.LimitTotalUse,
                        MinOrderValue = result.Data.MinOrderValue != null ? decimal.Parse(result.Data.MinOrderValue) : null,
                        UsedCount = result.Data.UsedCount,
                        IsActive = result.Data.IsActive
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new UpdatePromotionResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }
    }
}