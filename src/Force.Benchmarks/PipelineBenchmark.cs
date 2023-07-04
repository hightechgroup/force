using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.Extensions.DependencyInjection;
using Force.Cqrs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Force.Benchmarks
{
    // [SimpleJob(RunStrategy.Monitoring, launchCount: 10, warmupCount: 3, targetCount: 100)]
    [MemoryDiagnoser]
    public class PipelineBenchmark
    {
        private IServiceProvider _serviceProvider;
        private Controller2 _controller;
        private MyCommand _command = new MyCommand();
        
        
        [GlobalSetup]
        public void GlobalSetup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ICommandHandler<MyCommand, string>, MyHandler>();
            serviceCollection.AddSingleton<Pipeline>();
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _controller = new Controller2()
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        RequestServices = _serviceProvider
                    }
                }
            };
        }


        [Benchmark]
        public void Empty()
        {
            var a = 3;
        }
        
        [Benchmark]
        public void RunPipeline()
        {
            _controller.Run(_command);
        }

        [Benchmark]
        public void RunHandler()
        {
            Handle();
        }

        private ActionResult Handle()
        {
            return new ObjectResult(_serviceProvider.GetRequiredService<ICommandHandler<MyCommand, string>>().Handle(_command));
        }
    }

    public class Controller2 : ControllerBase
    {
        public ActionResult Run(MyCommand command) => this.RunPipeline(command);
    }

    public class MyHandler: ICommandHandler<MyCommand, string>
    {
        public string Handle(MyCommand input)
        {
            var j = 0;
            for (var i = 0; i < 1_000; i++)
            {
                j = i;
            }

            return string.Empty;
        }
    }
}