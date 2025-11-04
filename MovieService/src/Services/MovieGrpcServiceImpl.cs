using Grpc.Core;
using src;
using MovieGrpc;
using src.DomainLogic;
using src.DataTransferObject.Parameter;
using AutoMapper;

namespace src.Services
{
    public class MovieGrpcServiceImpl : MovieGrpcService.MovieGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly GetGenresLogic _getGenresLogic;
        public MovieGrpcServiceImpl(IMapper mapper, GetGenresLogic getGenresLogic)
        {
            _mapper = mapper;
            _getGenresLogic = getGenresLogic;
        }

        public override async Task<GetGenresGrpcReplyDTO> GetGenres(GetGenresGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? genreId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.Id, out var parsedId))
            {
                genreId = parsedId;
            }

            var result = await _getGenresLogic.Execute(new GetGenresParam
            {
                Id = genreId,
                Name = request.Name,
            });

            return _mapper.Map<GetGenresGrpcReplyDTO>(result);
        }
    }
}
