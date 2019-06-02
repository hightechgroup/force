using System;
using System.Collections.Generic;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Force.Benchmarks
{
    public class DomainEventDispatcher: IHandler<object>
    {
        private Dictionary<Type, IEnumerable<dynamic>> _handlers
            = new Dictionary<Type, IEnumerable<dynamic>>();
        
        public void Handle(object input)
        {
            var type = input.GetType();
            if (!_handlers.ContainsKey(type)) return;
            
            foreach (var handler in _handlers[type])
            {
                handler.Invoke(input);
            }
        }
    }
    
    public interface IA
    {
        void Invoke(string str);
        void Invoke(object obj);
    }

    public class A : IA
    {
        public void Invoke(string str)
        {
            Thread.Sleep(100);
        }
        
        public void Invoke(object obj)
        {
            Thread.Sleep(1000);
        }
    }
    
    public class DynamicVsCallBenchmarks
    {
        readonly List<IA> _list = new List<IA>()
        {
            new A(),
            new A(),
            new A(),
            new A(),
            new A(),
            new A(),
            new A(),
            new A(),
            new A(),
            new A()
        };

        private DomainEventDispatcher _dispatcher = new DomainEventDispatcher();

        [Benchmark]
        public void Dispatcher()
        {
            _dispatcher.Handle("123");
        }
        
        //[Benchmark]
        public void Dynamic()
        {         
            foreach (dynamic l in _list)
            {
                l.Invoke((dynamic)"test");
            }   
        }
        
        //[Benchmark]
        public void Simplecall()
        {
            foreach (var l in _list)
            {
                l.Invoke("test");
            } 
        }
    }
}