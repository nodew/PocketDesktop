using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pocket.Sdk;

internal class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (long.TryParse(reader.GetString(), out var value))
        {
            return DateTimeOffset.FromUnixTimeSeconds(value).DateTime.ToUniversalTime();
        }

        return DateTime.MinValue.ToUniversalTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
