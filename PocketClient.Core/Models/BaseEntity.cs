namespace PocketClient.Core.Models;

public abstract class Entity<TKey> : Entity
{
    public virtual TKey Id
    {
        get; set;
    }
}
