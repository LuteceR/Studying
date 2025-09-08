using System;

namespace Studying 
{
    class Program
    {
        static void FunctionMethod1(int N)
        {
            var stopwatch = new Stopwatch();
            double[,] a = new double[N, N];
            stopwatch.Start();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    a[i, j] = i / (j + 1);
            stopwatch.Stop();
            Console.WriteLine($"Время создания {stopwatch.ElapsedMilliseconds} мс");
        }
        public static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();

            FunctionMethod1(10);
        }
    }
}