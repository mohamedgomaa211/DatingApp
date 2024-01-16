using API.Dtos;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByUserName(string userName);
        Task<bool> SaveAllAsync();

        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string username);



    }
}
