namespace Kuk.Entities.Common
{
    public interface IEntity
    {
    }
    
    public abstract class BaseEntity<TKey> : IEntity
    {
        public TKey Id { get; set; }
    }

    public abstract class BaseEntityGuid : BaseEntity<Guid>
    {
    }

    public abstract class BaseEntityInt : BaseEntity<int>
    {
    }
}
