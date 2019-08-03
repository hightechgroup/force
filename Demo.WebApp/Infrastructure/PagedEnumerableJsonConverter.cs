using System;
using Force.Linq.Pagination;
using Newtonsoft.Json;

namespace Demo.WebApp.Infrastructure
{
    
    public class PagedEnumerableJsonConverter: JsonConverter<PagedEnumerable>
    {
        public override void WriteJson(JsonWriter writer, PagedEnumerable value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new {Items = value.Items, Total = value.Total});
        }

        public override PagedEnumerable ReadJson(JsonReader reader, Type objectType, PagedEnumerable existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}