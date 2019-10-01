using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.Extensions;
using Demo.WebApp.Domain;
using Demo.WebApp.Domain.Entities.Blog;
using Demo.WebApp.Infrastructure;
using Force.AutoMapper;
using Force.Ddd;
using Newtonsoft.Json;

namespace Demo.WebApp.Features.Blog
{
    [Display(Name = "Tada")]
    [Projection(typeof(Post))]
    public class PostListItem : HasIdBase
    {
        public string Title { get; set; }

        [JsonIgnore] 
        public DateTime Created { get; set; }

        [JsonIgnore] 
        public DateTime? LastUpdated { get; set; }

        public string SubTitle => LastUpdated.HasValue
            ? $"{Created.ToString(DateTimeFormats.Default)} / {LastUpdated.Value.ToString(DateTimeFormats.Default)}"
            : Created.ToString(DateTimeFormats.Default);

        public static void CreateMap(IMappingExpression<Post, PostListItem> mappingExpression)
            => mappingExpression
                .ForMember(x => x.Title, o => o.MapFrom(x => $"{x.Hub.Name} / {x.Name}"));
    }
}