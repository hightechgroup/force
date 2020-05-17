using System;

namespace Force.Tests.Infrastructure
{
    public class TestCase<TIn, TOut>
    {
        private Func<TOut, bool> _assertFunc;

        public TestCase(TIn input, Func<TOut, bool> assertFunc, string errorMessage = null)
        {
            Input = input;
            _assertFunc = assertFunc ?? throw new ArgumentNullException(nameof(assertFunc));
            ErrorMessage = errorMessage;
        }
        
        public static implicit operator TestCase<TIn, TOut>(Tuple<TIn, Func<TOut, bool>> tuple)
            => new TestCase<TIn, TOut>(tuple.Item1, tuple.Item2);

        public TIn Input { get; }
        
        private string ErrorMessage { get; }
        
        public void Assert(TOut output)
        {
            var flag = _assertFunc(output);
            Xunit.Assert.True(flag, ErrorMessage);
        }

    }
}