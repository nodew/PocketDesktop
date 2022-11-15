using System.Text.Json.Serialization;

namespace Pocket.Core;

public class PocketRequest
{
}

public class PocketAddItemRequest : PocketRequest
{
    [JsonPropertyName("url")]
    public string Url
    {
        get; set;
    }

    [JsonPropertyName("title")]
    public string? Title
    {
        get; set;
    }

    [JsonPropertyName("tags")]
    public string? Tags
    {
        get; set;
    }

    [JsonPropertyName("tweet_id")]
    public string? TweetId
    {
        get; set;
    }
}

public class PocketUpdateRequest<T> : PocketRequest where T : PocketAction
{
    [JsonPropertyName("actions")]
    public List<T> Actions
    {
        get; set;
    }
}

public class PocketAuthorizeRequest : PocketRequest
{
    [JsonPropertyName("consumer_key")]
    public string ConsumerKey
    {
        get; set;
    }

    [JsonPropertyName("code")]
    public string Code
    {
        get; set;
    }
}

public class PocketTokenRequest : PocketRequest
{
    [JsonPropertyName("consumer_key")]
    public string ConsumerKey
    {
        get; set;
    }

    [JsonPropertyName("redirect_uri")]
    public string RedirectUri
    {
        get; set;
    }
}

