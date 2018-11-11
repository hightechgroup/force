``` ini

BenchmarkDotNet=v0.11.2, OS=Windows 10.0.17134.345 (1803/April2018Update/Redstone4)
Intel Core i7-4710HQ CPU 2.50GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2435778 Hz, Resolution=410.5464 ns, Timer=TSC
.NET Core SDK=2.1.301
  [Host]     : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT


```
|   Method |       Mean |    Error |    StdDev |
|--------- |-----------:|---------:|----------:|
|   Dapper |   724.7 us | 13.92 us |  17.61 us |
|       Ef | 1,089.4 us | 41.54 us | 120.50 us |
| Compiled |   130.2 us | 13.46 us |  38.84 us |
