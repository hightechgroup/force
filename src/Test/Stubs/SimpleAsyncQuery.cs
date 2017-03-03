using System.Threading.Tasks;
using CostEffectiveCode.Cqrs;

namespace CostEffectiveCode.Tests.Stubs
{
    public class SimpleAsyncQuery : IAsyncQuery<string, string>
    {
        public async Task<string> Ask(string spec)
        {
            return await Task.FromResult(spec);
        }
    }
}
