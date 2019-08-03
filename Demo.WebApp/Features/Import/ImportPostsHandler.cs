using System.Collections.Generic;
using Force;
using Force.Cqrs;

namespace Demo.WebApp.Features.Import
{
    public class ImportPostsHandler: IHandler<IEnumerable<ImportPost>>
    {
        public void Handle(IEnumerable<ImportPost> input)
        {
        }
    }
}