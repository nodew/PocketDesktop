using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pocket.Core;

internal class LongIntegerConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var content = reader.GetString();

        if (long.TryParse(content, out var value))
        {
            return value;
        }

        return 0;
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
