
using ChatUpdater.Infrastructure.Data;
using ChatUpdater.Infrastructure.Repository.Interfaces;
using ChatUpdater.Models.Entities;

namespace ChatUpdater.Infrastructure.Repository.Repositories
{
    public class GCContactsRepository : Repository<GCContacts>, IGCContactsRepository
    {
        public GCContactsRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
