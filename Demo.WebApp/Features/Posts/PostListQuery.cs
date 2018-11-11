using System.Linq;
using Force.Cqrs;
using Force.Ddd;
using Force.Ddd.Pagination;

namespace Demo.WebApp.Features.Posts
{
    public class PostListQuery
        : IQuery<PagedResponse<PostListDto>>
        , IFilter<PostListDto>
        , ISorter<PostListDto>
        , IPaging
    {
        private Spec<PostListDto> _spec;
        
        public int Page { get; set; }
        
        public int Take { get; set; }
        
        public Spec<PostListDto> Spec => _spec ?? (_spec = new Spec<PostListDto>(x => true));

        private ISorter<PostListDto> _sorter = new Sorter<PostListDto, int>(x => x.Id);
        
        public IOrderedQueryable<PostListDto> Sort(IQueryable<PostListDto> queryable)
            => _sorter.Sort(queryable);
    }
}