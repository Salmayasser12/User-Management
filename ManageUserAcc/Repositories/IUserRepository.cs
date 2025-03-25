using ManageUserAcc.Models;

namespace ManageUserAcc.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
        Task<ApplicationUser> GetUserById(string id);
        Task<bool> CreateUser(ApplicationUser user, string password);
        Task<bool> UpdateUser(ApplicationUser user);
        Task<bool> DeleteUser(string id);
        Task<bool> DeleteUsers(List<string> userIds);

        Task<IEnumerable<ApplicationUser>> SearchUsers(string? name, string? email, string? role, string? status);
        Task<bool> EnableDisableUser(string userId, bool enable);


    }


}