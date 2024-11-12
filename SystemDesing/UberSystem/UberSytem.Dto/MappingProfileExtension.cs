using AutoMapper;
using UberSystem.Domain.Entities;
using UberSytem.Dto.Requests;
using UberSytem.Dto.Responses;

namespace UberSytem.Dto
{
    public class MappingProfileExtension : Profile
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public MappingProfileExtension()
        {
            CreateMap<User, Customer>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => Helper.GenerateRandomLong()));
            CreateMap<User, Driver>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => Helper.GenerateRandomLong()))
                .ReverseMap();

            CreateMap<User, UserResponseModel>().ReverseMap();
            CreateMap<Driver, UserReponseInformation>().ReverseMap();
            CreateMap<CreateFeedback, Rating>().ReverseMap();


            CreateMap<User, UserReponseInformation>().ReverseMap(); 
            CreateMap<TripRequest, Trip>()
           .ForMember(dest => dest.SourceLatitude, opt => opt.MapFrom(src => src.SourceLatitude))
           .ForMember(dest => dest.SourceLongitude, opt => opt.MapFrom(src => src.SourceLongitude))
           .ForMember(dest => dest.DestinationLatitude, opt => opt.MapFrom(src => src.DestinationLatitude))
           .ForMember(dest => dest.DestinationLongitude, opt => opt.MapFrom(src => src.DestinationLongitude))
           .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId));

            // Mapping from TripRequest to Payment
            CreateMap<TripRequest, Payment>()
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
            CreateMap<SignupModel, User>();
            CreateMap<TripReponse,Trip>().ReverseMap();


            CreateMap<DriverReponse, Driver>();

            // CreateMap<HistoryTrip,Trip>()
            // .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.PaymentId));
            // CreateMap<HistoryTrip,Payment>();
            // CreateMap<HistoryTrip,Customer>()
            
             CreateMap<Trip, HistoryTrip>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Customer.User.UserName))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.SourceLatitude, opt => opt.MapFrom(src => src.SourceLatitude))
            .ForMember(dest => dest.SourceLongitude, opt => opt.MapFrom(src => src.SourceLongitude))
            .ForMember(dest => dest.DestinationLatitude, opt => opt.MapFrom(src => src.DestinationLatitude))
            .ForMember(dest => dest.DestinationLongitude, opt => opt.MapFrom(src => src.DestinationLongitude))
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.PaymentId))
            .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Payment.Method))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Payment.Amount));
            CreateMap<HistoryTrip,Customer>();
// tu customer moi qua dc User
            CreateMap<HistoryTrip,User>();

            CreateMap<HistoryTrip,Payment>();


            CreateMap<DriverReponse, User>();  

        }
    }
}
