﻿using PocketClient.Core.Models;

namespace PocketClient.Desktop.Models;

public class ItemArchiveStatusChangedMessage
{
    public ItemArchiveStatusChangedMessage(PocketItem item)
    {
        Item = item;
    }

    public PocketItem Item
    {
        get; set;
    }
}
