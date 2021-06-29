# Enhanced-XOR-Encryption-System

This is a class that combines XOR bit operations with SHA512 hashing to provide a simple but efficient way to encrypt
files and strings.

## How it works

First, it uses the SHA512 algorithm provided by the standard library to extract the hash of a given key. Then, it
repeats the key to match the input length. After, it performs the XOR (exclusive or) operation with each byte of the
input and its corresponding byte in the key. The advantage of XOR operations is that they work for encryption and
decryption. For example:

 ```
   00100111 (input byte)
  ^11010011 (key byte)
  =11110100 (result byte)
  
   11110100 (input byte)
  ^11010011 (key byte)
  =00100111 (result byte)
 ```

This class includes methods for working in parallel in the CPU. This speeds up the process for giant inputs, but it's not 
adequate in every case.

## Available Methods
* `string EncodeString(string str)`
* `string EncodeStringParallel(string str)`
* `void EncodeFile(string filepath)`
* `void EncodeFileParallel(string filepath)`

## How to use the Demo Program

It is a command-line program. You need to pass 3 arguments: mode, data, and key. Currently, the available modes are
file (reads data from the specified file path) and string (reads from the next argument). File mode is recommended
because the console can't show some characters. This Demo Program uses Parallel methods by default.
`DemoProgram.exe [mode] [filepath/stringToEncode] [key]`
Examples:

* `DemoProgram.exe file SecretFile.txt 123456`
* `DemoProgram.exe string "Super Secret Text" 123456`

## Performance
Benchmark of the methods to encrypt strings, using BenchmarkDotNet:

```
BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19041.1052 (2004/May2020Update/20H1)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
[Host]     : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT
DefaultJob : .NET 5.0.6 (5.0.621.22011), X64 RyuJIT


|                Method | NumberOfBytes |          Mean |      Error |     StdDev |        Median |
|---------------------- |-------------- |--------------:|-----------:|-----------:|--------------:|
|         EncryptString |           100 |      2.251 us |  0.0083 us |  0.0078 us |      2.248 us |
| EncryptStringParallel |           100 |      6.395 us |  0.1190 us |  0.1114 us |      6.424 us |
|         EncryptString |          1000 |     11.679 us |  0.0804 us |  0.0752 us |     11.688 us |
| EncryptStringParallel |          1000 |     13.662 us |  0.1870 us |  0.1658 us |     13.668 us |
|         EncryptString |         10000 |     97.059 us |  0.5113 us |  0.4783 us |     97.056 us |
| EncryptStringParallel |         10000 |    127.730 us |  2.5113 us |  5.1300 us |    125.550 us |
|         EncryptString |        100000 |  1,129.565 us |  3.3734 us |  3.1555 us |  1,130.119 us |
| EncryptStringParallel |        100000 |  1,072.097 us | 14.8146 us | 13.8576 us |  1,075.960 us |
|         EncryptString |       1000000 | 11,400.657 us | 45.0290 us | 42.1202 us | 11,402.378 us |
| EncryptStringParallel |       1000000 |  9,114.643 us | 35.5433 us | 29.6802 us |  9,111.477 us |

// * Hints *
Outliers
EncryptionBenchmark.EncryptParallel: Default -> 1 outlier  was  removed (14.14 us)
EncryptionBenchmark.EncryptParallel: Default -> 6 outliers were removed (142.47 us..144.18 us)
EncryptionBenchmark.EncryptParallel: Default -> 2 outliers were removed (9.25 ms, 9.91 ms)

// * Legends *
NumberOfBytes : Value of the 'NumberOfBytes' parameter
Mean          : Arithmetic mean of all measurements
Error         : Half of 99.9% confidence interval
StdDev        : Standard deviation of all measurements
Median        : Value separating the higher half of all measurements (50th percentile)
1 us          : 1 Microsecond (0.000001 sec)```
 
 
