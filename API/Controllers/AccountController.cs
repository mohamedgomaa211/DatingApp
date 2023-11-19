using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [AllowAnonymous]
    public class AccountController  : BaseApiController
    {
        private readonly DataContext _Context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dataContext, ITokenService tokenInterface)
        {
            _Context=dataContext;
            _tokenService = tokenInterface;
        }
        [Authorize]

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if(await _Context.Users.AnyAsync(x=>x.UserName==registerDto.UserName.ToLower())) return BadRequest("User Is taken");
            using HMACSHA512? hmac = new HMACSHA512();
            var user = new AppUser()
            {
                UserName = registerDto.UserName.ToLower(),
                 PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            await _Context.Users.AddAsync(user);
            await _Context.SaveChangesAsync();
            return new UserDto
            {
                Username=user.UserName,
                Token= _tokenService.CreateToken(user)

            };

        }
        [Authorize]

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _Context.Users.SingleOrDefaultAsync(x=>x.UserName==loginDto.UserName);

            if (user == null) return Unauthorized("invalid Password");
            using HMACSHA512? hmac = new HMACSHA512(user.PasswordSalt);
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i < ComputedHash.Length; i++)
            {
                if (ComputedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid Password");
            }
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)

            };


        }

    }
}
