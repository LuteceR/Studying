using System;
using System.Diagnostics;
using System.Threading;

// Задание 2

namespace c__project_for_studying
{
    public class MyProgram
    {
        // формирование массива B из масива array из элементов по индексам i до j не включительно
        public static void calculation(double[] arr1, double[] arr2, int i, int j)
        {
            for (int k = i; k < j; k++)
            {
                arr2[k] = Math.Pow(arr1[k], 1.789);
            };
        }

        // Создание массива для параллельного последовательного вычисления из k-того кол-ва потоков
        public static void parallelComputing(double[] a, double[] b, int n, int k)
        {
            Thread[] threads = new Thread[k];

            // создание первого потока вне цикла, так как он начинается не с (i * n / k + 1), а с (i * n / k)
            // i - номер создаваемого потока + кол-во потоков до него
            // n - размер массивов A и B
            // k - кол-во потоков

            threads[0] = new Thread(() => calculation(a, b, 0, n / k));
            threads[0].Start();
            //Console.WriteLine("Создан 1 поток");
            //Console.WriteLine($"Работает с данными с индексами с 0 до {n / k}");

            if (k > 1)
            {
                for (int i = 1; i < k - 1; i++)
                {
                    int start = i * n / k;
                    int end = i * (n / k) + (n / k);
                    threads[i] = new Thread(() => calculation(a, b, start, end));
                    threads[i].Start();
                    //Console.WriteLine($"Создан {i + 1} поток");
                    //Console.WriteLine($"Работает с данными с индексами с {start} до {end}");
                };

                // Если не получается ровно разделить массивы на все потоки, то остаток выполняет последний поток
                threads[k - 1] = new Thread(() => calculation(a, b, n - n / k - n % k, n));
                threads[k - 1].Start();
                //Console.WriteLine($"Создан {k} поток");
                //Console.WriteLine($"Работает с данными с индексами с {n - n / k - n % k} до {n}");
            }
            ;

            // дожидание завершения работы потоков
            foreach (var thread in threads)
            {
                thread.Join();
            };

            //Console.WriteLine("Все потоки завершили работу!");
        }

        public static void Main()
        {
            int[] N = new int[5] { 1000, 5000, 10000, 15000, 20000 };
            int[] numOfThreads = new int[7] { 1, 2, 4, 8, 12, 16, 20};
            int countOfLaunching = 20;

            foreach (int n in N) 
            {
                foreach (int num in numOfThreads)
                {
                    double[] A = new double[n];
                    double[] B = new double[n];
                    double[] results = new double[countOfLaunching];

                    Random rnd = new Random();
                    var stopwatch = new Stopwatch();

                    // заполнение массива
                    for (int i = 0; i < n; i++)
                    {
                        A[i] = rnd.Next(0, 100);
                    }
                    ;

                    // создание потоков и их запуск
                    for (int i = 0; i < countOfLaunching; i++)
                    {
                        stopwatch.Start();
                        parallelComputing(A, B, n, num);
                        stopwatch.Stop();

                        results[i] = stopwatch.Elapsed.TotalSeconds;
                    }
                    ;

                    // проверка на обработку каждого элемента массива
                    //for (int i = 0; i < B.Length; i++)
                    //{
                    //    Console.WriteLine($"{B[i]}, {A[i]}, {i}");
                    //}

                    //Console.WriteLine($"Для параллельного вычисления массива A из {n} элементов на {num} потоках после {countOfLaunching} запусков среднее время равно");
                    //Console.WriteLine($"{results.Average()} сек.");
                    Console.WriteLine($"N = {n}, Threads = {num}, avg = {results.Average()}");
                }
                ;
            };
        }
    }
}
