using MovieService.DataTransferObject.Parameter;
using MovieService.DataTransferObject.ResultData;
using MovieService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace MovieService.DomainLogic
{
    public class UpdateGenreLogic : IDomainLogic<UpdateGenreParam, Task<UpdateGenreResultData>>
    {
        private readonly IGenreRepository _genreRepository;

        public UpdateGenreLogic(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<UpdateGenreResultData> Execute(UpdateGenreParam param)
        {
            var genre = await _genreRepository.GetGenreById(param.Id);

            if (genre == null)
            {
                throw new NotFoundException($"Genre {param.Name} don't exists.");
            }

            genre.Name = param.Name;
            genre.UpdatedAt = DateTime.UtcNow;

            await _genreRepository.UpdateGenre(genre);

            return new UpdateGenreResultData
            {
                Result = true,
                Message = "Update Genres Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateGenreDataResult
                {
                    Id = genre.Id,
                    Name = param.Name,
                }
            };
        }
    }
}
