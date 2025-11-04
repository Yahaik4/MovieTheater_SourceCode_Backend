using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class CreateGenreLogic : IDomainLogic<CreateGenreParam, Task<CreateGenreResultData>>
    {
        private readonly IGenreRepository _genreRepository;

        public CreateGenreLogic(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<CreateGenreResultData> Execute(CreateGenreParam param)
        {
            var genre = await _genreRepository.GetGenreByName(param.Name);

            if (genre != null)
            {
                throw new ValidationException($"Genre {param.Name} already exists.");
            }

            var newGenre = await _genreRepository.CreateGenre(new Genre
            {
                Id = Guid.NewGuid(),
                Name = param.Name,
                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.UtcNow
            });

            return new CreateGenreResultData
            {
                Result = true,
                Message = "Create Genres Successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateGenreDataResult
                {
                    Id = newGenre.Id,
                    Name = newGenre.Name,
                }
            };
        }
    }
}
