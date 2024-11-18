using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.helpers
{
    public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var dateString = reader.GetString();
                if (string.IsNullOrWhiteSpace(dateString))
                {
                    return default;
                }

                if (DateOnly.TryParse(dateString, out var date))
                {
                    return date;
                }

                throw new JsonException("Invalid DateOnly format.");
            }

            throw new JsonException("Invalid token type for DateOnly.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd")); // Use a clear format for DateOnly.
        }
    }

}