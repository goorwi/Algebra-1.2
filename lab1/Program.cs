using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lab1
{
    public class Permutation
    {
        private List<int> permutation = new List<int>();

        public Permutation()
        {
        }

        public void Add(int x) => permutation.Add(x);

        public int Get(int i) => permutation[i];

        public void Change(int i, int y) => permutation[i] = y;

        public bool HasValue(int x) => permutation.Contains(x);

        public int Size => permutation.Count;

        public Permutation Copy()
        {
            var p = new Permutation();
            for (int i = 0; i < Size; i++)
                p.Add(permutation[i]);
            return p;
        }

        public override bool Equals(object obj)
        {
            Permutation other = obj as Permutation;
            if (this == null && other == null) return false;
            else if (this == null || other == null) return false;
            else if (this.Size != other.Size) return false;

            for (int i = 0; i < this.Size; i++)
            {
                if (this.Get(i) != other.Get(i)) return false;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < permutation.Count; i++)
                sb.Append($"{i + 1}\t");
            sb.Append("\n");
            for (var i = 0; i < permutation.Count; i++)
                sb.Append($"{permutation[i]}\t");
            return sb.ToString();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            int task = Menu();

            Permutation permutation = null;

            do
            {
                switch (task)
                {
                    case 1:
                        {
                            permutation = InputPermutatioin();
                            break;
                        }
                    case 2:
                        {
                            if (permutation != null)
                                Console.WriteLine(permutation.ToString());
                            break;
                        }
                    case 3:
                        {
                            Permutation first = new Permutation();
                            Permutation second = new Permutation();
                            if (permutation == null)
                            {
                                Console.WriteLine("Первоначальная подстановка отсутствует");
                                Console.WriteLine("Ввод первой подстановки");
                                first = InputPermutatioin();
                                Console.WriteLine("Ввод второй подстановки");
                                second = InputPermutatioin();
                            }
                            else
                            {
                                Console.WriteLine(permutation.ToString());
                                Console.WriteLine("1. Оставить подстановку\n" +
                                    "2. Изменить подстановку");
                                Console.Write("Номер действия: ");
                                int choice = 0;
                                int.TryParse(permutation.ToString(), out choice);

                                if (choice == 1)
                                {
                                    first = permutation;
                                    Console.WriteLine("Ввод второй подстановки");
                                    second = InputPermutatioin();
                                }
                                else
                                {
                                    Console.WriteLine("Ввод первой подстановки");
                                    first = InputPermutatioin();
                                    Console.WriteLine("Ввод второй подстановки");
                                    second = InputPermutatioin();
                                }
                            }

                            Console.WriteLine($"\nПервая подстановка:\n{first.ToString()}");
                            Console.WriteLine($"Первая подстановка:\n{second.ToString()}");
                            Console.WriteLine($"Результат композиции:\n{Composition(first, second).ToString()}");

                            break;
                        }
                    case 4:
                        {
                            if (permutation == null)
                            {
                                Console.WriteLine("Первоначальная подстановка отсутствует");
                                Console.WriteLine("Ввод подстановки");
                                permutation = InputPermutatioin();
                            }
                            Console.WriteLine($"Подстановка:\n{permutation}");
                            Console.WriteLine($"Обратная подстановка:\n{Reverse(permutation).ToString()}");
                            break;
                        }
                    case 5:
                        {
                            if (permutation == null)
                            {
                                Console.WriteLine("Первоначальная подстановка отсутствует");
                                Console.WriteLine("Ввод подстановки");
                                permutation = InputPermutatioin();
                            }
                            Console.WriteLine($"Подстановка:\n{permutation}");

                            int pow = 0;
                            do
                            {
                                Console.Write("Введите степень: ");
                                if (!int.TryParse(Console.ReadLine(), out pow))
                                {
                                    Console.WriteLine("Некорректное значение!");
                                    pow = int.MinValue;
                                }
                            } while (pow == int.MinValue);

                            Console.WriteLine($"Подстановка в {pow} степени:\n{Pow(permutation, pow).ToString()}");
                            break;
                        }
                    case 6:
                        {
                            if (permutation == null)
                            {
                                Console.WriteLine("Первоначальная подстановка отсутствует");
                                Console.WriteLine("Ввод подстановки");
                                permutation = InputPermutatioin();
                            }
                            Console.WriteLine($"Подстановка:\n{permutation}");

                            Console.WriteLine($"Необходимая степень: {GetPow(permutation).ToString()}");
                            break;
                        }
                    case 7:
                        {
                            if (permutation == null)
                            {
                                Console.WriteLine("Первоначальная подстановка отсутствует");
                                Console.WriteLine("Ввод подстановки");
                                permutation = InputPermutatioin();
                            }
                            Console.WriteLine($"Подстановка:\n{permutation}");
                            Console.WriteLine("Разложение подстановки на циклы:");

                            var cycles = Cycles(permutation);
                            foreach (var cycle in cycles)
                            {
                                Console.WriteLine($"({string.Join(", ", cycle)})");
                            }
                            break;
                        }
                }
                Console.WriteLine("\n");
                task = Menu();
            } while (task != 0);
        }

        public static List<List<int>> Cycles(Permutation perm)
        {
            int n = perm.Size;
            bool[] visited = new bool[n];
            List<List<int>> cycles = new List<List<int>>();

            // Обход элементов перестановки
            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                {
                    List<int> cycle = new List<int>();
                    int x = i + 1; // Индексация начинается с 1

                    // Строим цикл, пока не вернёмся в исходную точку
                    while (!visited[x - 1])
                    {
                        cycle.Add(x);
                        visited[x - 1] = true;
                        x = perm.Get(x - 1); // Переход к следующему элементу цикла
                    }

                    // Добавляем найденный цикл в список циклов
                    cycles.Add(cycle);
                }
            }

            return cycles;
        }

        static int GetPow(Permutation permutation)
        {
            Console.WriteLine("1. Выводить промежуточные результаты\n" +
                "2. Не выводить");
            int choice = 0;
            do
            {
                Console.Write("Номер действия: ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Некорректное значение!");
                    choice = 0;
                }
            } while (choice == 0);

            Console.WriteLine();
            Permutation fin = new Permutation();
            for (int i = 0; i < permutation.Size; i++)
                fin.Add(i + 1);
            var perm = permutation.Copy();
            var temp = perm.Copy();

            int pow = 1;
            do
            {
                temp = Composition(perm, temp);
                ++pow;
                if (choice == 1)
                    Console.WriteLine($"Подстановка в {pow} степени:\n{temp}");
            } while (!temp.Equals(fin));

            return pow;
        }

        static Permutation Pow(Permutation perm, int power)
        {
            var prev = perm.Copy();
            for (int i = 0; i < power - 1; i++)
            {
                var temp = Composition(prev, perm);
                prev = temp;
            }

            return prev;
        }

        static Permutation Reverse(Permutation p)
        {
            Permutation per = p.Copy();

            for (int i = 0; i < p.Size; i++)
                per.Change(p.Get(i) - 1, i + 1);

            return per;
        }

        static Permutation Composition(Permutation first, Permutation second)
        {
            Permutation result = new Permutation();
            for (var i = 0; i < first.Size; ++i)
            {
                int a = first.Get(i);
                int b = second.Get(a - 1);
                result.Add(b);
            }

            return result;
        }

        static Permutation InputPermutatioin()
        {
            Console.WriteLine("1. Ввести вручную\n" +
                "2. Считать из файла");
            int choice = -1;
            while (choice == -1)
            {
                Console.Write("Номер действия: ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Введите корректное значение!");
                    choice = -1;
                }
                else if (choice < 1 || choice > 2)
                {
                    Console.WriteLine("Введите корректное значение!");
                    choice = -1;
                }
            }
            switch (choice)
            {
                case 1:
                    {
                        Permutation perm = new Permutation();

                        Console.Write("\nВведите количество элементов: ");
                        int n = 0;
                        int.TryParse(Console.ReadLine(), out n);
                        Console.WriteLine("\nВведите элементы");
                        for (int i = 0; i < n; i++)
                        {
                            bool isGood = false;
                            while (!isGood)
                            {
                                Console.Write($"{i + 1} -> ");
                                int number = 0;
                                if (int.TryParse(Console.ReadLine(), out number) && (number > 0 && number <= n) && !perm.HasValue(number))
                                {
                                    perm.Add(number);
                                    isGood = !isGood;
                                }
                                else
                                    Console.WriteLine("Введите корректное значение!\n");
                            }
                        }

                        return perm;
                    }
                case 2:
                    {
                        using (var sr = new StreamReader("input.txt"))
                        {
                            Permutation perm = new Permutation();
                            while (!sr.EndOfStream)
                            {
                                var nums = sr.ReadLine().Split();
                                foreach (var num in nums)
                                {
                                    int number = 0;
                                    if (int.TryParse(num, out number) && (number > 0 && number <= nums.Length) && !perm.HasValue(number))
                                    {
                                        perm.Add(int.Parse(num));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Файл содержит некорректные значения!");
                                        return null;
                                    }
                                }
                            }
                            return perm;
                        }
                    }
            }
            return null;
        }

        static int Menu()
        {
            int choice = -1;
            do
            {
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine("0. Выйти из программы\n" +
                    "1. Ввод подстановки\n" +
                    "2. Вывод подстановки\n" +
                    "3. Нахождение композиции двух подстановок\n" +
                    "4. Нахождения обратной подстановки\n" +
                    "5. Возведение подстановки в заданную степень\n" +
                    "6. Определения степени, в которую следует возвести подстановку, чтобы получить тождественную\n" +
                    "7. Разложение подстановки в циклы\n");

                Console.Write("Номер задания: ");
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Введите корректное значение!\n");
                    choice = -1;
                }
                else if (choice < 0 || choice > 7)
                {
                    Console.WriteLine("Введите корректное значение!\n");
                    choice = -1;
                }
            } while (choice == -1);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
            return choice;
        }
    }
}
