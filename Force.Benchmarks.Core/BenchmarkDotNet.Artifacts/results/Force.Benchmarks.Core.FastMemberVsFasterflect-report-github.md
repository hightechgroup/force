``` ini

BenchmarkDotNet=v0.10.10, OS=Windows 10 Redstone 2 [1703, Creators Update] (10.0.15063.726)
Processor=Intel Core i7-7700 CPU 3.60GHz (Kaby Lake), ProcessorCount=8
Frequency=3515622 Hz, Resolution=284.4447 ns, Timer=TSC
.NET Core SDK=2.0.3
  [Host]     : .NET Core 2.0.3 (Framework 4.6.25815.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.3 (Framework 4.6.25815.02), 64bit RyuJIT


```
|                       Method |      Mean |     Error |    StdDev |
|----------------------------- |----------:|----------:|----------:|
|          TypeAccessor_Create |  28.00 ns | 0.3545 ns | 0.3143 ns |
|     TypeAccessor_GetProperty |  27.35 ns | 0.2287 ns | 0.2139 ns |
| Fasterflect_GetPropertyValue | 101.64 ns | 2.5569 ns | 3.7478 ns |
