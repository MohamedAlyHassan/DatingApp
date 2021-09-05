using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.entities;
using System.Collections.Generic;
using API.DTOs;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
     [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public LikesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var LikedUser = await _unitOfWork.userRep.GetUserByUsernameAsync(username);
            var sourceUser = await _unitOfWork.likesRep.GetUserWithLikes(sourceUserId);

            if (LikedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _unitOfWork.likesRep.GetUserLike(sourceUserId, LikedUser.Id);

            if (userLike != null) return BadRequest("You already like this user");

            userLike = new UserLike 
            {
                SourceUserId = sourceUserId,
                LikedUserId = LikedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {    
            likesParams.UserId = User.GetUserId(); 
            var users = await _unitOfWork.likesRep.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage,
            users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}