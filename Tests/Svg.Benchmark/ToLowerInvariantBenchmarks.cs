using System;
using BenchmarkDotNet.Attributes;

namespace Svg.Benchmark
{
    public class ToLowerInvariantBenchmarks
    {
        [Benchmark(Baseline = true)]
        public void ToLowerInvariantString()
        {
            var colour = "ActiveBorder".Trim().ToLowerInvariant();
        }

        [Benchmark]
        public void ToLowerInvariantSpan()
        {
            var colour = "ActiveBorder".AsSpan().Trim();
            Span<char> buffer = stackalloc char[colour.Length];
            colour.ToLowerInvariant(buffer);
        }

        [Benchmark]
        public void ToLowerInvariantSpanCustom()
        {
            var colour = "ActiveBorder".AsSpan().Trim();
            
            Span<char> buffer = stackalloc char[colour.Length];

            for (int i = 0; i < colour.Length; i++)
            {
                buffer[i] = char.ToLowerInvariant(colour[i]);
            }
        }

        [Benchmark]
        public void ToLowerInvariantSpanCustomFromString()
        {
            var colour = "ActiveBorder";
 
            Span<char> buffer = stackalloc char[colour.Length];

            for (int i = 0; i < colour.Length; i++)
            {
                buffer[i] = char.ToLowerInvariant(colour[i]);
            }
        }

        public static void ToLowerAscii(in ReadOnlySpan<char> colour, ref Span<char> buffer)
        {
            for (int i = 0; i < colour.Length; i++)
            {
                var c = colour[i];
                switch (c)
                {
                    case 'A': buffer[i] = 'a'; break;
                    case 'B': buffer[i] = 'b'; break;
                    case 'C': buffer[i] = 'c'; break;
                    case 'D': buffer[i] = 'd'; break;
                    case 'E': buffer[i] = 'e'; break;
                    case 'F': buffer[i] = 'f'; break;
                    case 'G': buffer[i] = 'g'; break;
                    case 'H': buffer[i] = 'h'; break;
                    case 'I': buffer[i] = 'i'; break;
                    case 'J': buffer[i] = 'j'; break;
                    case 'K': buffer[i] = 'k'; break;
                    case 'L': buffer[i] = 'l'; break;
                    case 'M': buffer[i] = 'm'; break;
                    case 'N': buffer[i] = 'n'; break;
                    case 'O': buffer[i] = 'o'; break;
                    case 'P': buffer[i] = 'p'; break;
                    case 'Q': buffer[i] = 'q'; break;
                    case 'R': buffer[i] = 'r'; break;
                    case 'S': buffer[i] = 's'; break;
                    case 'T': buffer[i] = 't'; break;
                    case 'U': buffer[i] = 'u'; break;
                    case 'V': buffer[i] = 'v'; break;
                    case 'W': buffer[i] = 'w'; break;
                    case 'X': buffer[i] = 'x'; break;
                    case 'Y': buffer[i] = 'y'; break;
                    case 'Z': buffer[i] = 'z'; break;
                    default: buffer[i] = c; break;
                }
            }
        }

        public static void ToLowerAscii(string colour, ref Span<char> buffer)
        {
            for (int i = 0; i < colour.Length; i++)
            {
                var c = colour[i];
                switch (c)
                {
                    case 'A': buffer[i] = 'a'; break;
                    case 'B': buffer[i] = 'b'; break;
                    case 'C': buffer[i] = 'c'; break;
                    case 'D': buffer[i] = 'd'; break;
                    case 'E': buffer[i] = 'e'; break;
                    case 'F': buffer[i] = 'f'; break;
                    case 'G': buffer[i] = 'g'; break;
                    case 'H': buffer[i] = 'h'; break;
                    case 'I': buffer[i] = 'i'; break;
                    case 'J': buffer[i] = 'j'; break;
                    case 'K': buffer[i] = 'k'; break;
                    case 'L': buffer[i] = 'l'; break;
                    case 'M': buffer[i] = 'm'; break;
                    case 'N': buffer[i] = 'n'; break;
                    case 'O': buffer[i] = 'o'; break;
                    case 'P': buffer[i] = 'p'; break;
                    case 'Q': buffer[i] = 'q'; break;
                    case 'R': buffer[i] = 'r'; break;
                    case 'S': buffer[i] = 's'; break;
                    case 'T': buffer[i] = 't'; break;
                    case 'U': buffer[i] = 'u'; break;
                    case 'V': buffer[i] = 'v'; break;
                    case 'W': buffer[i] = 'w'; break;
                    case 'X': buffer[i] = 'x'; break;
                    case 'Y': buffer[i] = 'y'; break;
                    case 'Z': buffer[i] = 'z'; break;
                    default: buffer[i] = c; break;
                }
            }
        }

        [Benchmark]
        public void ToLowerAsciiSpanCustomFromString()
        {
            var colour = "ActiveBorder";
            Span<char> buffer = stackalloc char[colour.Length];
            ToLowerAscii(colour, ref buffer);
            var trimmed = ((ReadOnlySpan<char>)buffer).Trim();
        }

        [Benchmark]
        public void ToLowerAsciiSpanCustomFromStringTrimmed()
        {
            var colour = "ActiveBorder".AsSpan().Trim();
            Span<char> buffer = stackalloc char[colour.Length];
            ToLowerAscii(colour, ref buffer);
        }
    }
}