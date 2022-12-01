using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketClient.Core.Models;

public class ItemTag
{
    public long ItemId
    {
        get; set;
    }

    public Guid TagId
    {
        get; set;
    }

    public PocketItem Item
    {
        get; set;
    }

    public Tag Tag
    {
        get; set;
    }
}
