using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(DataContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRep userRep => new UserRep(_context, _mapper);

        public IMessageRep messageRep => new MessageRep(_context, _mapper);

        public ILikesRep likesRep => new LikeRep(_context);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChange()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}