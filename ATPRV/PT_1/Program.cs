using System;
using System.Diagnostics;
using System.Threading;

namespace Studying 
{
    class Program
    {
        static long FunctionMethod1(int N)
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


        public static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            long[] arrfrst = new long[100];
            _ = FunctionMethod1(10000);
        }
    }
}