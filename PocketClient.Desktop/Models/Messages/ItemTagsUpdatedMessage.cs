using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocketClient.Core.Models;

namespace PocketClient.Desktop.Models;

public class ItemTagsUpdatedMessage
{
    public ItemTagsUpdatedMessage(PocketItem item, List<Tag> tags)
    {
        Item = item;
        Tags = tags;
    }

    public PocketItem Item { get; set; }

    public List<Tag> Tags { get; set; }
}
