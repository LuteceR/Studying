using System;
using System.Diagnostics;
using System.Threading;

//Задание 1

//namespace c__project_for_studying
//{
//    public class MyProgram
//    {
//        //const int N = 10000;

//        //Значение N, чтобы объем используемых данных был сопостовим с размерами кэшей L2 и L3 моего процессора
//        //L2 и L3 = 4мб и 32мб
//        const int N = 2000;
//        public static double Method1()
//        {
//            var stopwatch = new Stopwatch();
//            double[,] a = new double[N, N];
//            stopwatch.Start();
//            for (int i = 0; i < N; i++)
//                for (int j = 0; j < N; j++)
//                    a[i, j] = i / (j + 1);
//            stopwatch.Stop();
//            return stopwatch.Elapsed.TotalSeconds;
//        }

//        public static double Method2()
//        {
//            var stopwatch = new Stopwatch();
//            double[,] a = new double[N, N];
//            stopwatch.Start();
//            for (int j = 0; j < N; j++)
//                for (int i = 0; i < N; i++)
//                    a[i, j] = i / (j + 1);
//            stopwatch.Stop();
//            return stopwatch.Elapsed.TotalSeconds;
//        }

//        public static double Method3()
//        {
//            var stopwatch = new Stopwatch();
//            double[,] a = new double[N, N];
//            stopwatch.Start();
//            for (int i = N - 1; i >= 0; i--)
//                for (int j = N - 1; j >= 0; j--)
//                    a[i, j] = i / (j + 1);
//            stopwatch.Stop();
//            return stopwatch.Elapsed.TotalSeconds;
//        }

//        public static double Method4()
//        {
//            var stopwatch = new Stopwatch();
//            double[,] a = new double[N, N];
//            stopwatch.Start();
//            for (int j = N - 1; j >= 0; j--)
//                for (int i = N - 1; i >= 0; i--)
//                    a[i, j] = i / (j + 1);
//            stopwatch.Stop();
//            return stopwatch.Elapsed.TotalSeconds;
//        }

//        public static void Main()
//        {
//            // массивы с результатами
//            double[] method1 = new double[100];
//            double[] method2 = new double[100];
//            double[] method3 = new double[100];
//            double[] method4 = new double[100];

//            //код для подтверждения того, что скорость обращения к массиву зависит от его индекса
//            //int[] array = new int[11] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
//            //var stopwatch = new Stopwatch();

//            //stopwatch.Start();
//            //_ = array[0];
//            //stopwatch.Stop();

//            //Console.WriteLine(stopwatch.Elapsed.TotalSeconds);

//            //stopwatch.Start();
//            //_ = array[10];
//            //stopwatch.Stop();

//            //Console.WriteLine(stopwatch.Elapsed.TotalSeconds);

//            for (int i = 0; i < 100; i++)
//            {
//                method1[i] = Method1();
//                method2[i] = Method2();
//                method3[i] = Method3();
//                method4[i] = Method4();
//            }

//            Console.WriteLine($"Method1 min={method1.Min()}, max={method1.Max()}, average={method1.Average()}");
//            Console.WriteLine($"Method2 min={method2.Min()}, max={method2.Max()}, average={method2.Average()}");
//            Console.WriteLine($"Method3 min={method3.Min()}, max={method3.Max()}, average={method3.Average()}");
//            Console.WriteLine($"Method4 min={method4.Min()}, max={method4.Max()}, average={method4.Average()}");
//        }
//    }
//}

namespace c__project_for_studying
{
    public class MyProgram
    {
        // формирование массива B из масива array из элементов по индексам i до j не включительно
        public static void calculation(double[] arr1, double[] arr2, int i, int j)
        {
            for (int k = i; k < j; k++)
            {
                for (int l = 0; l < k; l++) 
                {
                    arr2[k] = Math.Pow(arr1[k], 1.789);
                }
            }
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
                Parallel.For(1, k - 1, i => 
                {
                    int start = i * n / k;
                    int end = i * (n / k) + (n / k);
                    threads[i] = new Thread(() => calculation(a, b, start, end));
                    threads[i].Start();
                    //Console.WriteLine($"Создан {i + 1} поток");
                    //Console.WriteLine($"Работает с данными с индексами с {start} до {end}");
                });

                // Если не получается ровно разделить массивы на все потоки, то остаток выполняет последний поток
                threads[k - 1] = new Thread(() => calculation(a, b, n - n / k - n % k, n));
                threads[k - 1].Start();
                //Console.WriteLine($"Создан {k} поток");
                //Console.WriteLine($"Работает с данными с индексами с {n - n / k - n % k} до {n}");
            };

            // дожидание завершения работы потоков
            foreach (var thread in threads)
            {
                thread.Join();
            };

            //Console.WriteLine("Все потоки завершили работу!");
        }

        // Функция для подсчёта всех элементов массива arr2 из массива arr1 с элемента с индексом i и с шагом l
        // предусмотрен отдельный случай для s = 0, тк в ином случае первый элемент не обрабатывался
        public static void partionCalculation(double[] arr1, double[] arr2, int s, int l, int n)
        {
            if (s == 0)
            {
                arr2[0] = Math.Pow(arr1[0], 1.789);
                partionCalculation(arr1, arr2, l, l, n);
            }
            else
            {
                for (int i = s; i < n; i = i + l)
                {
                    for (int k = 0; k < i; k++)
                    {
                        arr2[i] = Math.Pow(arr1[i], 1.789);
                    }
                    ;
                }
                ;
            }
            ;
        }

        // Функция создаёт k-тое кол-во потоков и каждый из них считает первый элемент, индекс которого соответствует номеру потока
        // и каждый i-ый + 1 элемент
        // Пришлось вынести нулевой Thread, потому что в ином случае получалось так, что какие-то потоки не работали
        public static void CrossingParallelComputing(double[] a, double[] b, int k, int n)
        {
            Thread[] threads = new Thread[k];
            threads[0] = new Thread(() => partionCalculation(a, b, 0, k, n));
            threads[0].Start();

            for (int i = 1; i < k; i++)
            {
                threads[i] = new Thread(() => partionCalculation(a, b, i, k, n));
                threads[i].Start();
            };

            for (int i = 0; i < k; i++)
            {
                threads[i].Join();
            };
        }

        // Использование метода Parallel.For() из namespace System.Threading.Tasks
        // Он запускает цикл For итерациями, которые идут параллельно и на практике оказался быстрее метода parallelComputing()
        public static void ParallelClassComputing(double[] a, double[] b, int n, int k)
        {
            Parallel.For(0, k, i => {
                calculation(a, b, 0, n);
            });
        }

        public static void Main()
        {
            int[] N = new int[5] { 1000, 5000, 10000, 15000, 20000 };
            int[] numOfThreads = new int[7] { 1, 2, 4, 8, 12, 16, 20 };
            int countOfLaunching = 20;

            //// код для запуска другой версии заполнения массива B
            //foreach (int n in N)
            //{
            //    double[] A = new double[n];
            //    double[] B = new double[n];
            //    double[] results = new double[countOfLaunching];

            //    Random rnd = new Random();
            //    var stopwatch = new Stopwatch();

            //    // заполнение массива
            //    for (int i = 0; i < n; i++)
            //    {
            //        A[i] = rnd.Next(0, 100);
            //    }

            //    foreach (int num in numOfThreads)
            //    {
            //        for (int i = 0; i < countOfLaunching; i++)
            //        {
            //            stopwatch.Start();
            //            CrossingParallelComputing(A, B, num, n);
            //            stopwatch.Stop();

            //            results[i] = stopwatch.Elapsed.TotalSeconds;
            //        }
            //        Console.WriteLine($"N = {n}, Threads = {num}, avg = {results.Average()}");
            //    }

            //    // вывод значений массивов A и B
            //    for (int i = 0; i < 30; i++)
            //    {
            //        Console.WriteLine($"{A[i]}, {B[i]}, {i}");
            //    }
            //}

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
            }
            ;
        }
    }
}
