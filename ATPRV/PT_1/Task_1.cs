using System;
using System.Diagnostics;
using System.Threading;

namespace c__project_for_studying
{
    public class MyProgram
    {
        //const int N = 10000;

        //Значение N, чтобы объем используемых данных был сопостовим с размерами кэшей L2 и L3 моего процессора
        //L2 и L3 = 4мб и 32мб
        const int N = 2000;
        public static double Method1()
        {
            var stopwatch = new Stopwatch();
            double[,] a = new double[N, N];
            stopwatch.Start();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    a[i, j] = i / (j + 1);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalSeconds;
        }

        public static double Method2()
        {
            var stopwatch = new Stopwatch();
            double[,] a = new double[N, N];
            stopwatch.Start();
            for (int j = 0; j < N; j++)
                for (int i = 0; i < N; i++)
                    a[i, j] = i / (j + 1);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalSeconds;
        }

        public static double Method3()
        {
            var stopwatch = new Stopwatch();
            double[,] a = new double[N, N];
            stopwatch.Start();
            for (int i = N - 1; i >= 0; i--)
                for (int j = N - 1; j >= 0; j--)
                    a[i, j] = i / (j + 1);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalSeconds;
        }

        public static double Method4()
        {
            var stopwatch = new Stopwatch();
            double[,] a = new double[N, N];
            stopwatch.Start();
            for (int j = N - 1; j >= 0; j--)
                for (int i = N - 1; i >= 0; i--)
                    a[i, j] = i / (j + 1);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalSeconds;
        }

        public static void Main()
        {
            // массивы с результатами
            double[] method1 = new double[100];
            double[] method2 = new double[100];
            double[] method3 = new double[100];
            double[] method4 = new double[100];

            //код для подтверждения того, что скорость обращения к массиву зависит от его индекса
            //int[] array = new int[11] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            //var stopwatch = new Stopwatch();

            //stopwatch.Start();
            //_ = array[0];
            //stopwatch.Stop();

            //Console.WriteLine(stopwatch.Elapsed.TotalSeconds);

            //stopwatch.Start();
            //_ = array[10];
            //stopwatch.Stop();

            //Console.WriteLine(stopwatch.Elapsed.TotalSeconds);

            for (int i = 0; i < 100; i++)
            {
                method1[i] = Method1();
                method2[i] = Method2();
                method3[i] = Method3();
                method4[i] = Method4();
            }

            Console.WriteLine($"Method1 min={method1.Min()}, max={method1.Max()}, average={method1.Average()}");
            Console.WriteLine($"Method2 min={method2.Min()}, max={method2.Max()}, average={method2.Average()}");
            Console.WriteLine($"Method3 min={method3.Min()}, max={method3.Max()}, average={method3.Average()}");
            Console.WriteLine($"Method4 min={method4.Min()}, max={method4.Max()}, average={method4.Average()}");
        }
    }
}
