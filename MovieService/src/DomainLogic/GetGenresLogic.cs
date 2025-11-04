using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
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
