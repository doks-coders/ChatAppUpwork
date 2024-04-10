namespace ChatUpdater.Models.Entities
{
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; } = default!;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public Guid? DeletedBy { get; set; }
        public Guid? LastModifiedBy { get; set; }
    }

    public interface IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
