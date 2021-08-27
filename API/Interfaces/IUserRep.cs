using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRep
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PaggedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string username);
        
    }
}