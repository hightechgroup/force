using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Demo.WebApp.Domain;
using Force.AutoMapper;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace Demo.WebApp.Features.Blog
{
    public class PostListQuery
        : IQuery<Task<IEnumerable<PostListDto>>>
        , IFilter<PostListDto>
        , IFilter<Post>
        , ISorter<PostListDto>
        , IPaging
    {
        private Spec<PostListDto> _spec;

        public int Page { get; set; } = 1;

        public int Take { get; set; } = 10;
        
        Spec<PostListDto> IFilter<PostListDto>.Spec => _spec ?? (_spec = new Spec<PostListDto>(x => true));

        Spec<Post> IFilter<Post>.Spec => CanBePublishedSpec<Post>.Published;

        private ISorter<PostListDto> _sorter = new Sorter<PostListDto, int>(x => x.Id);

        public IOrderedQueryable<PostListDto> Sort(IQueryable<PostListDto> queryable)
            => _sorter.Sort(queryable);

    }
}