using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRep _userRep;
        private readonly IMessageRep _messgeRep;
        private readonly IMapper _mapper;

        public MessagesController(IUserRep userRep, IMessageRep messgeRep,IMapper mapper)
        {
            _userRep = userRep;
            _messgeRep = messgeRep;
            _mapper = mapper;
        }

         [HttpPost]
        public async Task<ActionResult<MessgeDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send messages to yourself");

            var sender = await _userRep.GetUserByUsernameAsync(username);
            var recipient = await _userRep.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messgeRep.AddMessage(message);

            if (await _messgeRep.SaveAllAsync()) return Ok(_mapper.Map<MessgeDto>(message));

            return BadRequest("Failed to send Message");  
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessgeDto>>> GetMessagesForUser([FromQuery]
        MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _messgeRep.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize,
            messages.TotalCount,messages.TotalPages);

            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessgeDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();

            return Ok(await _messgeRep.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id) 
        {
            var username = User.GetUsername();
            var message = await _messgeRep.GetMessage(id);
            if (message.Sender.UserName != username && message.Recipient.UserName != username)
                return Unauthorized();

            if (message.Sender.UserName == username) message.SenderDeleted = true;

            if (message.Recipient.UserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
               _messgeRep.DeleteMessage(message);

            if (await _messgeRep.SaveAllAsync()) return Ok();

            return BadRequest("problem deleting the message");       
        }
    }
}
