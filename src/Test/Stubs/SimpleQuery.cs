using CostEffectiveCode.Cqrs;

namespace CostEffectiveCode.Tests.Stubs
{
    public class SimpleQuery : IQuery<string, string>
    {
        public string Ask(string spec)
        {
            return spec;
        }
    }
}
