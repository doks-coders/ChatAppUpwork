using ChatUpdater.Infrastructure.Data;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Models.Entities;

namespace ChatUpdater.Infrastructure.Repository.Repositories
{
    public class ConnectionRepository : Repository<Connection>, IConnectionRepository
    {
        public ConnectionRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
