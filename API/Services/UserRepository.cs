using API.Data;
using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public UserRepository(DataContext dataContext,IMapper mapper)
        {
            _context = dataContext;
            _mapper = mapper;

        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserName(string userName)
        {
            return await _context.Users.Include(u => u.Photos).SingleOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.Include(u=>u.Photos).ToListAsync();

        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync()>0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;  
        }
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            query = query.Where(predicate: u => u.UserName != userParams.CurrentUserName);
            query = query.Where(u => u.Gender == userParams.Gender);
            query = userParams.OrderBy switch
            {
              "created"=>query.OrderByDescending(x=>x.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };


            return await PagedList<MemberDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

    }
}
