using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
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
