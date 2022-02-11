namespace Pidp.Infrastructure;

using System.Text.Json;
using System.Text.Json.Serialization;

public class PolymorphicDictionarySerializer<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>> where TKey : notnull where TValue : class
{
    public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();

    public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value.ToDictionary(item => item.Key, item => (object)item.Value), options);
}
