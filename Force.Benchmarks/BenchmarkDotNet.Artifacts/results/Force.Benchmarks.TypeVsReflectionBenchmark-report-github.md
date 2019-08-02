``` ini

BenchmarkDotNet=v0.11.2, OS=Windows 10.0.17763.615 (1809/October2018Update/Redstone5)
Intel Core i7-7700 CPU 3.60GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.300
  [Host]     : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT


```
|     Method |         Mean |       Error |      StdDev |
|----------- |-------------:|------------:|------------:|
|       Type |     2.133 ns |   0.0714 ns |   0.0877 ns |
| Reflection | 5,588.331 ns | 131.0387 ns | 365.2834 ns |
