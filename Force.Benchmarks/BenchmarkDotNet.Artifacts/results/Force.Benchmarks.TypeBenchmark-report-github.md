``` ini

BenchmarkDotNet=v0.11.2, OS=Windows 10.0.17763.503 (1809/October2018Update/Redstone5)
Intel Core i7-7700 CPU 3.60GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.300
  [Host]     : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT


```
|                       Method |       Mean |     Error |    StdDev |     Median |
|----------------------------- |-----------:|----------:|----------:|-----------:|
| ConcurrentDictionaryGetOrAdd |  23.765 ns | 0.5323 ns | 1.4391 ns |  23.339 ns |
|              ActivatorInvoke |  14.655 ns | 0.5502 ns | 1.6136 ns |  14.301 ns |
|                 GetSignature |  14.504 ns | 0.3448 ns | 0.3225 ns |  14.505 ns |
|                   TypeCreate |  60.024 ns | 0.8248 ns | 0.7312 ns |  60.031 ns |
|              ActivatorCreate | 478.759 ns | 5.5268 ns | 4.6152 ns | 477.169 ns |
|                  Constructor |   3.975 ns | 0.1239 ns | 0.1737 ns |   3.905 ns |
