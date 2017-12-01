using System;
using BenchmarkDotNet.Running;

namespace Force.Benchmarks.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FastMemberVsFasterflect>();
            Console.ReadLine();
        }
    }
}