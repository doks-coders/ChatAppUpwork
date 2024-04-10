using ChatUpdater.Infrastructure.Data;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Models.Entities;

namespace ChatUpdater.Infrastructure.Repository.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext db) : base(db) { }
    }
}
