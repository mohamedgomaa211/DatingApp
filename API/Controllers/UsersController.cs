using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class UsersController : BaseApiController
    {
        private readonly DataContext _Context;

        public UsersController(DataContext Context)
        {
            _Context = Context;
        }
        [HttpGet]
        public async Task <ActionResult<IEnumerable<AppUser>>> GetUsers()
        {

                var users = await _Context.Users.ToListAsync();
                return users;
        }
        [HttpGet("{id}")]

        public async Task <ActionResult<AppUser>> GetById(int id)
        {

            var user = await _Context.Users.FindAsync(id);
            return user;
        }
    }
}
