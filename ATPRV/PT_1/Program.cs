using System;
using System.Diagnostics;
using System.Threading;

namespace Studying 
{
    class Program
    {
        static long FunctionMethod1(int M, int N)
        {
            for (int i = 0; i < M; i++)
            {
                var stopwatch = new Stopwatch();
                double[,] a = new double[N, N];
                stopwatch.Start();
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                        a[i, j] = i / (j + 1);
                stopwatch.Stop();
                //Console.WriteLine($"Время создания {stopwatch.ElapsedMilliseconds} мс");
                return stopwatch.ElapsedMilliseconds;
            }
            ;
        }


        public static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();

            _ = FunctionMethod1(100, 10000);
        }
    }
}