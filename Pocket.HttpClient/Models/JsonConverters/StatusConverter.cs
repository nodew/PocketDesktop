using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pocket.Sdk;

internal class StatusConverter : JsonConverter<PocketItemStatus>
{
    public override PocketItemStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.GetString())
        {
            case "0":
                return PocketItemStatus.Unread;
            case "1":
                return PocketItemStatus.Archived;
            case "2":
                return PocketItemStatus.ShouldDelete;
            default:
                return PocketItemStatus.ShouldDelete;
        }
    }

    public override void Write(Utf8JsonWriter writer, PocketItemStatus value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
