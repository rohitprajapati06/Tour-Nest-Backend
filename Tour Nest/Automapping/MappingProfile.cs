using AutoMapper;
using TourNest.Models.TravelExpenses.UserBudget;
using TourNest.Models.UserProfile;

namespace TourNest.Automapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Register Mapping
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.FirstName + "" + src.LastName))
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(_ => Guid.NewGuid()));

            // Login Mapping (if needed)
            CreateMap<LoginDTO, User>();

            CreateMap<User, VerifyOtpDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Otp, opt => opt.Ignore()); // Ignore Otp since it's not in User

            CreateMap<UserExpenseView, UserExpense>();

        }
    }
}
