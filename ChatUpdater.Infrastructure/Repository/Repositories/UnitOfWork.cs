using ChatUpdater.Infrastructure.Data;
using ChatUpdater.Infrastructure.Repository.Interfaces;

namespace ChatUpdater.Infrastructure.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IMessageRepository Messages { get; }

        public IUserRepository Users { get; }

        public IGroupRepository Groups { get; }

        public IConnectionRepository Connections { get; }

        public IGroupChatRepository GroupChats { get; }

        public IGCContactsRepository GCContacts { get; }

        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            Messages = new MessageRepository(db);
            Users = new UserRepository(db);
            Groups = new GroupRepository(db);
            Connections = new ConnectionRepository(db);
            GroupChats = new GroupChatRepository(db);
            GCContacts = new GCContactsRepository(db);
            _db = db;
        }

        public async Task<bool> Save()
        {
            return 0 < await _db.SaveChangesAsync();
        }
    }
}
