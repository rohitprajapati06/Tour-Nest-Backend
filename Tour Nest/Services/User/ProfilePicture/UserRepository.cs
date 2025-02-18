using Microsoft.EntityFrameworkCore;
using TourNest.Models;

namespace TourNest.Services.User.ProfilePicture;

public class UserRepository : IUserRepository
{
    private readonly TourNestContext _context; // Replace with your actual DbContext class

    public UserRepository(TourNestContext context)
    {
        _context = context;
    }

    public async Task UpdateUserProfilePhotoAsync(Guid userId, string photoUrl)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
            throw new Exception("User not found.");

        user.ProfilePhoto = photoUrl;
        await _context.SaveChangesAsync();
    }
}
