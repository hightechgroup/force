using System;
using Force.Ddd.Pagination;
using Newtonsoft.Json;

namespace Demo.WebApp.Infrastructure
{
    public class PagedEnumerableJsonConverter<T>: JsonConverter<PagedEnumerable<T>>
    {
        public override void WriteJson(JsonWriter writer, PagedEnumerable<T> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override PagedEnumerable<T> ReadJson(JsonReader reader, Type objectType, PagedEnumerable<T> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}