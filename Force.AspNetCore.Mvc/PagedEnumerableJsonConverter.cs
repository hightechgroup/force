using System;
using Force.Linq.Pagination;
using Newtonsoft.Json;

namespace Force.AspNetCore.Mvc
{
    
    public class PagedEnumerableJsonConverter: JsonConverter<PagedEnumerable>
    {
        public override void WriteJson(JsonWriter writer, PagedEnumerable value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new {value.Items, value.Total});
        }

        public override PagedEnumerable ReadJson(JsonReader reader, Type objectType, PagedEnumerable existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}