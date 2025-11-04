using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.Utils;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
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
        public async Task<GetGenresResultDTO> GetGenres([FromQuery] GetGenresParam query)
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
    }
}
