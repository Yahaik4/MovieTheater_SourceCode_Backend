using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiGateway.DataTransferObject.Parameter
{
    public class GetShowtimesByCinemaRequestParam
    {
        [BindRequired]
        public DateOnly Date { get; set; }
    }
}
