using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRep
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PaggedList<MessgeDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessgeDto>> GetMessageThread(string currentUsername, string recipientUsername);
        Task<bool> SaveAllAsync();
        
    }
}