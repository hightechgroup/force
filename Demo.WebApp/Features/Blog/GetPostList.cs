using Force.Cqrs;
using Force.Linq;

namespace Demo.WebApp.Features.Blog
{
    public class GetPostList: FilterQuery<PostListItem>
    {
        [SearchBy(SearchKind.Contains)]
        public string Name { get; set; }


    }
}