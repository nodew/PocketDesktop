using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketClient.Core.Models;

public class Author : Entity<long>
{
    public string Name
    {
        get; set;
    }

    public string Url
    {
        get; set;
    }

    public List<PocketItem> Items
    {
        get; set;
    }

    public List<ItemAuthor> ItemAuthors
    {
        get; set;
    }
}