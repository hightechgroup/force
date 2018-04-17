using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Force.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Force.Meta
{
    public static class MetaProvider<T>
    {
        public static readonly string Title =
            FastTypeInfo<T>.GetCustomAttribute<DisplayAttribute>()?.Name
            ?? typeof(T).ToString();

        private static string Format(string str) => str;
        
        public static readonly FormMetaInfo Form = new FormMetaInfo(FastTypeInfo<T>
                .PublicProperties
                .Select(x => new FormItemMetaInfo()
                {
                    Hidden = FastTypeInfo<T>.HasAttribute<HiddenInputAttribute>(),
                    Required = FastTypeInfo<T>.HasAttribute<RequiredAttribute>(),
                    ReadOnly = FastTypeInfo<T>.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? false,
                    Pattern = FastTypeInfo<T>.GetCustomAttribute<RegularExpressionAttribute>()?.Pattern,
                    MaxLength = FastTypeInfo<T>.GetCustomAttribute<MaxLengthAttribute>()?.Length ?? 0,
                    Mask = FastTypeInfo<T>.GetCustomAttribute<MaskAttribute>()?.Mask
                })
                .ToArray())
        {
            Title = Title,          
        };
        
        public static readonly ListMetaInfo List = new ListMetaInfo(FastTypeInfo<T>
            .PublicProperties
            .Select(x => new ListItemMetaInfo()
            {
            })
            .ToArray())
        {
            Title = Title,          
        };
    }
}