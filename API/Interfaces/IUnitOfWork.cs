using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRep userRep {get; }
        IMessageRep messageRep {get; }
        ILikesRep likesRep {get; }
        Task<bool> Complete();
        bool HasChange();
    }
}