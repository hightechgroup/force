using Force.Cqrs;
using Force.Linq;

namespace Demo.WebApp.Features.Blog
{
    public class GetPostList: FilterQuery<PostListItem>
    {
        [SearchBy(SearchKind.Contains)]
        [SearchAnywhere]
        public string Name { get; set; }


    }
}