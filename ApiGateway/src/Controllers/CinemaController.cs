using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.DataTransferObject.ResultData;
using ApiGateway.Helper;
using ApiGateway.ServiceConnector.CinemaService;
using ApiGateway.ServiceConnector.MovieService;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api")]
    public class CinemaController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly CinemaServiceConnector _cinemaServiceConnector;
        private readonly MovieServiceConnector _movieServiceConnector;

        public CinemaController(CinemaServiceConnector cinemaServiceConnector, ICurrentUserService currentUserService, MovieServiceConnector movieServiceConnector)
        {
            _cinemaServiceConnector = cinemaServiceConnector;
            _currentUserService = currentUserService;
            _movieServiceConnector = movieServiceConnector;
        }

        [HttpGet("cinemas")]
        public async Task<GetAllCinemasResultDTO> GetAllCinema([FromQuery] GetAllCinemaRequestParam query)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetAllCinemas(query.Id, query.Name, query.City, query.Status);

                return new GetAllCinemasResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(c => new GetAllCinemasDataResult
                    {
                        Id = Guid.Parse(c.Id),
                        Name = c.Name,
                        Address = c.Address,
                        City = c.City,
                        Email = c.Email,
                        PhoneNumber = c.PhoneNumber,
                        Open_Time = c.OpenTime,
                        Close_Time = c.CloseTime,
                        TotalRoom = c.TotalRoom,
                        Status = c.Status,
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GetAllCinemasResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPost("cinema")]
        public async Task<CreateCinemaResultDTO> CreateCinema(CreateCinemaRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.CreateCinema(param.Name, param.Address, param.City, param.PhoneNumber, param.Email, param.Open_Time, param.Close_Time, param.Status);

                return new CreateCinemaResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateCinemaDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        Address = result.Data.Address,
                        City = result.Data.City,
                        PhoneNumber = result.Data.PhoneNumber,
                        Email = result.Data.Email,
                        Open_Time = TimeOnly.Parse(result.Data.OpenTime),
                        Close_Time = TimeOnly.Parse(result.Data.CloseTime),
                        Status = result.Data.Status,
                        CreateBy = result.Data.CreatedBy
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreateCinemaResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPut("cinema/{id}")]
        public async Task<UpdateCinemaResultDTO> UpdateCinema(Guid id, [FromBody] UpdateCinemaRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateCinema(id, param.Name, param.Address, param.City, param.PhoneNumber, param.Email, param.Open_Time, param.Close_Time, param.Status);

                return new UpdateCinemaResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateCinemaDataResult
                    {
                        Name = result.Data.Name,
                        Address = result.Data.Address,
                        City = result.Data.City,
                        PhoneNumber = result.Data.PhoneNumber,
                        Email = result.Data.Email,
                        Open_Time = TimeOnly.Parse(result.Data.OpenTime),
                        Close_Time = TimeOnly.Parse(result.Data.CloseTime),
                        Status = result.Data.Status
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdateCinemaResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpDelete("cinema/{id}")]
        public async Task<DeleteCinemaResultDTO> DeleteCinema(Guid id)
        {
            try
            {
                var result = await _cinemaServiceConnector.DeleteCinema(id);
                return new DeleteCinemaResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new DeleteCinemaResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("room-types")]
        public async Task<GetAllRoomTypesResultDTO> GetAllRoomType([FromQuery] GetAllRoomTypeRequestParam query)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetAllRoomTypes(query.Id, query.Type, query.BasePrice);

                return new GetAllRoomTypesResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(rt => new GetAllRoomTypesDataResult
                    {
                        Id = Guid.Parse(rt.Id),
                        Type = rt.Type,
                        BasePrice = decimal.Parse(rt.BasePrice)
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GetAllRoomTypesResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPost("room-type")]
        public async Task<CreateRoomTypeResultDTO> CreateRoomType(CreateRoomTypeRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.CreateRoomType(param.Type, param.BasePrice);

                return new CreateRoomTypeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateRoomTypeDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Type = result.Data.Type,
                        BasePrice = decimal.Parse(result.Data.BasePrice),
                        CreatedBy = result.Data.CreatedBy
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreateRoomTypeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPut("room-type/{id}")]
        public async Task<UpdateRoomTypeResultDTO> UpdateRoomType(Guid id, [FromBody] UpdateRoomTypeRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateRoomType(id, param.Type, param.BasePrice);

                return new UpdateRoomTypeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateRoomTypeDataResult
                    {
                        Type = result.Data.Type,
                        BasePrice = decimal.Parse(result.Data.BasePrice),
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdateRoomTypeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpDelete("room-type/{id}")]
        public async Task<DeleteRoomTypeResultDTO> DeleteRoomType(Guid id)
        {
            try
            {
                var result = await _cinemaServiceConnector.DeleteRoomType(id);
                return new DeleteRoomTypeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new DeleteRoomTypeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("seat-types")]
        public async Task<GetAllSeatTypesResultDTO> GetAllSeatType([FromQuery] GetAllSeatTypeRequestParam query)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetAllSeatTypes(query.Id, query.Type, query.ExtraPrice);

                return new GetAllSeatTypesResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(st => new GetAllSeatTypesDataResult
                    {
                        Id = Guid.Parse(st.Id),
                        Type = st.Type,
                        ExtraPrice = decimal.Parse(st.ExtraPrice)
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GetAllSeatTypesResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPost("seat-type")]
        public async Task<CreateSeatTypeResultDTO> CreateSeatType(CreateSeatTypeRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.CreateSeatType(param.Type, param.ExtraPrice);

                return new CreateSeatTypeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateSeatTypeDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Type = result.Data.Type,
                        ExtraPrice = decimal.Parse(result.Data.ExtraPrice),
                        CreatedBy = result.Data.CreatedBy
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreateSeatTypeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPut("seat-type/{id}")]
        public async Task<UpdateSeatTypeResultDTO> UpdateSeatType(Guid id, [FromBody] UpdateSeatTypeRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateSeatType(id, param.Type, param.ExtraPrice);

                return new UpdateSeatTypeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateSeatTypeDataResult
                    {
                        Type = result.Data.Type,
                        ExtraPrice = decimal.Parse(result.Data.ExtraPrice),
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdateSeatTypeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpDelete("seat-type/{id}")]
        public async Task<DeleteSeatTypeResultDTO> DeleteSeatType(Guid id)
        {
            try
            {
                var result = await _cinemaServiceConnector.DeleteSeatType(id);
                return new DeleteSeatTypeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new DeleteSeatTypeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("rooms/{cinemaId}")]
        public async Task<GetAllRoomResultDTO> GetAllRoom(Guid cinemaId, [FromQuery] GetAllRoomRequestParam query)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetAllRooms(cinemaId, query.Id, query.RoomNumber, query.Status,  query.Type);

                return new GetAllRoomResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(r => new GetAllRoomsDataResult
                    {
                        Id = Guid.Parse(r.Id),
                        RoomNumber = r.RoomNumber,
                        Status = r.Status,
                        TotalColumn = r.TotalColumn,
                        TotalRow = r.TotalRow,
                        RoomTypeId = Guid.Parse(r.RoomTypeId),
                        RoomType = r.RoomType,
                        CreatedBy = r.CreatedBy,
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GetAllRoomResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "CinemaManagerOrHigher")]
        [HttpPost("room")]
        public async Task<CreateRoomResultDTO> CreateRoom(CreateRoomRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.CreateRoom(param.RoomNumber, param.TotalColumn, param.TotalRow, param.Status, param.RoomTypeId, param.CinemaId);

                return new CreateRoomResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateRoomDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        RoomNumber = result.Data.RoomNumber,
                        TotalColumn = result.Data.TotalColumn,
                        TotalRow = result.Data.TotalRow,
                        Status = result.Data.Status,
                        RoomType = result.Data.RoomType,
                        Cinema = result.Data.Cinema,
                        CreatedBy = result.Data.CreatedBy,
                        //Seats = result.Data.Seats.Select(s => new CreateSeatDataResult
                        //{
                        //    Id = Guid.Parse(s.Id),
                        //    Label = s.Label,
                        //    ColumnIndex = s.ColumnIndex,
                        //    DisplayNumber = s.DisplayNumber,
                        //    SeatCode = s.SeatCode,
                        //    isActive = s.IsActive,
                        //    Status = s.Status,
                        //    SeatType = s.SeatType,
                        //}).ToList()
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreateRoomResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "CinemaManagerOrHigher")]
        [HttpPut("room/{id}")]
        public async Task<UpdateRoomResultDTO> UpdateRoom(Guid id, [FromBody] UpdateRoomRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateRoom(id, param.RoomNumber, param.Status,param.TotalColumn,param.TotalRow, param.RoomTypeId, param.CinemaId);

                return new UpdateRoomResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateRoomDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        RoomNumber = result.Data.RoomNumber,
                        TotalColumn = result.Data.TotalColumn,
                        TotalRow = result.Data.TotalRow,
                        RoomType = result.Data.RoomType,
                        Status = result.Data.Status,
                        Cinema = result.Data.Cinema
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdateRoomResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "CinemaManagerOrHigher")]
        [HttpDelete("room/{id}")]
        public async Task<DeleteRoomResultDTO> DeleteRoom(Guid id)
        {
            try
            {
                var result = await _cinemaServiceConnector.DeleteRoom(id);
                return new DeleteRoomResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new DeleteRoomResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("seats/{roomId}")]
        public async Task<GetAllSeatsResultDTO> GetAllSeat(Guid roomId)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetAllSeats(roomId);

                return new GetAllSeatsResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(r => new GetAllSeatsDataResult
                    {
                        Id = Guid.Parse(r.Id),
                        Label = r.Label,
                        ColumnIndex = r.ColumnIndex,
                        DisplayNumber = r.DisplayNumber,
                        SeatCode = r.SeatCode,
                        isActive = r.IsActive,
                        Status = r.Status,
                        SeatType = r.SeatType,
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GetAllSeatsResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "CinemaManagerOrHigher")]
        [HttpPatch("seats")]
        public async Task<UpdateSeatsResultDTO> UpdateSeats(UpdateSeatsRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateSeats(param.Ids, param.IsActive, param.SeatTypeId);

                return new UpdateSeatsResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(s => new UpdateSeatsDataResult
                    {
                        Label = s.Label,
                        ColumnIndex = s.ColumnIndex,
                        DisplayNumber = s.DisplayNumber,
                        SeatCode = s.SeatCode,
                        isActive = s.IsActive,
                        Status = s.Status,
                        SeatType = s.SeatType,
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdateSeatsResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("showtimes/by-cinema/{cinemaId}")]
        public async Task<GetShowtimesByCinemaResultDTO> GetShowtimesByCinema(Guid cinemaId, [FromQuery] GetShowtimesByCinemaRequestParam param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new GetShowtimesByCinemaResultDTO
                    {
                        Result = false,
                        Message = "Date is required",
                        StatusCode = 400,
                        Data = new List<GetShowtimesByCinemaDataResult>()
                    };
                }

                var showtimeReply = await _cinemaServiceConnector.GetShowtimesByCinema(cinemaId, param.Date);

                return new GetShowtimesByCinemaResultDTO
                {
                    Result = showtimeReply.Result,
                    Message = showtimeReply.Message,
                    StatusCode = showtimeReply.StatusCode,
                    Data = showtimeReply.Data.Select(c => new GetShowtimesByCinemaDataResult
                    {
                        MovieId = Guid.Parse(c.MovieId),
                        MovieName = c.MovieName,
                        MovieDescription = c.MovieDescription,
                        Poster = $"{Request.Scheme}://{Request.Host}/api/movies/{Guid.Parse(c.MovieId)}/poster",
                        RoomTypes = c.RoomTypes
                                .Select(rt => new GetRoomTypeDataResult
                                {
                                    RoomTypeId = Guid.Parse(rt.RoomTypeId),
                                    RoomTypeName = rt.RoomTypeName,
                                    Showtimes = rt.Showtimes
                                        .Select(st => new ShowtimeDataResult
                                        {
                                            ShowtimeId = Guid.Parse(st.ShowtimeId),
                                            StartTime = st.StartTime,
                                            EndTime = st.EndTime
                                        })
                                        .OrderBy(st => st.StartTime)
                                        .ToList()
                                })
                                .ToList()
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetAllShowtimes Error: {message}");

                return new GetShowtimesByCinemaResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                    Data = new List<GetShowtimesByCinemaDataResult>()
                };
            }
        }

        [HttpGet("showtimes/by-movie/{movieId}")]
        public async Task<GetShowtimesResultDTO> GetShowtimes(Guid movieId, [FromQuery] GetShowtimesRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetShowtimes(null, movieId, param.Date, param.City);

                return  new GetShowtimesResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data
                        .Select(c => new GetShowtimesDataResult
                        {
                            CinemaId = Guid.Parse(c.CinemaId),
                            CinemaName = c.CinemaName,
                            Address = c.Address,
                            RoomTypes = c.RoomTypes
                                .Select(rt => new GetRoomTypeDataResult
                                {
                                    RoomTypeId = Guid.Parse(rt.RoomTypeId),
                                    RoomTypeName = rt.RoomTypeName,
                                    Showtimes = rt.Showtimes
                                        .Select(st => new ShowtimeDataResult
                                        {
                                            ShowtimeId = Guid.Parse(st.ShowtimeId),
                                            StartTime = st.StartTime,
                                            EndTime = st.EndTime
                                        })
                                        .OrderBy(st => st.StartTime)
                                        .ToList()
                                })
                                .ToList()
                        })
                        .ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new GetShowtimesResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                    Data = new List<GetShowtimesDataResult>()
                };
            }
        }

        [HttpGet("showtime-details/{showtimeId}")]
        public async Task<GetShowtimeDetailsResultDTO> GetShowtimeDetails(Guid showtimeId)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetShowtimeDetails(showtimeId);

                return new GetShowtimeDetailsResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new GetShowtimeDetailsDataResult
                    {
                        CinemaId = Guid.Parse(result.Data.CinemaId),
                        CinemaName = result.Data.CinemaName,
                        City = result.Data.City,
                        RoomId = Guid.Parse(result.Data.RoomId),
                        RoomNumber = result.Data.RoomNumber,
                        TotalColumn = result.Data.TotalColumn,
                        TotalRow = result.Data.TotalRow,
                        RoomType = result.Data.RoomType,
                        StartTime = DateTime.Parse(result.Data.StartTime),
                        EndTime = DateTime.Parse(result.Data.EndTime),
                        MovieId = Guid.Parse(result.Data.MovieId),
                        MovieName = result.Data.MovieName,
                        Poster = $"{Request.Scheme}://{Request.Host}/api/movies/{Guid.Parse(result.Data.MovieId)}/poster"
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new GetShowtimeDetailsResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "CinemaManagerOrHigher")]
        [HttpPost("showtime")]
        public async Task<CreateShowtimeResultDTO> CreateShowtime(CreateShowtimeRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.CreateShowtime(param.MovieId, param.RoomId, param.StartTime,param.EndTime,  param.Status);

                return new CreateShowtimeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateShowtimeDataResult
                    {
                        ShowtimeId = Guid.Parse(result.Data.ShowtimeId),
                        MovieId = Guid.Parse(result.Data.MovieId),
                        RoomId = Guid.Parse(result.Data.RoomId),
                        MovieName = result.Data.MovieName,
                        RoomNumber = result.Data.RoomNumber,
                        StartTime = result.Data.StartTime,
                        EndTime = result.Data.EndTime,
                        Status = result.Data.Status
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreateShowtimeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "CinemaManagerOrHigher")]
        [HttpPut("showtime/{id}")]
        public async Task<UpdateShowtimeResultDTO> UpdateShowtime(Guid id, [FromBody] UpdateShowtimeRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateShowtime(id, param);

                return new UpdateShowtimeResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateShowtimeDataResult
                    {
                        ShowtimeId = Guid.Parse(result.Data.ShowtimeId),
                        MovieId = Guid.Parse(result.Data.MovieId),
                        RoomId = Guid.Parse(result.Data.RoomId),
                        StartTime = result.Data.StartTime,
                        EndTime = result.Data.EndTime,
                        MovieName = result.Data.MovieName,
                        RoomNumber = result.Data.RoomNumber,
                        Status = result.Data.Status
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdateShowtimeResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("showtime-seats/{showtimeId}")]
        public async Task<GetShowtimeSeatsResultDTO> GetShowtimeseats(Guid showtimeId)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetShowtimeSeats(showtimeId);

                return new GetShowtimeSeatsResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(sts => new GetShowtimeSeatsDataResult
                    {
                        ShowtimeSeatId = Guid.Parse(sts.ShowtimeSeatId),
                        RoomNumber = sts.RoomNumber,
                        SeatCode = sts.SeatCode,
                        Label = sts.Label,
                        Status = sts.Status,
                        Price = decimal.Parse(sts.Price),
                        SeatType = sts.SeatType
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new GetShowtimeSeatsResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [HttpGet("food-drinks/{id}/image")]
        public async Task<IActionResult> GetFoodDrinkImage(Guid id)
        {
            var result = await _cinemaServiceConnector.GetAllFoodDrinks(id, null, null, null);

            if (result == null || result.Data == null || !result.Data.Any())
                return NotFound();

            var foodDrink = result.Data.FirstOrDefault();

            if (foodDrink == null || string.IsNullOrEmpty(foodDrink.Image))
                return NotFound();

            var bytes = Convert.FromBase64String(foodDrink.Image);

            return File(bytes, "image/jpeg");
        }

        private static string GuessImageContentType(byte[] bytes)
        {
            // JPEG
            if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
                return "image/jpeg";
            // PNG
            if (bytes.Length >= 8 &&
                bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                return "image/png";
            // GIF
            if (bytes.Length >= 6 &&
                bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
                return "image/gif";
            // WEBP: "RIFF....WEBP"
            if (bytes.Length >= 12 &&
                bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46 &&
                bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50)
                return "image/webp";

            return "application/octet-stream";
        }

        [HttpGet("food-drinks")]
        public async Task<GetAllFoodDrinkResultDTO> GetAllFoodDrinks([FromQuery] GetAllFoodDrinkRequestParam query)
        {
            try
            {
                var result = await _cinemaServiceConnector.GetAllFoodDrinks(
                    query.Id, query.Name, query.Type, query.Size);

                return new GetAllFoodDrinkResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(fd => new GetAllFoodDrinkDataResult
                    {
                        Id = Guid.Parse(fd.Id),
                        Name = fd.Name,
                        Type = fd.Type,
                        Size = fd.Size,
                        Price = decimal.Parse(fd.Price),
                        Image = string.IsNullOrWhiteSpace(fd.Image)
                        ? null
                        : $"{Request.Scheme}://{Request.Host}/api/food-drinks/{Guid.Parse(fd.Id)}/image",
                        Description = string.IsNullOrWhiteSpace(fd.Description) ? null : fd.Description,
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetAllFoodDrinks Error: {message}");

                return new GetAllFoodDrinkResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                    Data = new List<GetAllFoodDrinkDataResult>()
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPost("food-drink")]
        [Consumes("multipart/form-data")]
        public async Task<CreateFoodDrinkResultDTO> CreateFoodDrink([FromForm] CreateFoodDrinkFormRequestParam param)
        {
            try
            {
                string? imageBase64 = null;

                if (param.ImageFile != null && param.ImageFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await param.ImageFile.CopyToAsync(ms);
                    imageBase64 = Convert.ToBase64String(ms.ToArray());
                }

                var result = await _cinemaServiceConnector.CreateFoodDrink(new CreateFoodDrinkRequestParam
                {
                    Name = param.Name,
                    Type = param.Type,
                    Size = param.Size,
                    Price = param.Price,
                    Image = imageBase64,
                    Description = param.Description
                });

                return new CreateFoodDrinkResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateFoodDrinkDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        Type = result.Data.Type,
                        Size = result.Data.Size,
                        Price = decimal.Parse(result.Data.Price),
                        Image = $"{Request.Scheme}://{Request.Host}/api/food-drinks/{Guid.Parse(result.Data.Id)}/image",
                        Description = string.IsNullOrWhiteSpace(result.Data.Description) ? null : result.Data.Description,
                        CreatedBy = string.IsNullOrWhiteSpace(result.Data.CreatedBy) ? null : result.Data.CreatedBy
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"CreateFoodDrink Error: {message}");

                return new CreateFoodDrinkResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                    Data = new CreateFoodDrinkDataResult()
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPut("food-drink/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<UpdateFoodDrinkResultDTO> UpdateFoodDrink(
            Guid id,
            [FromForm] UpdateFoodDrinkFormRequestParam param)
        {
            try
            {
                string? imageBase64 = null;

                if (param.ImageFile != null && param.ImageFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await param.ImageFile.CopyToAsync(ms);
                    imageBase64 = Convert.ToBase64String(ms.ToArray());
                }

                var result = await _cinemaServiceConnector.UpdateFoodDrink(
                    id,
                    new UpdateFoodDrinkRequestParam
                    {
                        Name = param.Name,
                        Type = param.Type,
                        Size = param.Size,
                        Price = param.Price,
                        Image = imageBase64, // null => không update ảnh
                        Description = param.Description
                    });

                return new UpdateFoodDrinkResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateFoodDrinkDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        Type = result.Data.Type,
                        Size = result.Data.Size,
                        Price = decimal.Parse(result.Data.Price),
                        Image = string.IsNullOrWhiteSpace(result.Data.Image) ? null : result.Data.Image,
                        Description = string.IsNullOrWhiteSpace(result.Data.Description)
                            ? null
                            : result.Data.Description
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"UpdateFoodDrink Error: {message}");

                return new UpdateFoodDrinkResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                    Data = new UpdateFoodDrinkDataResult()
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpDelete("food-drink/{id}")]
        public async Task<DeleteFoodDrinkResultDTO> DeleteFoodDrink(Guid id)
        {
            try
            {
                var result = await _cinemaServiceConnector.DeleteFoodDrink(id);

                return new DeleteFoodDrinkResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"DeleteFoodDrink Error: {message}");

                return new DeleteFoodDrinkResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize]
        [HttpPost("booking")]
        public async Task<CreateBookingResultDTO> CreateBooking(CreateUserBookingRequestParam param)
        {
            try
            {
                var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null) {
                    throw new Exception("Not found userId in Token");
                }
                
                var result = await _cinemaServiceConnector.CreateBooking(userId, param.ShowtimeId, param.PromotionId, param.ShowtimeSeatIds,  param.FoodDrinkItems);

                return new CreateBookingResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateBookingDataResult
                    {
                        BookingId = Guid.Parse(result.Data.BookingId),
                        PromotionId = string.IsNullOrWhiteSpace(result.Data.PromotionId) ? null : Guid.Parse(result.Data.PromotionId),
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

        [Authorize]
        [HttpPut("booking-status/{bookingId}")]
        public async Task<UpdateBookingStatusResultDTO> CreateBooking(Guid bookingId, [FromBody] UpdateBookingStatusRequestParam param)
        {
            try
            {
                var result = await _cinemaServiceConnector.UpdateBookingStatus(bookingId, param.Status);

                return new UpdateBookingStatusResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateBookingStatusDataResult
                    {
                        BookingId = Guid.Parse(result.Data.BookingId),
                        Status = result.Data.Status,
                        BookingSeats = result.Data.BookingSeats.Select(bt => new UpdateBookingSeatsDataResult
                        {
                            SeatId = Guid.Parse(bt.SeatId),
                            SeatCode = bt.SeatCode,
                            SeatType = bt.SeatType,
                            Label = bt.Label,
                            Price = decimal.Parse(bt.Price),
                            Status = bt.Status
                        }).ToList()
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetShowtimes Error: {message}");

                return new UpdateBookingStatusResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode,
                };
            }
        }

        [HttpGet("booking-history")]
        public async Task<GetBookingHistoryResultDTO> GetBookingHistory()
        {
            try
            {
                var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                     ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    throw new Exception("Not found userId in Token");
                }
                
                var result = await _cinemaServiceConnector.GetBookingHistory(userId);

                return new GetBookingHistoryResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(b => new CreateBookingDataResult
                    {
                        BookingId = Guid.Parse(b.BookingId),
                        PromotionId = string.IsNullOrWhiteSpace(b.PromotionId) ? null : Guid.Parse(b.PromotionId),
                        CinemaName = b.CinemaName,
                        MovieName = b.MovieName,
                        RoomNumber = b.RoomNumber,
                        StartTime = DateTime.Parse(b.StartTime),
                        EndTime = DateTime.Parse(b.EndTime),
                        NumberOfSeats = b.NumberOfSeats,
                        TotalPrice = Decimal.Parse(b.TotalPrice),

                        BookingSeats = b.BookingSeats.Select(s => new BookingSeatsDataResult
                        {
                            SeatId = Guid.Parse(s.SeatId),
                            SeatCode = s.SeatCode,
                            SeatType = s.SeatType,
                            Label = s.Label,
                            Price = Decimal.Parse(s.Price)
                        }).ToList(),

                        BookingFoodDrinks = b.BookingFoodDrinks?.Select(f => new BookingFoodDrinkDataResult
                        {
                            FoodDrinkId = Guid.Parse(f.FoodDrinkId),
                            Name = f.Name,
                            Type = f.Type,
                            Size = f.Size,
                            Quantity = f.Quantity,
                            UnitPrice = Decimal.Parse(f.UnitPrice),
                            TotalPrice = Decimal.Parse(f.TotalPrice)
                        }).ToList()

                    }).ToList()
                };

            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"GetAllFoodDrinks Error: {message}");

                return new GetBookingHistoryResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("search-promotion/{code}")]
        public async Task<SearchPromotionResultDTO> SearchPromotion(string code)
        {
            try
            {
                var result = await _cinemaServiceConnector.SearchPromotion(code);

                return new SearchPromotionResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new GetPromotionsDataResult
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
                Log.Error($"GetAllFoodDrinks Error: {message}");

                return new SearchPromotionResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }
    }
}
