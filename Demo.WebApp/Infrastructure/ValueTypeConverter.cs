using System;
using Demo.WebApp.Domain;
using Newtonsoft.Json;

namespace Demo.WebApp.Infrastructure
{
    public class ValueTypeConverter: JsonConverter
    {
        public ValueTypeConverter()
        {
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {            
            writer.WriteValue(value?.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Email.TryParse(reader.Value?.ToString(), out var email);
            return email;
        }

        public override bool CanConvert(Type objectType)
            => true;
    }
}