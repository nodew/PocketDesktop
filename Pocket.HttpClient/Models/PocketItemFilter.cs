using System;
using System.Collections.Generic;
using System.Text;

namespace Pocket.Sdk;

public class PocketItemFilter
{
    public PocketItemState? State
    {
        get; set;
    }

    public bool? Favorite
    {
        get; set;
    }

    public string? Tag
    {
        get; set;
    }

    public PocketItemContentType? ContentType
    {
        get; set;
    }

    public PocketItemDetailType? DetailType
    {
        get; set;
    }

    public PocketItemSortMethod? SortBy
    {
        get; set;
    }

    public string? Search
    {
        get; set;
    }

    public string? Domain
    {
        get; set;
    }

    public DateTimeOffset? Since
    {
        get; set;
    }
}
