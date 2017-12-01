``` ini

BenchmarkDotNet=v0.10.10, OS=Windows 10 Redstone 2 [1703, Creators Update] (10.0.15063.726)
Processor=Intel Core i7-7700 CPU 3.60GHz (Kaby Lake), ProcessorCount=8
Frequency=3515622 Hz, Resolution=284.4447 ns, Timer=TSC
.NET Core SDK=2.0.3
  [Host]     : .NET Core 2.0.3 (Framework 4.6.25815.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.3 (Framework 4.6.25815.02), 64bit RyuJIT


```
|                 Method |          Mean |         Error |        StdDev |
|----------------------- |--------------:|--------------:|--------------:|
|                Compile |  45,464.56 ns |   363.2867 ns |   339.8186 ns |
|            AsQueryable |      72.32 ns |     0.3044 ns |     0.2201 ns |
|              ListWhere |     170.17 ns |     0.5104 ns |     0.4775 ns |
| AsQueryableWhereToList | 322,938.13 ns | 4,009.5173 ns | 3,750.5046 ns |
