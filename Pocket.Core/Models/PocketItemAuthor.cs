using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pocket.Core;

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
