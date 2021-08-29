using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.entities;
using System.Collections.Generic;
using API.DTOs;
using API.Helpers;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly IUserRep _userRep;
        private readonly ILikesRep _likesRep;

        public LikesController(IUserRep userRep, ILikesRep likesRep)
        {
            _userRep = userRep;
            _likesRep = likesRep;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var LikedUser = await _userRep.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRep.GetUserWithLikes(sourceUserId);

            if (LikedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _likesRep.GetUserLike(sourceUserId, LikedUser.Id);

            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike 
            {
                SourceUserId = sourceUserId,
                LikedUserId = LikedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _userRep.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {    
            likesParams.UserId = User.GetUserId(); 
            var users = await _likesRep.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage,
            users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}