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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessagesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

         [HttpGet]
        public async Task<ActionResult<IEnumerable<MessgeDto>>> GetMessagesForUser([FromQuery]
        MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _unitOfWork.messageRep.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize,
            messages.TotalCount,messages.TotalPages);

            return messages;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id) 
        {
            var username = User.GetUsername();
            var message = await _unitOfWork.messageRep.GetMessage(id);
            if (message.Sender.UserName != username && message.Recipient.UserName != username)
                return Unauthorized();

            if (message.Sender.UserName == username) message.SenderDeleted = true;

            if (message.Recipient.UserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
               _unitOfWork.messageRep.DeleteMessage(message);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("problem deleting the message");       
        }

    }
}
