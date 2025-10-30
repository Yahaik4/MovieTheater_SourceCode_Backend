﻿using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.ServiceConnector.CinemaService;

namespace src.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class CinemaController : ControllerBase
    {
        private readonly CinemaServiceConnector _cinemaServiceConnector;

        public CinemaController(CinemaServiceConnector cinemaServiceConnector)
        {
            _cinemaServiceConnector = cinemaServiceConnector;
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
                        BasePrice = Decimal.Parse(rt.BasePrice)
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

        [Authorize(Roles = "admin")]
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
                        BasePrice = Decimal.Parse(result.Data.BasePrice),
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

        [Authorize(Roles = "admin")]
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
                        ExtraPrice = Decimal.Parse(st.ExtraPrice)
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

        [Authorize(Roles = "admin")]
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
                        ExtraPrice = Decimal.Parse(result.Data.ExtraPrice),
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

        [Authorize(Roles = "admin")]
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
    }
}
