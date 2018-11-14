using System.Collections.Generic;
using Force;

namespace Demo.WebApp.Features.Posts
{
    public class ImportPostsHandler: IHandler<IEnumerable<ImportPost>>
    {
        public void Handle(IEnumerable<ImportPost> input)
        {
        }
    }
}