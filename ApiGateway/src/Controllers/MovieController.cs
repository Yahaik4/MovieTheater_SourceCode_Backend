using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.DataTransferObject.ResultData;
using ApiGateway.ServiceConnector.MovieService;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieGrpc;
using Serilog;
using Shared.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiGateway.Controllers
{
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

        [Authorize(Policy = "OperationsManagerOnly")]
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

        [Authorize(Policy = "OperationsManagerOnly")]
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

        [Authorize(Policy = "OperationsManagerOnly")]
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
        
        [HttpGet("persons")]
        public async Task<GetPersonsResultDTO> GetPersons([FromQuery] GetPersonsRequestParam query)
        {
            try
            {
                var result = await _movieServiceConnector.GetPersons(query.Id, query.Name);

                return new GetPersonsResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(c => new GetPersonsDataResult
                    {
                        Id = Guid.Parse(c.Id),
                        FullName = c.FullName,
                        Gender = c.Gender,
                        BirthDate = string.IsNullOrEmpty(c.BirthDate) ? null : DateOnly.Parse(c.BirthDate),
                        Nationality = c.Nationality,
                        Bio = c.Bio,
                        ImageUrl = c.ImageUrl,
                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GetPersonsResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPost("person")]
        public async Task<CreatePersonResultDTO> CreatePerson(CreatePersonRequestParam param)
        {
            try
            {
                var result = await _movieServiceConnector.CreatePerson(param.FullName, param.Gender, param.BirthDate, param.Nationality, param.Bio, param.ImageUrl);

                return new CreatePersonResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreatePersonDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        FullName = result.Data.FullName,
                        Gender = result.Data.Gender,
                        BirthDate = string.IsNullOrEmpty(result.Data.BirthDate) ? null : DateOnly.Parse(result.Data.BirthDate),
                        Nationality = result.Data.Nationality,
                        Bio = result.Data.Bio,
                        ImageUrl = result.Data.ImageUrl,
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreatePersonResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPut("person/{id}")]
        public async Task<UpdatePersonResultDTO> UpdatePerson(Guid id, [FromBody] UpdatePersonRequestParam param)
        {
            try
            {
                var result = await _movieServiceConnector.UpdatePerson(id, param.FullName, param.Gender, param.BirthDate, param.Nationality, param.Bio, param.ImageUrl);

                return new UpdatePersonResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdatePersonDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        FullName = result.Data.FullName,
                        Gender = result.Data.Gender,
                        BirthDate = string.IsNullOrEmpty(result.Data.BirthDate) ? null : DateOnly.Parse(result.Data.BirthDate),
                        Nationality = result.Data.Nationality,
                        Bio = result.Data.Bio,
                        ImageUrl = result.Data.ImageUrl,
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdatePersonResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }
        
        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpDelete("person/{id}")]
        public async Task<DeletePersonResultDTO> DeletePerson(Guid id)
        {
            try
            {
                var result = await _movieServiceConnector.DeletePerson(id);

                return new DeletePersonResultDTO
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

                return new DeletePersonResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [HttpGet("movies")]
        public async Task<GetMoviesResultDTO> GetMovies([FromQuery] GetMoviesRequestParam query)
        {
            try
            {
                var result = await _movieServiceConnector.GetMovies(query.Id, query.Name, query.Country, query.Status);

                return new GetMoviesResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = result.Data.Select(m => new GetMoviesDataResult
                    {
                        Id = Guid.Parse(m.Id),
                        Name = m.Name,
                        Country = m.Country,
                        Description = m.Description,
                        Status = m.Status,
                        Duration = TimeSpan.Parse(m.Duration),
                        Language = m.Language,
                        Poster = m.Poster,
                        Publisher = m.Publisher,
                        ReleaseDate = DateOnly.Parse(m.ReleaseDate),
                        TrailerUrl = m.TrailerUrl,
                        Genres = m.Genres.Select(mg => new MovieGenreDataResult
                        {
                            GenreId = Guid.Parse(mg.GenreId),
                            GenreName = mg.GenreName,
                        }).ToList(),
                        Persons = m.Persons.Select(mp => new MoviePersonDataResult
                        {
                            PersonId = Guid.Parse(mp.PersonId),
                            FullName = mp.FullName,
                            Role = mp.Role,
                        }).ToList()

                    }).ToList()
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new GetMoviesResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        // [Authorize(Policy = "OperationsManagerOnly")]
        // [HttpPost("movie")]
        // public async Task<CreateMovieResultDTO> CreateMovie(CreateMovieRequestParam param)
        // {
        //     try
        //     {
        //         var result = await _movieServiceConnector.CreateMovie(param);

        //         return new CreateMovieResultDTO
        //         {
        //             Result = result.Result,
        //             Message = result.Message,
        //             StatusCode = result.StatusCode,
        //             Data = new CreateMovieDataResult
        //             {
        //                 Id = Guid.Parse(result.Data.Id),
        //                 Name = result.Data.Name,
        //                 Country = result.Data.Country,
        //                 Description = result.Data.Description,
        //                 Status = result.Data.Status,
        //                 Duration = TimeSpan.Parse(result.Data.Duration),
        //                 Language = result.Data.Language,
        //                 Poster = result.Data.Poster,
        //                 Publisher = result.Data.Publisher,
        //                 ReleaseDate = DateOnly.Parse(result.Data.ReleaseDate),
        //                 TrailerUrl  = result.Data.TrailerUrl,
        //                 Genres = result.Data.Genres.Select(mg => new MovieGenreDataResult
        //                 {
        //                     GenreId = Guid.Parse(mg.GenreId),
        //                     GenreName = mg.GenreName,
        //                 }).ToList(),
        //                 Persons = result.Data.Persons.Select(mp => new MoviePersonDataResult
        //                 {
        //                     PersonId = Guid.Parse(mp.PersonId),
        //                     FullName = mp.FullName,
        //                     Role = mp.Role,
        //                 }).ToList()
        //             }
        //         };
        //     }
        //     catch (RpcException ex)
        //     {
        //         var (statusCode, message) = RpcExceptionParser.Parse(ex);
        //         Log.Error($"Login Error: {message}");

        //         return new CreateMovieResultDTO
        //         {
        //             Result = false,
        //             Message = message,
        //             StatusCode = (int)statusCode
        //         };
        //     }
        // }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPost("movie")]
        public async Task<CreateMovieResultDTO> CreateMovie([FromForm] CreateMovieFormParam form)
        {
            try
            {
                string posterBase64 = null;

                if (form.PosterFile != null && form.PosterFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await form.PosterFile.CopyToAsync(ms);
                    var bytes = ms.ToArray();
                    posterBase64 = Convert.ToBase64String(bytes);
                }

                var param = new CreateMovieRequestParam
                {
                    Name = form.Name,
                    Description = form.Description,
                    ReleaseDate = form.ReleaseDate,
                    Duration = form.Duration,
                    Publisher = form.Publisher,
                    Country = form.Country,
                    Language = form.Language,
                    Poster = posterBase64,
                    TrailerUrl = form.TrailerUrl,
                    Status = form.Status,
                    Genres = form.Genres,
                    Persons = form.Persons
                };

                var result = await _movieServiceConnector.CreateMovie(param);

                return new CreateMovieResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new CreateMovieDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        Country = result.Data.Country,
                        Description = result.Data.Description,
                        Status = result.Data.Status,
                        Duration = TimeSpan.Parse(result.Data.Duration),
                        Language = result.Data.Language,
                        Poster = result.Data.Poster,
                        Publisher = result.Data.Publisher,
                        ReleaseDate = DateOnly.Parse(result.Data.ReleaseDate),
                        TrailerUrl = result.Data.TrailerUrl,
                        Genres = result.Data.Genres.Select(mg => new MovieGenreDataResult
                        {
                            GenreId = Guid.Parse(mg.GenreId),
                            GenreName = mg.GenreName,
                        }).ToList(),
                        Persons = result.Data.Persons.Select(mp => new MoviePersonDataResult
                        {
                            PersonId = Guid.Parse(mp.PersonId),
                            FullName = mp.FullName,
                            Role = mp.Role,
                        }).ToList()
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new CreateMovieResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpPatch("movie/{id}")]
        public async Task<UpdateMovieResultDTO> UpdateMovie(Guid id, UpdateMovieRequestParam param)
        {
            try
            {
                var result = await _movieServiceConnector.UpdateMovie(id, param);

                return new UpdateMovieResultDTO
                {
                    Result = result.Result,
                    Message = result.Message,
                    StatusCode = result.StatusCode,
                    Data = new UpdateMovieDataResult
                    {
                        Id = Guid.Parse(result.Data.Id),
                        Name = result.Data.Name,
                        Country = result.Data.Country,
                        Description = result.Data.Description,
                        Status = result.Data.Status,
                        Duration = TimeSpan.Parse(result.Data.Duration),
                        Language = result.Data.Language,
                        Poster = result.Data.Poster,
                        Publisher = result.Data.Publisher,
                        ReleaseDate = DateOnly.Parse(result.Data.ReleaseDate),
                        TrailerUrl = result.Data.TrailerUrl,
                        Genres = result.Data.Genres.Select(mg => new MovieGenreDataResult
                        {
                            GenreId = Guid.Parse(mg.GenreId),
                            GenreName = mg.GenreName,
                        }).ToList(),
                        Persons = result.Data.Persons.Select(mp => new MoviePersonDataResult
                        {
                            PersonId = Guid.Parse(mp.PersonId),
                            FullName = mp.FullName,
                            Role = mp.Role,
                        }).ToList()
                    }
                };
            }
            catch (RpcException ex)
            {
                var (statusCode, message) = RpcExceptionParser.Parse(ex);
                Log.Error($"Login Error: {message}");

                return new UpdateMovieResultDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }

        [Authorize(Policy = "OperationsManagerOnly")]
        [HttpDelete("movie/{id}")]
        public async Task<DeleteMovieGrpcReplyDTO> DeleteMovie(Guid id)
        {
            try
            {
                var result = await _movieServiceConnector.DeleteMovie(id);

                return new DeleteMovieGrpcReplyDTO
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

                return new DeleteMovieGrpcReplyDTO
                {
                    Result = false,
                    Message = message,
                    StatusCode = (int)statusCode
                };
            }
        }
    }
}
