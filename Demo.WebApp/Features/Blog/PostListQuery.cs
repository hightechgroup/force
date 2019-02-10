using System.Collections.Generic;
using System.Linq;
using Demo.WebApp.Domain;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace Demo.WebApp.Features.Blog
{
    public class PostListQuery
        : IQuery<IEnumerable<PostListDto>>
        , IQuery<PagedResponse<PostListDto>>
        , IFilter<PostListDto>
        , IFilter<Post>
        , ISorter<PostListDto>
        , IPaging
    {
        private Spec<PostListDto> _spec;

        public int Page { get; set; }
        
        public int Take { get; set; }
        
        public Spec<PostListDto> Spec => _spec ?? (_spec = new Spec<PostListDto>(x => true));

        Spec<Post> IFilter<Post>.Spec => CanBePublishedSpec<Post>.Published;

        private ISorter<PostListDto> _sorter = new Sorter<PostListDto, int>(x => x.Id);

        public IOrderedQueryable<PostListDto> Sort(IQueryable<PostListDto> queryable)
            => _sorter.Sort(queryable);

    }
}