namespace TourNest.Services.User.ProfilePicture
{
    public interface IUserRepository
    {
        Task UpdateUserProfilePhotoAsync(Guid userId, string photoUrl);
    }
}
