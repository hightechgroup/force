``` ini

BenchmarkDotNet=v0.11.2, OS=macOS Mojave 10.14.2 (18C54) [Darwin 18.2.0]
Intel Core i5-4278U CPU 2.60GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET Core SDK=2.1.502
  [Host]     : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT


```
|     Method |     Mean |     Error |    StdDev |
|----------- |---------:|----------:|----------:|
| Dispatcher | 8.097 ns | 0.2515 ns | 0.2100 ns |
