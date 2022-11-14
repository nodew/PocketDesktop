using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pocket.Sdk;

internal class IntegerConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var content = reader.GetString();

        if (int.TryParse(content, out var value))
        {
            return value;
        }

        return 0;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}