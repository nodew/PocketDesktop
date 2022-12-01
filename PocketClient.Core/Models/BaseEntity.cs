using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketClient.Core.Models;

public abstract class Entity<TKey> : Entity
{
    public virtual TKey Id
    {
        get; set;
    }
}
