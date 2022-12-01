using System.Text.Json.Serialization;

namespace PocketClient.HttpSdk;

public class PocketItem
{
    [JsonPropertyName("item_id")]
    [JsonConverter(typeof(LongIntegerConverter))]
    public long ItemId
    {
        get; set;
    }

    [JsonPropertyName("resolved_id")]
    [JsonConverter(typeof(LongIntegerConverter))]
    public long ResolvedId
    {
        get; set;
    }

    [JsonPropertyName("given_url")]
    public Uri GivenUrl
    {
        get; set;
    }

    [JsonPropertyName("given_title")]
    public string? GivenTitle
    {
        get; set;
    }

    [JsonPropertyName("favorite")]
    [JsonConverter(typeof(BooleanConverter))]
    public bool Favorite
    {
        get; set;
    }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(StatusConverter))]
    public PocketItemStatus Status
    {
        get; set;
    }

    [JsonPropertyName("time_added")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TimeAdded
    {
        get; set;
    }

    [JsonPropertyName("time_updated")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TimeUpdated
    {
        get; set;
    }

    [JsonPropertyName("time_read")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TimeRead
    {
        get; set;
    }

    [JsonPropertyName("time_favorited")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TimeFavorited
    {
        get; set;
    }

    [JsonPropertyName("sort_id")]
    public long SortId
    {
        get; set;
    }

    [JsonPropertyName("resolved_title")]
    public string? ResolvedTitle
    {
        get; set;
    }

    [JsonPropertyName("resolved_url")]
    public Uri? ResolvedUrl
    {
        get; set;
    }

    [JsonPropertyName("excerpt")]
    public string? Excerpt
    {
        get; set;
    }

    [JsonPropertyName("is_article")]
    [JsonConverter(typeof(BooleanConverter))]
    public bool IsArticle
    {
        get; set;
    }

    [JsonPropertyName("is_index")]
    [JsonConverter(typeof(BooleanConverter))]
    public bool IsIndex
    {
        get; set;
    }

    [JsonPropertyName("has_video")]
    [JsonConverter(typeof(IntegerConverter))]
    public int HasVideo
    {
        get; set;
    }

    [JsonPropertyName("has_image")]
    [JsonConverter(typeof(IntegerConverter))]
    public int HasImage
    {
        get; set;
    }

    [JsonPropertyName("word_count")]
    [JsonConverter(typeof(IntegerConverter))]
    public int WordCount
    {
        get; set;
    }

    [JsonPropertyName("lang")]
    public string? Lang
    {
        get; set;
    }

    [JsonPropertyName("amp_url")]
    public Uri? AmpUrl
    {
        get; set;
    }

    [JsonPropertyName("top_image_url")]
    public Uri? TopImageUrl
    {
        get; set;
    }

    [JsonPropertyName("authors")]
    [JsonConverter(typeof(DictionaryConverter<string, PocketItemAuthor>))]
    public Dictionary<string, PocketItemAuthor>? Authors
    {
        get; set;
    }

    [JsonPropertyName("images")]
    [JsonConverter(typeof(DictionaryConverter<string, Image>))]
    public Dictionary<string, Image> Images
    {
        get; set;
    }

    [JsonPropertyName("domain_metadata")]
    public DomainMetadata? DomainMetadata
    {
        get; set;
    }

    [JsonPropertyName("listen_duration_estimate")]
    public int ListenDurationEstimate
    {
        get; set;
    }

    [JsonPropertyName("time_to_read")]
    public int? TimeToRead
    {
        get; set;
    }

    [JsonPropertyName("videos")]
    [JsonConverter(typeof(DictionaryConverter<string, Video>))]
    public Dictionary<string, Video> Videos
    {
        get; set;
    }

    [JsonPropertyName("badge_group_id")]
    public string? BadgeGroupId
    {
        get; set;
    }

    [JsonPropertyName("tags")]
    [JsonConverter(typeof(DictionaryConverter<string, PocketItemTag>))]
    public Dictionary<string, PocketItemTag> Tags
    {
        get; set;
    }
}

public class DomainMetadata
{
    [JsonPropertyName("name")]
    public string? Name
    {
        get; set;
    }

    [JsonPropertyName("logo")]
    public Uri Logo
    {
        get; set;
    }

    [JsonPropertyName("greyscale_logo")]
    public Uri GreyscaleLogo
    {
        get; set;
    }
}

public class Image
{
    [JsonPropertyName("item_id")]
    [JsonConverter(typeof(LongIntegerConverter))]
    public long ItemId
    {
        get; set;
    }

    [JsonPropertyName("image_id")]
    [JsonConverter(typeof(LongIntegerConverter))]
    public long ImageId
    {
        get; set;
    }

    [JsonPropertyName("src")]
    public Uri Src
    {
        get; set;
    }

    [JsonPropertyName("width")]
    [JsonConverter(typeof(IntegerConverter))]
    public int Width
    {
        get; set;
    }

    [JsonPropertyName("height")]
    [JsonConverter(typeof(IntegerConverter))]
    public int Height
    {
        get; set;
    }

    [JsonPropertyName("credit")]
    public string Credit
    {
        get; set;
    }

    [JsonPropertyName("caption")]
    public string Caption
    {
        get; set;
    }
}

public class Video
{
    [JsonPropertyName("item_id")]
    [JsonConverter(typeof(LongIntegerConverter))]
    public long ItemId
    {
        get; set;
    }

    [JsonPropertyName("video_id")]
    [JsonConverter(typeof(LongIntegerConverter))]
    public long VideoId
    {
        get; set;
    }

    [JsonPropertyName("src")]
    public Uri Src
    {
        get; set;
    }

    [JsonPropertyName("width")]
    [JsonConverter(typeof(IntegerConverter))]
    public int Width
    {
        get; set;
    }

    [JsonPropertyName("height")]
    [JsonConverter(typeof(IntegerConverter))]
    public int Height
    {
        get; set;
    }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(IntegerConverter))]
    public int Type
    {
        get; set;
    }

    [JsonPropertyName("vid")]
    public string Vid
    {
        get; set;
    }

    [JsonPropertyName("length")]
    [JsonConverter(typeof(IntegerConverter))]
    public int Length
    {
        get; set;
    }
}

public class PocketItemTag
{
    [JsonPropertyName("item_id")]
    [JsonConverter(typeof(LongIntegerConverter))]
    public long ItemId
    {
        get; set;
    }

    [JsonPropertyName("tag")]
    public string Tag
    {
        get; set;
    }
}