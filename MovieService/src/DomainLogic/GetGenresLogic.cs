using MovieService.DataTransferObject.Parameter;
using MovieService.DataTransferObject.ResultData;
using MovieService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace MovieService.DomainLogic
{
    public class GetGenresLogic : IDomainLogic<GetGenresParam, Task<GetGenresResultData>>
    {
        private readonly IGenreRepository _genreRepository;

        public GetGenresLogic(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<GetGenresResultData> Execute(GetGenresParam param)
        {
            var genres = await _genreRepository.GetGenres(param);

            return new GetGenresResultData
            {
                Result = true,
                Message = "Get Genres Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = genres.Select(g => new GetGenresDataResult
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToList()
            };
        }
    }
}
