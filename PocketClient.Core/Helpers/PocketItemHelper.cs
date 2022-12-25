using PocketClient.Core.Models;
using PocketClient.HttpSdk;

using PocketItem = PocketClient.Core.Models.PocketItem;
using RawPocketItem = PocketClient.HttpSdk.PocketItem;

namespace PocketClient.Core.Helpers;

public static class PocketItemHelper
{
    public static PocketItem NormalizeRawPocketItem(RawPocketItem item)
    {
        var itemUrl = item.ResolvedUrl ?? item.GivenUrl;
        var itemDomain = item.DomainMetadata?.Name ?? itemUrl.Host;
        var itemTitle = item.ResolvedTitle ?? item.GivenTitle ?? itemDomain;

        return new PocketItem()
        {
            Id = item.ItemId,
            Title = itemTitle,
            Url = itemUrl,
            Excerpt = item.Excerpt,
            IsFavorited = item.Favorite,
            IsArchived = item.Status == PocketItemStatus.Archived,
            TimeAdded = item.TimeAdded,
            TimeUpdated = item.TimeUpdated,
            TimeFavorited = item.TimeFavorited,
            TimeRead = item.TimeRead,
            HasImage = item.HasImage > 0,
            TopImageUrl = item.TopImageUrl,
            WordCount = item.WordCount,
            TimeToRead = item.TimeToRead ?? 0,
            Lang = item.Lang,
            Domain = itemDomain,
            Type = GetItemType(item),
            Tags = GetItemTags(item),
            Authors = GetAuthors(item)
        };
    }

    public static List<Tag> GetItemTags(RawPocketItem item)
    {
        var tags = new List<Tag>();

        if (item.Tags == null)
        {
            return tags;
        }

        foreach (var tag in item.Tags)
        {
            tags.Add(new Tag()
            {
                Name = tag.Value.Tag,
            });
        }

        return tags;
    }

    public static List<Author> GetAuthors(RawPocketItem item)
    {
        var authors = new List<Author>();

        if (item.Authors == null)
        {
            return authors;
        }

        foreach (var author in item.Authors)
        {
            authors.Add(
                new Author()
                {
                    Id = author.Value.AuthorId,
                    Name = author.Value.Name,
                    Url = author.Value.Url,
                });
        }

        return authors;
    }

    public static ItemType GetItemType(RawPocketItem item)
    {
        if (item.HasVideo == 2)
        {
            return ItemType.Video;
        }

        if (item.HasImage == 2)
        {
            return ItemType.Image;
        }

        return ItemType.Article;
    }
}
