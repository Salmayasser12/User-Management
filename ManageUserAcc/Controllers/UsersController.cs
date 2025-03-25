using ManageUserAcc.Models;
using ManageUserAcc.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ManageUserAcc.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }



        // Admin and Manager can view the user list
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            return Ok(users);
        }





        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }




        //  Only Admin can create users
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
        {
            var result = await _userRepository.CreateUser(user, "Default@123");
            if (!result) return BadRequest("User creation failed.");
            return Ok("User created successfully.");
        }






        // Only Admin and Manager can update users
        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] ApplicationUser user)
        {
            var existingUser = await _userRepository.GetUserById(id);
            if (existingUser == null) return NotFound();

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Role = user.Role;

            var result = await _userRepository.UpdateUser(existingUser);
            if (!result) return BadRequest("User update failed.");

            return Ok("User updated successfully.");
        }





        // Only Admin can delete users
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userRepository.DeleteUser(id);
            if (!result) return NotFound("User not found.");
            return Ok("User deleted successfully.");
        }

        [HttpDelete("bulk-delete")]
        public async Task<IActionResult> DeleteUsers([FromBody] List<string> userIds)
        {
            var result = await _userRepository.DeleteUsers(userIds);
            if (!result) return NotFound("No users found to delete.");
            return Ok("Users deleted successfully.");
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string? name, [FromQuery] string? email, [FromQuery] string? role, [FromQuery] string? status)
        {
            var users = await _userRepository.SearchUsers(name, email, role, status);
            return Ok(users);
        }


       

        [HttpPatch("enable-disable/{userId}")]
        public async Task<IActionResult> EnableDisableUser(string userId, [FromBody] bool enable)
        {
            var result = await _userRepository.EnableDisableUser(userId, enable);
            if (!result) return NotFound("User not found.");
            return Ok(enable ? "User enabled successfully." : "User disabled successfully.");
        }

    }

}

