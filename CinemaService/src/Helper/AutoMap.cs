using AutoMapper;
using CinemaGrpc;
using CinemaService.DataTransferObject.ResultData;

namespace CinemaService.Helper
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {

            //GetAllCinemaMapping
            CreateMap<GetAllCinemasResultData, GetAllCinemasGrpcReplyDTO>();
            CreateMap<GetAllCinemasDataResult, GetAllCinemasGrpcReplyDataDTO>();

            // CreateCinemaMapping
            CreateMap<CreateCinemaResultData, CreateCinemaGrpcReplyDTO>();
            CreateMap<CreateCinemaDataResult, CreateCinemaGrpcReplyDataDTO>();

            // UpdateCinemaMapping
            CreateMap<UpdateCinemaResultData, UpdateCinemaGrpcReplyDTO>();
            CreateMap<UpdateCinemaDataResult, UpdateCinemaGrpcReplyDataDTO>();

            // DeleteCinemaMapping
            CreateMap<DeleteCinemaResultData, DeleteCinemaGrpcReplyDTO>();

            // CreateRoomTypeMapping
            CreateMap<CreateRoomTypeResultData, CreateRoomTypeGrpcReplyDTO>();
            CreateMap<CreateRoomTypeDataResult, CreateRoomTypeGrpcReplyDataDTO>();

            //GetAllRoomTypeMapping
            CreateMap<GetAllRoomTypeResultData, GetAllRoomTypesGrpcReplyDTO>();
            CreateMap<GetAllRoomTypesDataResult, GetAllRoomTypesGrpcReplyDataDTO>();

            // UpdateRoomTypeMapping
            CreateMap<UpdateRoomTypeResultData, UpdateRoomTypeGrpcReplyDTO>();
            CreateMap<UpdateRoomTypeDataResult, UpdateRoomTypeGrpcReplyDataDTO>();

            // DeleteRoomTypeMapping
            CreateMap<DeleteRoomTypeResultData, DeleteRoomTypeGrpcReplyDTO>();

            // GetAllSeatTypeMapping
            CreateMap<GetAllSeatTypeResultData, GetAllSeatTypesGrpcReplyDTO>();
            CreateMap<GetAllSeatTypeDataResult, GetAllSeatTypesGrpcReplyDataDTO>();

            // CreateSeatTypeMapping
            CreateMap<CreateSeatTypeResultData, CreateSeatTypeGrpcReplyDTO>();
            CreateMap<CreateSeatTypeDataResult, CreateSeatTypeGrpcReplyDataDTO>();

            // UpdateSeatTypeMapping
            CreateMap<UpdateSeatTypeResultData, UpdateSeatTypeGrpcReplyDTO>();
            CreateMap<UpdateSeatTypeDataResult, UpdateSeatTypeGrpcReplyDataDTO>();

            // DeleteSeatTypeMapping
            CreateMap<DeleteSeatTypeResultData, DeleteSeatTypeGrpcReplyDTO>();

            CreateMap<CreateRoomResultData, CreateRoomGrpcReplyDTO>();
            CreateMap<CreateRoomDataResult, CreateRoomGrpcReplyDataDTO>();
            CreateMap<CreateSeatDataResult, CreateSeatGrpcReplyDataDTO>();

            CreateMap<GetAllRoomResultData, GetAllRoomsGrpcReplyDTO>();
            CreateMap<GetAllRoomDataResult, GetAllRoomsGrpcReplyDataDTO>();

            CreateMap<UpdateRoomResultData, UpdateRoomGrpcReplyDTO>();
            CreateMap<UpdateRoomDataResult, UpdateRoomGrpcReplyDataDTO>();

            CreateMap<DeleteRoomResultData, DeleteRoomGrpcReplyDTO>();

            CreateMap<GetAllSeatResultData, GetAllSeatsGrpcReplyDTO>();
            CreateMap<GetAllSeatDataResult, GetAllSeatsGrpcReplyDataDTO>();

            CreateMap<UpdateSeatsResultData, UpdateSeatsGrpcReplyDTO>();
            CreateMap<UpdateSeatsDataResult, UpdateSeatsGrpcReplyDataDTO>();

            CreateMap<GetShowtimesByMovieResultData, GetShowtimesGrpcReplyDTO>();
            CreateMap<GetShowtimesByMovieDataResult, GetShowtimesGrpcReplyDataDTO>();
            CreateMap<GetRoomTypeDataResult, GetRoomTypeGrpcReplyDataDTO>();
            CreateMap<ShowtimeDataResult, ShowtimeGrpcReplyDataDTO>();

            CreateMap<GetShowtimesByCinemaResultData, GetShowtimesByCinemaGrpcReplyDTO>();
            CreateMap<GetShowtimesByCinemaDataResult, GetShowtimesByCinemaGrpcReplyDataDTO>();

            CreateMap<GetShowtimeDetailsResultData, GetShowtimeDetailsGrpcReplyDTO>();
            CreateMap<GetShowtimeDetailsDataResult, GetShowtimeDetailsGrpcReplyDataDTO>();

            CreateMap<CreateShowtimeResultData, CreateShowtimeGrpcReplyDTO>();
            CreateMap<CreateShowtimeDataResult, CreateShowtimeGrpcReplyDataDTO>();

            CreateMap<UpdateShowtimeResultData, UpdateShowtimeGrpcReplyDTO>();
            CreateMap<UpdateShowtimeDataResult, UpdateShowtimeGrpcReplyDataDTO>();

            CreateMap<GetShowtimeSeatsResultData, GetShowtimeSeatsGrpcReplyDTO>();
            CreateMap<GetShowtimeSeatsDataResult, GetShowtimeSeatsGrpcReplyDataDTO>();

            CreateMap<GetBookingResultData, GetBookingGrpcReplyDTO>();
            CreateMap<GetBookingDataResult, GetBookingGrpcReplyDataDTO>();
            CreateMap<GetSeatsBooking, GetSeatsBookingGrpc>();

            CreateMap<CreateBookingResultData, CreateBookingGrpcReplyDTO>();
            CreateMap<CreateBookingDataResult, CreateBookingGrpcReplyDataDTO>();
            CreateMap<BookingSeatsDataResult, CreateBookingSeatsGrpcReplyDataDTO>();

            CreateMap<UpdateBookingStatusResultData, UpdateBookingStatusGrpcReplyDTO>();
            CreateMap<UpdateBookingStatusDataResult, UpdateBookingStatusGrpcReplyDataDTO>();
            CreateMap<UpdateBookingSeatsDataResult, UpdateBookingSeatsStatus>();

            CreateMap<GetAllFoodDrinkResultData, GetAllFoodDrinksGrpcReplyDTO>();
            CreateMap<GetAllFoodDrinkDataResult, GetAllFoodDrinksGrpcReplyDataDTO>();

            CreateMap<CreateFoodDrinkResultData, CreateFoodDrinkGrpcReplyDTO>();
            CreateMap<CreateFoodDrinkDataResult, CreateFoodDrinkGrpcReplyDataDTO>();

            CreateMap<UpdateFoodDrinkResultData, UpdateFoodDrinkGrpcReplyDTO>();
            CreateMap<UpdateFoodDrinkDataResult, UpdateFoodDrinkGrpcReplyDataDTO>();

            CreateMap<DeleteFoodDrinkResultData, DeleteFoodDrinkGrpcReplyDTO>();
            CreateMap<BookingFoodDrinkDataResult, CreateBookingFoodDrinkGrpcReplyDataDTO>();

            CreateMap<CheckInBookingResultData, CheckInBookingGrpcReplyDTO>();
            CreateMap<CheckInBookingDataResult, CheckInBookingGrpcReplyDataDTO>();

            CreateMap<GetBookingHistoryResultData, GetBookingHistoryGrpcReplyDTO>();

            CreateMap<GetCustomerTypesResultData, GetCustomerTypesGrpcReplyDTO>();
            CreateMap<GetCustomerTypesDataResult, GetCustomerTypesGrpcReplyDataDTO>();

            CreateMap<CreateCustomerTypeResultData, CreateCustomerTypeGrpcReplyDTO>();
            CreateMap<CreateCustomerTypeDataResult, CreateCustomerTypeGrpcReplyDataDTO>();

            CreateMap<UpdateCustomerTypeResultData, UpdateCustomerTypeGrpcReplyDTO>();
            CreateMap<UpdateCustomerTypeDataResult, UpdateCustomerTypeGrpcReplyDataDTO>();

            CreateMap<GetHolidaysResultData, GetHolidaysGrpcReplyDTO>();
            CreateMap<GetHolidaysDataResult, GetHolidaysGrpcReplyDataDTO>();

            CreateMap<CreateHolidayResultData, CreateHolidayGrpcReplyDTO>();
            CreateMap<CreateHolidayDataResult, CreateHolidayGrpcReplyDataDTO>();

            CreateMap<UpdateHolidayResultData, UpdateHolidayGrpcReplyDTO>();
            CreateMap<UpdateHolidayDataResult, UpdateHolidayGrpcReplyDataDTO>();

            CreateMap<GetPromotionsResultData, GetPromotionsGrpcReplyDTO>();
            CreateMap<GetPromotionsDataResult, GetPromotionsGrpcReplyDataDTO>();

            CreateMap<CreatePromotionResultData, CreatePromotionGrpcReplyDTO>();
            CreateMap<CreatePromotionDataResult, CreatePromotionGrpcReplyDataDTO>();

            CreateMap<UpdatePromotionResultData, UpdatePromotionGrpcReplyDTO>();
            CreateMap<UpdatePromotionDataResult, UpdatePromotionGrpcReplyDataDTO>();

            CreateMap<SearchPromotionResultData, SearchPromotionGrpcReplyDTO>();
        }
    }
}
