using AutoMapper;
using Grpc.Core;
using MovieGrpc;
using src.DataTransferObject.Parameter;
using src.DomainLogic;

namespace src.Services
{
    public class MovieGrpcServiceImpl : MovieGrpcService.MovieGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly GetGenresLogic _getGenresLogic;
        private readonly CreateGenreLogic _createGenreLogic;
        private readonly UpdateGenreLogic _updateGenreLogic;
        private readonly DeleteGenreLogic _deleteGenreLogic;
        public MovieGrpcServiceImpl(IMapper mapper, 
                                    GetGenresLogic getGenresLogic, 
                                    CreateGenreLogic createGenreLogic, 
                                    UpdateGenreLogic updateGenreLogic,
                                    DeleteGenreLogic deleteGenreLogic)
        {
            _mapper = mapper;
            _getGenresLogic = getGenresLogic;
            _createGenreLogic = createGenreLogic;
            _updateGenreLogic = updateGenreLogic;
            _deleteGenreLogic = deleteGenreLogic;
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

        public override async Task<CreateGenreGrpcReplyDTO> CreateGenre(CreateGenreGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createGenreLogic.Execute(new CreateGenreParam
            {
                Name = request.Name,
            });

            return _mapper.Map<CreateGenreGrpcReplyDTO>(result);
        }

        public override async Task<UpdateGenreGrpcReplyDTO> UpdateGenre(UpdateGenreGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _updateGenreLogic.Execute(new UpdateGenreParam
            {
                Id = Guid.Parse(request.Id),
                Name = request.Name,
            });

            return _mapper.Map<UpdateGenreGrpcReplyDTO>(result);
        }

        public override async Task<DeleteGenreGrpcReplyDTO> DeleteGenre(DeleteGenreGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _deleteGenreLogic.Execute(new DeleteGenreParam
            {
                Id = Guid.Parse(request.Id)
            });

            return _mapper.Map<DeleteGenreGrpcReplyDTO>(result);
        }
    }
}
