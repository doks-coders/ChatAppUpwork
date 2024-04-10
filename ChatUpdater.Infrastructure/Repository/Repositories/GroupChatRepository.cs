using ChatUpdater.Infrastructure.Data;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Models.Entities;

namespace ChatUpdater.Infrastructure.Repository.Repositories
{
    public class GroupChatRepository : Repository<GroupChat>, IGroupChatRepository
    {
        public GroupChatRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
