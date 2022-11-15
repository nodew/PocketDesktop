using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pocket.Client.Core.Data;

namespace Pocket.Client.Core.Models;

public class Tag : Entity<Guid>
{
    public string Name
    {
        get; set;
    }

    public bool IsPinned
    {
        get; set;
    }

    public List<PocketItem> Items
    {
        get; set;
    }

    public List<ItemTag> ItemTags
    {
        get; set;
    }
}
