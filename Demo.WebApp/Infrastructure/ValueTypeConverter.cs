using System;
using Demo.WebApp.Domain;
using Force.Ddd;
using Force.Infrastructure;
using Newtonsoft.Json;

namespace Demo.WebApp.Infrastructure
{
    public class ValueTypeConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {            
            writer.WriteValue(value?.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            ValueObject.TryParse(reader.Value?.ToString(), out var value);
            return value;
        }

        public override bool CanConvert(Type objectType)
            => objectType.IsAssignableToGenericType(typeof(ValueObject<>));
    }
}