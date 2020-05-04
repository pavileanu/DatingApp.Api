using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using MyApi.Data;
using MyApi.Dtos;
using MyApi.Helpers;
using MyApi.Models;
using System;

namespace MyApi.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("users/{userId}/{controller}")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id); 
            if(messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);          
        
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesFromUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            messageParams.UserId = userId;

            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageReturnDto>>(messagesFromRepo);
            
            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
                                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);  
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageCreationDto messageDto)
        {
            var sender = await _repo.GetUser(userId);

            if(sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
        
            messageDto.SenderId = userId;
            var recipient = await _repo.GetUser(messageDto.RecipientId);
            if(recipient == null)
                return BadRequest("Could not find user"); 
            
            var message = _mapper.Map<Message>(messageDto);
            _repo.add(message);

            if(await _repo.SaveAll())
            {
                var messageForReturn = _mapper.Map<MessageReturnDto>(message);
                return Ok(messageForReturn);
            }
            throw new Exception("Creting the message failed on save");
        }

                 
         [HttpGet("thread/{recipientId}")]
         public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
         {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messageFromRepo = await _repo.GetMessageThread(userId, recipientId);
            var messageThread = _mapper.Map<IEnumerable<MessageReturnDto>>(messageFromRepo);

            return Ok(messageThread);
         }

         [HttpPost("{id}")]
         public async Task<IActionResult> DeleteMessage(int id, int userId)
         {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messageFromRepo = await _repo.GetMessage(id);
            if(messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;
            
            if(messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;
            
            if(messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repo.remove(messageFromRepo);
            
            if(await _repo.SaveAll())
                return NoContent();
            
            throw new Exception("Error deleting message");
         }
        
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var message = await _repo.GetMessage(id);

            if(message.RecipientId != userId)
                return Unauthorized();
            
            message.isRead = true;
            message.DatRead = DateTime.Now;

            await _repo.SaveAll();
            return NoContent();
        }

    }
}