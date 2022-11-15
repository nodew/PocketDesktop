using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pocket.Core;

internal class DictionaryConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>> where TKey: notnull
{
    public override Dictionary<TKey, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return (Dictionary<TKey, TValue>?)JsonSerializer.Deserialize(ref reader, typeToConvert, options);
        }

        var _ = reader.Read();
        return default;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
