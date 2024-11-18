using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.helpers
{
    public sealed class GuidJsonConverter : JsonConverter<Guid>
    {
        public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the string value
            var value = reader.GetString();

            // Handle the case where the value is empty or not valid
            if (string.IsNullOrWhiteSpace(value) || !Guid.TryParse(value, out var guid))
            {
                return Guid.Empty;
            }

            return guid;
        }

        public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        {
            // Serialize the Guid as a string
            writer.WriteStringValue(value.ToString());
        }
    }
}