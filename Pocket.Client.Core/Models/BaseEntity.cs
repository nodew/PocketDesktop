using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket.Client.Core.Models;

public abstract class Entity<TKey>
{
    public virtual TKey Id
    {
        get; set;
    }
}
