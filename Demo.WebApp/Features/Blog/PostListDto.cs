using System;
using AutoMapper;
using Demo.WebApp.Domain;
using Demo.WebApp.Infrastructure;
using Force.Ddd;
using Newtonsoft.Json;

namespace Demo.WebApp.Features.Blog
{
    public class PostListDto : HasIdBase
    {
        public string Title { get; set; }

        [JsonIgnore] 
        public DateTime Created { get; set; }

        [JsonIgnore] 
        public DateTime? LastUpdated { get; set; }

        public string SubTitle => LastUpdated.HasValue
            ? $"{Created.ToString(DateTimeFormats.Default)} / {LastUpdated.Value.ToString(DateTimeFormats.Default)}"
            : Created.ToString(DateTimeFormats.Default);

        public static void CreateMap(IMappingExpression<Post, PostListDto> mappingExpression)
            => mappingExpression
                .ForMember(x => x.Title, o => o.MapFrom(x => $"{x.Hub.Name} / {x.Name}"));
    }
}