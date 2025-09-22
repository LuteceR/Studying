using System;
using System.Diagnostics;
using System.Threading;

namespace csProjectForStudying
{
    public class MyProgram
    {
        // умножение матрицы A на матрицу B.
        // n - размерность матриц
        // [p1, p2] - строки/столбцы матрицы C, которые нужно получить посчитать
        // horizontal - результатом умножения получается строка или столбец
        public static void matrixTimes(int[,] mA, int[,] mB, int[,] mC, int n, int p1, int p2, bool horizontal = true)
        {
            // изначально считается, что мы умножаем по строчно
            if (horizontal)
            {
                Parallel.For(p1, p2, i =>
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            mC[i, j] += mA[i, k] * mB[k, j];
                        }
                    }
                });
            } else
            {
                Parallel.For(p1, p2, k =>
                {
                    for (int j = 0; j < n; j++)
                    {
                        for (int l = 0; l < n; l++)
                        {
                            mC[j, k] += mA[j, l] * mB[l, k];
                            //проверка на корректность умножения
                            //Console.WriteLine($"C = |{j + 1} {k + 1}| A = |{j + 1} {l + 1}| B = |{l + 1} {k + 1}|");
                        }
                    }
                });
            }
        }

        // time - время на 1 потоке
        // time_n - время на n потоках
        public static double speedup(double time, double time_n)
        {
            return time / time_n;
        }

        // speedup_ - высчитанное ускорение
        // n - количество потоков
        public static double efficiency(double speedup_, int n)
        {
            return (speedup_ / n) * 100; //в процентах
        }

        // Вычисление, создание потоков и распараллеливание по потокам
        // k - количество потоков
        // n - размерность матриц A и B
        public static void calculation(int k, int n, int[,] mA, int[,] mB, int[,] mC, bool horizontal = true)
        {
            Thread[] threads = new Thread[k];
            TimeSpan limit = TimaSpan.FromSeconds(180);
            CancellationTokenSource cts = new CancellationTokenSource(limit);

            CancellationTOken token = cts.Token();

            Task task = new Task(() =>
            {
                for (int i = 0; i < k; i++)
                {
                    int start = n / k * i;
                    int end = start + n / k;

                    threads[i] = new Thread(() => matrixTimes(mA, mB, mC, n, start, end, horizontal));
                    //Console.WriteLine($"Поток {i} работает с строками {start} -> {end}");
                    threads[i].Start();
                }
            }, token);
            
            task.Start()

            for (int i = 0; i < k; i++)
            {
                threads[i].Join();
            }
        }

        // подсчёт скорости работы 1 потока на матрицах размерности n x n
        // horizontal показывает - должно быть умножение по строкам (true) или столбцам (false)
        public static double one_thread(int n, int[,] mA, int[,] mB, int[,] mC, bool horizontal = true)
        {
            Thread thread = new Thread(() => matrixTimes(mA, mB, mC, n, 0, n, horizontal));
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            thread.Start();
            thread.Join();
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalSeconds;
        }

        public static void Main()
        {
            // размерности можно взять поменьше и будёт всё круто
            int[] N = new int[1] { 2500, 3000 };
            int[] numOfThreads = new int[7] { 1, 2, 4, 8, 12, 16, 20 };
            int line = 10; // граница для генерации случайных целочисленных значений

            Random rnd = new Random();

            foreach (bool horizontal in new[] { true, false })
            {
                foreach (int n in N)
                {
                    foreach (int num in numOfThreads)
                    {
                        int[,] A = new int[n, n];
                        int[,] B = new int[n, n];
                        int[,] C = new int[n, n];

                        Stopwatch time_n_ = new Stopwatch(); // подсчёт времени работы потоков n
                        double speedup_; // ускорение

                        // заполнение матриц
                        for (int i = 0; i < n; i++)
                        {
                            for (int j = 0; j < n; j++)
                            {
                                A[i, j] = rnd.Next(0, line);
                                B[i, j] = rnd.Next(0, line);
                            }
                        }


                        time_n_.Start();

                        calculation(num, n, A, B, C, horizontal);

                        time_n_.Stop();

                        double time = one_thread(n, A, B, C, horizontal);

                        speedup_ = speedup(time, time_n_.Elapsed.TotalSeconds);

                        Console.WriteLine($"horizontal = {horizontal}");
                        Console.WriteLine($" n = {n}, кол-во потоков = {num}");
                        Console.WriteLine($"Время работы алгоритма = {Math.Round(time_n_.Elapsed.TotalSeconds, 4)}");
                        Console.WriteLine($"speedup = {Math.Round(speedup_, 4)}");
                        //Console.WriteLine($"T(1) = {Math.Round(time, 4)}, T(n) = {Math.Round(time_n_.Elapsed.TotalSeconds, 4)}");
                        Console.WriteLine($"efficiency = {Math.Round(efficiency(speedup_, n), 4)}%\n");

                        // проверка результатов для матриц 5x5
                        //Console.WriteLine("\nmatrix A");
                        //for (int i = 0; i < n; i++)
                        //{
                        //    Console.WriteLine($"{A[i, 0]}, {A[i, 1]}, {A[i, 2]}, {A[i, 3]}, {A[i, 4]}");
                        //}

                        //Console.WriteLine("\nmatrix B");
                        //for (int i = 0; i < n; i++)
                        //{
                        //    Console.WriteLine($"{B[i, 0]}, {B[i, 1]}, {B[i, 2]}, {B[i, 3]}, {B[i, 4]}");
                        //}

                        //Console.WriteLine($"\nматрицы {n}x{n}, потоки = {num}");

                        //for (int i = 0; i < n; i++)
                        //{
                        //    Console.WriteLine($"{C[i,0]}, {C[i, 1]}, {C[i, 2]}, {C[i, 3]}, {C[i, 4]}");
                        //}
                    }
                }
            }
        }
    }
}
