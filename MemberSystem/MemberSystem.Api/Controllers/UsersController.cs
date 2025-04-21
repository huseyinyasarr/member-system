using MemberSystem.Business.Interfaces;
using MemberSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberSystem.Api.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // Tüm kullanıcıları listeleyen GET metodu.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync(); 
            return Ok(users);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // Yeni kullanıcı ekleyen POST metodu.
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] User user)
        {
            await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Mevcut kullanıcının güncellenmesi için PUT metodu.
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest();
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        // Kullanıcının silinmesi için DELETE metodu.
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}