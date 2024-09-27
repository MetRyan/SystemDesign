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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => Helper.GenerateRandomLong()));

            CreateMap<User, UserResponseModel>();
            CreateMap<User, UserReponseInformation>();
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
            CreateMap <DriverReponse, User>();  

        }
    }
}
