using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{   
    
    public class UsersController : BaseApiController
    {
        private readonly IUserRep _userRep;
        private readonly IMapper _mapper;

        public UsersController(IUserRep userRep , IMapper mapper)
        {
            _userRep = userRep;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous] 
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRep.GetMembersAsync();
           return Ok(users);
            
        }

         [Authorize]
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRep.GetMemberAsync(username);
             
        }
    }
}