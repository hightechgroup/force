using CostEffectiveCode.Cqrs;

namespace CostEffectiveCode.Tests.Stubs
{
    public class SimpleCommandHandler : IHandler<string, string>
    {
        public string Handle(string input)
        {
            return input;
        }
    }

    public class SimpleCommandHandler2 : IHandler<string>
    {
        public void Handle(string input)
        {
        }
    }
}
