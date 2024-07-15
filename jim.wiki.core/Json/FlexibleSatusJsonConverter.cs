using System.Text.Json;
using System.Text.Json.Serialization;

namespace jim.wiki.core.Json
{
    public class FlexibleStatusJsonConverter<T> : JsonConverter<T> where T : struct,Enum
    {
        private readonly bool _writeAsString;

        public FlexibleStatusJsonConverter(bool writeAsString = true)
        {
            _writeAsString = writeAsString;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var intValue = reader.GetInt32();
                if (Enum.IsDefined(typeof(T), intValue))
                {
                    return (T)(object)intValue;
                }
                throw new JsonException($"Invalid value {intValue} for enum {typeof(T)}");
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                var enumString = reader.GetString();
                if (Enum.TryParse<T>(enumString, true, out var result))
                {
                    return result;
                }
                throw new JsonException($"Invalid value {enumString} for enum {typeof(T)}");
            }
            throw new JsonException($"Invalid token type {reader.TokenType} for enum {typeof(T)}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (_writeAsString)
            {
                writer.WriteStringValue(value.ToString());
            }
            else
            {
                writer.WriteNumberValue(Convert.ToInt32(value));
            }
        }
    }
}
