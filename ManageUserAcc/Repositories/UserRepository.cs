using ManageUserAcc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ManageUserAcc.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private object query;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> CreateUser(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUser(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        public async Task<bool> DeleteUsers(List<string> userIds)
        {
            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

            if (!users.Any()) return false;

            foreach (var user in users)
            {
                await _userManager.DeleteAsync(user);
            }
            return true;
        }

        public async Task<IEnumerable<ApplicationUser>> SearchUsers(string? name, string? email, string? role, string? status)
        {
            var query = _userManager.Users.AsQueryable(); // Ensure query is initialized

            if (!string.IsNullOrEmpty(name))
                query = query.Where(u => u.FirstName.Contains(name) || u.LastName.Contains(name));

            if (!string.IsNullOrEmpty(email))
                query = query.Where(u => u.Email.Contains(email));

            if (!string.IsNullOrEmpty(role))
                query = query.Where(u => u.Role == role);

            if (!string.IsNullOrEmpty(status))
            {
                status = status.ToLower(); // Ensure lowercase comparison
                query = query.Where(u =>
                    (status == "active" && u.LockoutEnd == null) ||
                    (status == "inactive" && u.LockoutEnd != null)
                );
            }

            return await query.ToListAsync();
        }

    
   public async Task<bool> EnableDisableUser(string userId, bool enable)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null) return false;

    if (enable)
    {
        user.LockoutEnd = null; // Enable user
    }
    else
    {
        user.LockoutEnd = DateTimeOffset.MaxValue; // Disable user
    }

    var result = await _userManager.UpdateAsync(user);
    return result.Succeeded;
}


    }

}
