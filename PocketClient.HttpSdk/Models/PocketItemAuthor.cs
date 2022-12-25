using System.Text.Json.Serialization;

namespace PocketClient.HttpSdk;

public class PocketItemAuthor
{
    [JsonPropertyName("item_id")]
    [JsonConverter(typeof(LongIntegerConverter))]
    public long ItemId
    {
        get; set;
    }

    [JsonPropertyName("author_id")]
    [JsonConverter(typeof(IntegerConverter))]
    public int AuthorId
    {
        get; set;
    }

    [JsonPropertyName("name")]
    public string Name
    {
        get; set;
    }

    [JsonPropertyName("url")]
    public string Url
    {
        get; set;
    }
}
