using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikeRep : ILikesRep
    {
        private readonly DataContext _context;

        public LikeRep(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int sourceId, int likedUserId)
        {
            return await _context.Likes.FindAsync(sourceId, likedUserId);
        }

        public async Task<PaggedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
           var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
           var likes = _context.Likes.AsQueryable();

           if (likesParams.Predicate == "liked")
           {
               likes= likes.Where(like => like.SourceUserId == likesParams.UserId);
               users = likes.Select(like => like.LikedUser);
           }

           if (likesParams.Predicate == "likedBy")
           {
               likes= likes.Where(like => like.LikedUserId == likesParams.UserId);
               users = likes.Select(like => like.SourceUser);
           }

          var likedUsers=users.Select(user => new LikeDto
           {
               Username = user.UserName,
               KnowAs = user.KnowAs,
               Age = user.DateOfBirth.CalculateAge(),
               PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
               City = user.City,
               Id = user.Id
           });

           return await PaggedList<LikeDto>.CreateAsync(likedUsers,likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
             .Include(x=> x.LikedUsers)
             .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}