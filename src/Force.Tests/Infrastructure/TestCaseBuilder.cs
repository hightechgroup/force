using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Force.Tests.Infrastructure
{
    public static class TestCaseBuilder
    {
        public static TestCaseBuilder<TIn, TOut> For<TIn, TOut>() => new();
            
    }

    public class TestCaseBuilder<TIn, TOut>: IEnumerable<object[]>
    {
        List<TestCase<TIn, TOut>> testCases = new();

        public TestCaseBuilder<TIn, TOut> Add(TIn input, Func<TOut, bool> assert, string errorMessage = null)
        {
            testCases.Add(new TestCase<TIn, TOut>(input, assert, errorMessage));
            return this;
        }
        
        private IEnumerator<object[]> DoGetEnumerator()
            => testCases
                .Select(x => new object[] {x})
                .ToList()
                .GetEnumerator();

        public IEnumerator<object[]> GetEnumerator()
            => DoGetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => DoGetEnumerator();
    }
}