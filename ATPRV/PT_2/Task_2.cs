using System;
using System.Diagnostics;
using System.Threading;

namespace csProjectForStudying
{
    public class MyProgram
    {
        public static double function(double x)
        {
            // моя функция
            return ((Math.Sin(2 * x) + 9) + Math.Cos(x) / (1.3 + x * 3));
        }

        // функция, высчитывающая интеграл с l-того бина до k-того не включительно
        // от a до b с n кол-вом разбиений
        // l > 0
        public static double Integral(double a, double b, int n, int l, int k)
        {
            // ширина бинов при разбиении длинны интеграла на n частей
            double w = (b - a) / n;
            double[] S = new double[k];
            double sum = 0;

            for (int i = l; i < k; i++)
            {
                int index = i;
                double x = a + index * w;
                double y = function(x);
                S[index - l] = Math.Abs(y * w);
                //Console.WriteLine($"w = {w} x = {x} y = {y} | {S[index - l]}");
            }

            for (int i = 0; i < k - l; i++)
            {
                //Console.WriteLine($"S{i} = {S[i]}");
                sum += S[i];
            }

            //Console.WriteLine($"S = {sum} | bins = {n} | from {l} to {k - 1}");
            return sum;
        }

        // функция, которая распараллеливает задание по нахождению значения интеграла
        // A - начало интеграла
        // B - конец интеграла
        // num_of_threads - количество потоков
        // n - количество разбиений
        public static double Parallelization(double A, double B, int num_of_threads, int n)
        {
            Thread[] threads = new Thread[num_of_threads];
            int k = n / num_of_threads;
            double[] result = new double[num_of_threads + 1];
            double sum = 0;

            for (int i = 0; i < num_of_threads; i++)
            {
                // index необходим, чтобы i не замыкалась
                // + лямбда не считывает значение i на момент создания,
                // а захватывает саму переменную и потоки работают с значением i из последней итерации
                int index = i;
                int start = i * k;
                int end = (i + 1) * k;
                threads[index] = new Thread(() =>
                {
                    result[index] = Integral(A, B, n, start, end + 1);
                });

                //Console.WriteLine($"thread {i + 1} works with {start} - {end} bins");
                threads[i].Start();
            }

            for (int i = 0; i < num_of_threads; i++)
            {
                threads[i].Join();
            }

            for (int i = 0; i < num_of_threads; i++)
            {
                sum += result[i];
                //Console.WriteLine($"{result[i]}");
            }

            return sum;
        }

        public static void Main()
        {
            // n - количество разбиений
            int n = 4;
            double A = 0.0;
            double B = 100.0;
            double[] Ss = new double[n];
            int lastNum = 0;
            int[] threads = new int[5] { 4, 8 , 12, 16, 20};

            Stopwatch sw = new Stopwatch();

            Random rnd = new Random();

            //Integral(A, B, 6, 1, 7);

            double S1 = Integral(A, B, 1, 1, 1 + 1);
            double S2 = Integral(A, B, 2, 1, 2 + 1);

            //Console.WriteLine($"{Parallelization(A, B, 4, 10000)}");

            // множество эпсилонов
            double[] E = new double[2] { 0.001, 0.0001 }; 

            foreach (int num in threads)
            {
                foreach (double e in E)
                {
                    sw.Start();
                    while (Math.Abs(S2 - S1) >= e)
                    {
                        S1 = S2;
                        S2 = Parallelization(A, B, num, n * 2);
                        //Console.WriteLine($"|S2 - S1| = {Math.Abs(S1 - S2)} S1 = {S1} S2 = {S2} threads = {num} n = {n}");
                        lastNum = n;
                        n = n * 2;
                    }
                    sw.Stop();
                    Console.WriteLine($"получившаяся точность удовлетворяющая |S2 - S1| >= e\n|S2 - S1| = |{S2} - {S1}| = {Math.Abs(S2 - S1)} n = {lastNum} threads = {num} e = {e}\n time = {sw.Elapsed.TotalSeconds}");
                }
            }
        }
    }
}
