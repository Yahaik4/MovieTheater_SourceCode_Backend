using MovieService.DataTransferObject.Parameter;
using MovieService.DataTransferObject.ResultData;
using MovieService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace MovieService.DomainLogic
{
    public class DeleteMovieLogic : IDomainLogic<DeleteMovieParam, Task<DeleteMovieResultData>>
    {
        private readonly IMovieRepository _movieRepository;

        public DeleteMovieLogic(IMovieRepository movieRepository)
        {            
            _movieRepository = movieRepository;
        }

        public async Task<DeleteMovieResultData> Execute(DeleteMovieParam param)
        {
            var movie = await _movieRepository.GetMovieById(param.Id);

            if (movie == null) { 
                throw new NotFoundException("Movie not found");
            }

            movie.IsDeleted = true;
            await _movieRepository.UpdateMovie(movie);

            return new DeleteMovieResultData
            {
                Result = true,
                Message = "Delete Movie Successfully",
                StatusCode = StatusCodeEnum.Success
            };
        }
    }
}
