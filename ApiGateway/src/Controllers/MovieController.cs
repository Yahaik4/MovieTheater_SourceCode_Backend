using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.ServiceConnector.CinemaService;
using src.ServiceConnector.MovieService;

namespace src.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class MovieController : ControllerBase
    {
        private readonly MovieServiceConnector _movieServiceConnector;

        public MovieController(MovieServiceConnector movieServiceConnector)
        {
            _movieServiceConnector = movieServiceConnector;
        }

        [HttpGet("genres")]
        public async Task<GetGenresResultDTO> GetGenres([FromQuery] GetGenresRequestParam query)
        {
            try
            {
                var result = await _movieServiceConnector.GetGenres(query.Id, query.Name);

                return new GetGenresResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(c => new GetGenresDataResult
                    {
                        Id = Guid.Parse(c.Id),
                        Name = c.Name,
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GetGenresResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPost("genre")]
        public async Task<CreateGenreResultDTO> GetGenres(CreateGenreRequestParam param)
        {
            try
            {
                var result = await _movieServiceConnector.CreateGenre(param.Name);

                return new CreateGenreResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateGenreDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreateGenreResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpPut("genre/{id}")]
        public async Task<UpdateGenreResultDTO> UpdateGenre(Guid id, [FromBody] UpdateGenreRequestParam param)
        {
            try
            {
                var result = await _movieServiceConnector.UpdateGenre(id, param.Name);

                return new UpdateGenreResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateGenreDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdateGenreResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("genre/{id}")]
        public async Task<DeleteGenreResultDTO> DeleteGenre(Guid id)
        {
            try
            {
                var result = await _movieServiceConnector.DeleteGenre(id);

                return new DeleteGenreResultDTO
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

                return new DeleteGenreResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }
    }
}
