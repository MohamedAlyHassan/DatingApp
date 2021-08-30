using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRep
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PaggedList<MessgeDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessgeDto>> GetMessageThread(string currentUsername, string recipientUsername);
        Task<bool> SaveAllAsync();
        
    }
}