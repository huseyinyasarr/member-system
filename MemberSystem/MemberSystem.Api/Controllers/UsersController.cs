using MemberSystem.Business.Interfaces;  // Burada artık IUserService tanımlı olmalı
using MemberSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;  // İsim güncellendi

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        // Tüm kullanıcıları listeleyen GET metodu.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();  // Metot ismi güncellendi
            return Ok(users);
        }

        // Belirtilen id’li kullanıcıyı dönen GET metodu.
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);  // Metot ismi güncellendi
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // Yeni kullanıcı ekleyen POST metodu.
        [HttpPost]
        public async Task<ActionResult> PostUser(User user)  // Parametre adı güncellendi
        {
            await _userService.AddUserAsync(user);  // Metot ismi güncellendi
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Mevcut kullanıcının güncellenmesi için PUT metodu.
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int id, User user)  // İsim güncellendi
        {
            if (id != user.Id)
                return BadRequest();
            await _userService.UpdateUserAsync(user);  // Metot ismi güncellendi
            return NoContent();
        }

        // Kullanıcının silinmesi için DELETE metodu.
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)  // İsim güncellendi
        {
            await _userService.DeleteUserAsync(id);  // Metot ismi güncellendi
            return NoContent();
        }
    }
}
