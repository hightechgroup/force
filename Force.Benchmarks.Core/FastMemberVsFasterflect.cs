using System;
using BenchmarkDotNet.Attributes;
using Fasterflect;
using FastMember;

namespace Force.Benchmarks.Core
{
    public class FastMemberVsFasterflect
    {
        private static object Constructor = Subject.GetType().CreateInstance();
        
        private TypeAccessor Ta = TypeAccessor.Create(typeof(CanBeActive));

        private static CanBeActive Subject = new CanBeActive(true);

        [Benchmark]
        public void Fasterflect_CreateInstance()
        {
            typeof(CanBeActive).CreateInstance(true);
        }
        
        [Benchmark]
        public void TypeAccessor_Create()
        {
            TypeAccessor.Create(typeof(CanBeActive));
        }

        [Benchmark]
        public void TypeAccessor_GetProperty()
        {
            var a = Ta[Subject, "IsActive"];
        }

        [Benchmark]
        public void Fasterflect_GetPropertyValue()
        {
            var val = Subject.GetPropertyValue("IsActive");
        }
    }
}