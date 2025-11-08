using MovieService.DataTransferObject.Parameter;
using MovieService.DataTransferObject.ResultData;
using MovieService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace MovieService.DomainLogic
{
    public class DeleteGenreLogic : IDomainLogic<DeleteGenreParam, Task<DeleteGenreResultData>>
    {
        private readonly IGenreRepository _genreRepository;

        public DeleteGenreLogic(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<DeleteGenreResultData> Execute(DeleteGenreParam param)
        {
            var genre = await _genreRepository.GetGenreById(param.Id);

            if (genre == null)
            {
                throw new ValidationException($"Genre don't already exists.");
            }

            genre.IsDeleted = true;

            await _genreRepository.UpdateGenre(genre);

            return new DeleteGenreResultData
            {
                Result = true,
                Message = "Delete Genres Successfully",
                StatusCode = StatusCodeEnum.Success
            };
        }
    }
}
