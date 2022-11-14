using System.Text.Json.Serialization;

namespace Pocket.Client.Core.Models;

public class PocketAction
{
    public PocketAction(string action)
    {
        this.Action = action;
        this.Time = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    /// <summary>
    /// The action to perform.
    /// </summary>
    [JsonPropertyName("action")]
    public string Action
    {
        get; set;
    }

    /// <summary>
    /// The time the action occurred.
    /// </summary>
    [JsonPropertyName("time")]
    public long Time
    {
        get; set;
    }
}

public class PocketBasicAction : PocketAction
{
    public PocketBasicAction(string action, long itemId) : base(action)
    {
        this.ItemId = itemId;
    }

    /// <summary>
    /// The id of the item to perform the action on.
    /// </summary>
    [JsonPropertyName("item_id")]
    public long ItemId
    {
        get; set;
    }
}

#region Basic actions

/// <summary>
/// Action to move an item to the user's archive.
/// </summary>
public class ArchiveItemAction : PocketBasicAction
{
    public ArchiveItemAction(long itemId) : base("Archive", itemId)
    {
    }
}

/// <summary>
/// Action to move an item from the user's archive back into their unread list.
/// </summary>
public class ReAddItemAction : PocketBasicAction
{
    public ReAddItemAction(long itemId) : base("readd", itemId)
    {
    }
}

/// <summary>
/// Action to mark an item as a favorite.
/// </summary>
public class FavoriteItemAction : PocketBasicAction
{
    public FavoriteItemAction(long itemId) : base("favorite", itemId)
    {
    }
}

/// <summary>
/// Action to remove an item from the user's favorites.
/// </summary>
public class UnfavoriteItemAction : PocketBasicAction
{
    public UnfavoriteItemAction(long itemId) : base("unfavorite", itemId)
    {
    }
}

/// <summary>
/// Action to permanently remove an item from the user's account.
/// </summary>
public class DeleteItemAction : PocketBasicAction
{
    public DeleteItemAction(long itemId) : base("delete", itemId)
    {
    }
}

#endregion

#region Tagging actions

/// <summary>
/// Action to add one or more tags to an item.
/// </summary>
public class AddTagsAction : PocketBasicAction
{
    public AddTagsAction(long itemId, string tags) : base("tags_add", itemId)
    {
        this.ItemId = itemId;
        this.Tags = tags;
    }

    /// <summary>
    /// A comma-delimited list of one or more tags to remove.
    /// </summary>
    [JsonPropertyName("tags")]
    public string Tags
    {
        get; set;
    }
}

/// <summary>
/// Action to replace all of the tags for an item with the one or more provided tags.
/// </summary>
public class ReplaceTagsAction : PocketBasicAction
{
    public ReplaceTagsAction(long itemId, string tags) : base("tags_replace", itemId)
    {
        this.ItemId = itemId;
        this.Tags = tags;
    }

    /// <summary>
    /// A comma-delimited list of one or more tags to remove.
    /// </summary>
    [JsonPropertyName("tags")]
    public string Tags
    {
        get; set;
    }
}

/// <summary>
/// Action to remove one or more tags from an item.
/// </summary>
public class RemoveTagsAction : PocketBasicAction
{
    public RemoveTagsAction(long itemId, string tags) : base("tags_remove", itemId)
    {
        this.ItemId = itemId;
        this.Tags = tags;
    }

    /// <summary>
    /// A comma-delimited list of one or more tags to remove.
    /// </summary>
    [JsonPropertyName("tags")]
    public string Tags
    {
        get; set;
    }
}

/// <summary>
/// Action to remove all tags from an item.
/// </summary>
public class ClearTagsAction : PocketBasicAction
{
    public ClearTagsAction(long itemId) : base("tags_clear", itemId)
    {
        this.ItemId = itemId;
    }
}

/// <summary>
/// Action to rename a tag. This affects all items with this tag.
/// </summary>
public class RenameTagAction : PocketAction
{
    public RenameTagAction(string oldTag, string newTag) : base("tag_rename")
    {
        this.OldTag = oldTag;
        this.NewTag = newTag;
    }

    /// <summary>
    /// The tag name that will be replaced.
    /// </summary>
    [JsonPropertyName("old_tag")]
    public string OldTag
    {
        get; set;
    }

    /// <summary>
    /// The new tag name that will be added.
    /// </summary>
    [JsonPropertyName("new_tag")]
    public string NewTag
    {
        get; set;
    }
}

/// <summary>
/// Action to delete a tag. This affects all items with this tag.
/// </summary>
public class DeleteTagAction : PocketAction
{
    public DeleteTagAction(string tag) : base("tag_delete")
    {
        this.Tag = tag;
    }

    /// <summary>
    /// The tag name that will be deleted.
    /// </summary>
    [JsonPropertyName("tag")]
    public string Tag
    {
        get; set;
    }
}

#endregion