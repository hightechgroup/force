using System;
using System.Collections.Concurrent;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Force.Ddd;
using Force.Reflection;

namespace Force.Benchmarks
{
    public class SimpeValueObject : ValueObject<string>
    {
        public SimpeValueObject(string value) : base(value)
        {
        }
    }
    
    //[SimpleJob(RunStrategy.Monitoring, launchCount: 10, warmupCount: 3, targetCount: 100)]
    public class TypeBenchmark
    {
        static string _val = Type<SimpeValueObject>.CreateInstance("string");

        private static object[] _pars = {"string"};

        private long _signature = Type<SimpeValueObject>.GetSignature(_pars);

        private static ConcurrentDictionary<long, ObjectActivator<SimpeValueObject>> _activators
            = new ConcurrentDictionary<long, ObjectActivator<SimpeValueObject>>();

        [Benchmark]
        public void PublicProperties_Type()
        {
            var properties = typeof(string).GetProperties(BindingFlags.Public
                                         | BindingFlags.GetProperty
                                         | BindingFlags.SetProperty);
        }
        
        [Benchmark]
        public void PublicProperties_TypeT()
        {
            var properties = Type<string>.PublicProperties;
        }
        
        
        [Benchmark]
        public void TypeCreate()
        {
            Type<SimpeValueObject>.CreateInstance("string");
        }
        
        [Benchmark]
        public void ActivatorCreate()
        {
            Activator.CreateInstance(typeof(SimpeValueObject), "string");
        }

        //[Benchmark]
        public void Constructor()
        {
            new SimpeValueObject("string");
        }
    }
}