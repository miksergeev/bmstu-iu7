using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

// ввод
Console.WriteLine("Введите первое слово");
string s1 = " " + Console.ReadLine();
s1 = s1.ToUpper();

Console.WriteLine("Введите второе слово");
string s2 = " " + Console.ReadLine();
s2 = s2.ToUpper();

int[,] D = new int[s1.Length + 1, s2.Length + 1];
D[0, 0] = 0;

// 1. Расчёт расстояния Левенштейна. Нерекурсивная формула с матрицей

// таймер
var before = DateTime.Now;
Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
for (int q = 0; q < 20; q++)
{   // расчёт расстояния
    // rCycle для строк (rows) и cCycle для столбцов
    for (int rCycle = 0; rCycle < s1.Length; rCycle++)
    {
        for (int cCycle = 0; cCycle < s2.Length; cCycle++)
        {
            int lettersComparison;
            if (s1[rCycle] == s2[cCycle])
                lettersComparison = 0;
            else
                lettersComparison = 1;

            if (rCycle == 0 && cCycle == 0)
                D[rCycle, cCycle] = 0;
            else
               if (rCycle > 0 && cCycle == 0)
                D[rCycle, cCycle] = rCycle;
            else
               if (cCycle > 0 && rCycle == 0)
                D[rCycle, cCycle] = cCycle;
            else
                if (rCycle > 0 && cCycle > 0)
                D[rCycle, cCycle] = Math.Min(Math.Min(D[rCycle, cCycle - 1] + 1, D[rCycle - 1, cCycle] + 1), D[rCycle - 1, cCycle - 1] + lettersComparison);
        }
    }
}
stopWatch.Stop();

// вывод результата
string s1output = (" " + s1);
string s2output = (" " + s2);
Console.WriteLine();
Console.WriteLine("1. Расстояние Левенштейна между этими словами, вычисленное с помощью нерекурсивной формулы с матрицей: ");
Console.WriteLine(D[s1.Length - 1, s2.Length - 1]);

// вывод времени выполнения алгоритма
Console.WriteLine("Время выполнения программы:");
Console.WriteLine(stopWatch.Elapsed / 20);
Console.WriteLine("Тактов таймера:");
Console.WriteLine(stopWatch.ElapsedTicks / 20);
Console.WriteLine();

// вывод матрицы для проверки
Console.WriteLine(s2output);
for (int rCycle = 0; rCycle < s1.Length; rCycle++)
{

    Console.Write(s1output[rCycle + 1]);
    for (int cCycle = 0; cCycle < s2.Length; cCycle++)
    {
        Console.Write(D[rCycle, cCycle]);
    }
    Console.WriteLine();
}
Console.WriteLine();

// 2. Расчёт расстояния Левенштейна. Рекурсивная формула без кэша
int rRecursive = s1.Length - 1;
int cRecursive = s2.Length - 1;

// функция расчёта расстояния
int LevDistance(string s1, string s2)
{
    int Recursive(int rRecursive, int cRecursive)
    {
        if ((rRecursive == 0) || (cRecursive == 0))
            return Math.Max(rRecursive, cRecursive);
        if ((rRecursive > 0) && (cRecursive > 0)) // чтобы не выйти за границы слов
            if (s1[rRecursive - 1] == s2[cRecursive - 1])
                return (1 + Math.Min(
                    Math.Min
                    (Recursive(rRecursive, cRecursive - 1), Recursive(rRecursive - 1, cRecursive)), Recursive(rRecursive - 1, cRecursive - 1) - 1));

        return (1 + Math.Min(
                            Math.Min
                            (Recursive(rRecursive, cRecursive - 1), Recursive(rRecursive - 1, cRecursive)), Recursive(rRecursive - 1, cRecursive - 1)));
    }
    return Recursive(s1.Length, s2.Length);
}

// вывод результата
Console.WriteLine("2. Расстояние Левенштейна между этими словами, вычисленное рекурсивно (без кэша): ");
stopWatch.Restart();
Console.WriteLine(LevDistance(s1, s2));
stopWatch.Stop();

// вывод времени выполнения алгоритма
Console.WriteLine("Время выполнения программы:");
Console.WriteLine(stopWatch.Elapsed);
Console.WriteLine("Тактов таймера:");
Console.WriteLine(stopWatch.ElapsedTicks);
Console.WriteLine();
stopWatch.Reset();

Console.WriteLine();

//3. рекурсивная формула с кэшем

// заполняем кэш-матрицу машинной бесконечностью
int[,] DCash = new int[s1.Length + 1, s2.Length + 1];
DCash[0, 0] = 0;
for (rRecursive = 0; rRecursive < s1.Length + 1; rRecursive++)
{
    for (cRecursive = 0; cRecursive < s2.Length + 1; cRecursive++)
    {
        DCash[rRecursive, cRecursive] = -1;
    }
}

int rRecursiveCash = s1.Length - 1;
int cRecursiveCash = s2.Length - 1;

// функция расчёта расстояния
int Recursive(int rRecursiveCash, int cRecursiveCash)
{
    if ((rRecursiveCash == 0) || (cRecursiveCash == 0))
        return Math.Max(rRecursiveCash, cRecursiveCash);
    if ((rRecursiveCash > 0) && (cRecursiveCash > 0)) // чтобы не выйти за границы слов

        if (s1[rRecursiveCash - 1] == s2[cRecursiveCash - 1])
        // return Recursive(rRecursiveCash - 1, cRecursiveCash - 1);
        { 
            D[rRecursiveCash, cRecursiveCash] = (Math.Min(
                     Math.Min
                     (Recursive(rRecursiveCash, cRecursiveCash - 1) + 1, Recursive(rRecursiveCash - 1, cRecursiveCash) + 1),
                                                                                                                     Recursive(rRecursiveCash - 1, cRecursiveCash - 1)));
    return D[rRecursiveCash, cRecursiveCash];
        }
    D[rRecursiveCash, cRecursiveCash] = (1 + Math.Min(
                        Math.Min
                        (Recursive(rRecursiveCash, cRecursiveCash - 1), Recursive(rRecursiveCash - 1, cRecursiveCash)),
                                                                                                                        Recursive(rRecursiveCash - 1, cRecursiveCash - 1)));
    return D[rRecursiveCash, cRecursiveCash];
}

// функция пополнения кэш-матрицы
int LevDistanceCash(string s1, string s2)
{
    for (int rRecursiveCash = 0; rRecursiveCash < s1.Length; rRecursiveCash++)
    {
        for (int cRecursiveCash = 0; cRecursiveCash < s2.Length; cRecursiveCash++)
        {
            // проверяем в кэш-матрице
            if (DCash[rRecursiveCash, cRecursiveCash] != -1)
                return DCash[rRecursiveCash, cRecursiveCash];
        }
    }
    DCash[rRecursiveCash, cRecursiveCash] = Recursive(s1.Length, s2.Length);
    return DCash[rRecursiveCash, cRecursiveCash];
}

// вывод результата
Console.WriteLine("3. Расстояние Левенштейна между этими словами, вычисленное рекурсивно (с кэшем): ");
stopWatch.Restart();
Console.WriteLine(LevDistanceCash(s1, s2));
stopWatch.Stop();

// вывод времени выполнения алгоритма
Console.WriteLine("Время выполнения программы:");
Console.WriteLine(stopWatch.Elapsed);
Console.WriteLine("Тактов таймера:");
Console.WriteLine(stopWatch.ElapsedTicks);
Console.WriteLine();


//4. простой нерекурсивный способ для подсчёта расстояния Дамерау-Левенштейна (без таймера)

// пустая матрица
int[,] DDamLev = new int[s1.Length + 1, s2.Length + 1];
DDamLev[0, 0] = 0;

    for (int rCycle = 0; rCycle < s1.Length; rCycle++)
    {
        for (int cCycle = 0; cCycle < s2.Length; cCycle++)
        {
            int lettersComparison;
            if (s1[rCycle] == s2[cCycle])
                lettersComparison = 0;
            else
                lettersComparison = 1;

            if (rCycle == 0 && cCycle == 0)
                DDamLev[rCycle, cCycle] = 0;
            else

               if (rCycle > 0 && cCycle == 0)
                DDamLev[rCycle, cCycle] = rCycle;

            else
               if (cCycle > 0 && rCycle == 0)
                DDamLev[rCycle, cCycle] = cCycle;

            else
               if (rCycle > 1 && cCycle > 1 && s1[rCycle] == s2[cCycle - 1] && s1[rCycle - 1] == s2[cCycle])
                DDamLev[rCycle, cCycle] = Math.Min(
                                            Math.Min(DDamLev[rCycle, cCycle - 1] + 1, DDamLev[rCycle - 1, cCycle] + 1),
                                            Math.Min(DDamLev[rCycle - 1, cCycle - 1] + lettersComparison, DDamLev[rCycle - 2, cCycle - 2] + 1)
                                            );

            else
                if (rCycle > 0 && cCycle > 0)
                DDamLev[rCycle, cCycle] = Math.Min(Math.Min(DDamLev[rCycle, cCycle - 1] + 1, DDamLev[rCycle - 1, cCycle] + 1), DDamLev[rCycle - 1, cCycle - 1] + lettersComparison);
        }
    }

Console.WriteLine();

// вывод результата
Console.WriteLine("4. Расстояние Дамерау-Левенштейна между этими словами, вычисленное с помощью нерекурсивной формулы с матрицей:");
Console.WriteLine(DDamLev[s1.Length - 1, s2.Length - 1]);
Console.WriteLine(s2output);

// вывод матрицы для проверки
for (int rCycle = 0; rCycle < s1.Length; rCycle++)
{
    Console.Write(s1output[rCycle + 1]);
    for (int cCycle = 0; cCycle < s2.Length; cCycle++)
    {
        Console.Write(DDamLev[rCycle, cCycle]);
        //
    }
    Console.WriteLine();
}
