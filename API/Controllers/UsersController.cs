using API.Data;
using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{

    public class UsersController : BaseApiController
    {
        IUserRepository _userRepository;

        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUser = await _userRepository.GetUserByUserName(User.GetUsername());
            userParams.CurrentUserName = currentUser.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages));

            return Ok(users);
        }
        //[HttpGet]
        //public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        //{


        //    var currentUser = await _userRepository.GetUserByUserName(User.GetUsername());
        //    //if (currentUser == null)
        //    //{
        //    //    return BadRequest("user is not exist");
        //    //}
        //    //userParams.CurrentUserName = currentUser.UserName;

        //    //userParams.CurrentUserName = currentUser.UserName;
        //    if(string.IsNullOrEmpty(userParams.Gender))
        //    {

        //        userParams.Gender= currentUser.Gender=="male"?"female":"male";

        //    }
        //    var users = await _userRepository.GetMembersAsync(userParams);
        //    Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

        //    return Ok(users);


        //}
        [HttpGet("{username}")]

        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {

            return await _userRepository.GetMemberAsync(username);

        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateUserDto updateUser)

        {
            var user = await _userRepository.GetUserByUserName(User.GetUsername());
            if (user == null) return NotFound();

            _mapper.Map(updateUser, user);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            ////var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = HttpContext.User.Identity.Name;

            var user = await _userRepository.GetUserByUserName(User.GetUsername());
            if (user == null) return NotFound();
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUri.AbsoluteUri,
                PublicId = result.PublicId,
            };
            if (user.Photos.Count == 0) photo.IsMain = true;
            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetUser),
                    new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("problem when Adding photo");

        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUserName(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting main photo");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUserName(User.GetUsername());
            if (user == null) return NotFound();
            var photo = user.Photos.FirstOrDefault(x=>x.Id==photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("you  Cannot Delete Main Photo");
            if (photo.PublicId != null)
            {
                var result=await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error!= null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting photo");
        }

    }
}
