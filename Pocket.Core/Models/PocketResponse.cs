using System.Text.Json.Serialization;

namespace Pocket.Core;

public class PocketResponse
{
    [JsonPropertyName("status")]
    public int Status
    {
        get; set;
    }
}

public class PocketAddItemResponse : PocketResponse
{
    [JsonPropertyName("item")]
    public PocketItem Item
    {
        get; set;
    }
}

public class PocketRetriveItemResponse : PocketResponse
{
    [JsonPropertyName("list")]
    [JsonConverter(typeof(DictionaryConverter<string, PocketItem>))]
    public Dictionary<string, PocketItem> List
    {
        get; set;
    }

    [JsonPropertyName("tags")]
    public List<string> Tags
    {
        get; set;
    }
}

public class PocketUpdateResponse<T> : PocketResponse
{
    [JsonPropertyName("action_results")]
    public List<T> ActionResults
    {
        get; set;
    }
}

public class PocketTokenResponse
{
    [JsonPropertyName("code")]
    public string Code
    {
        get; set;
    }
}

public class PocketAuthorizeResponse
{
    [JsonPropertyName("username")]
    public string Username
    {
        get; set;
    }

    [JsonPropertyName("access_token")]
    public string AccessToken
    {
        get; set;
    }
}
