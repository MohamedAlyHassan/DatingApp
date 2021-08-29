using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRep
    {
        Task<UserLike> GetUserLike(int sourceId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PaggedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}
