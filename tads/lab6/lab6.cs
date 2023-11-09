// Задача коммивояжёра методом полного перебора
using System;
using System.Globalization;
using System.Xml.Linq;

Console.WriteLine("Задача коммивояжёра методом полного перебора");
Console.WriteLine("Введите количество городов:");

int numOfTowns = Convert.ToInt32(Console.ReadLine()); // количество городов
int[,] arrayTowns = new int[numOfTowns, numOfTowns]; // создаём массив для хранения расстояний

Console.WriteLine("1 - заполнить матрицу смежности вручную");
Console.WriteLine("2 - сгенерировать матрицу смежности");
char choice = Convert.ToChar(Console.ReadLine());

int bestWay = 0; // значение лучшей длины
int currentWay = 0; // значение текущей длины маршрута

int last = 0;

switch (choice)
{
    case '1':
        {
            // заполнение вручную
            for (int i = 0; i < numOfTowns; i++)
            {
                for (int j = 0; j < numOfTowns; j++)
                {
                    if (i >= j) // для хранения верхнетреугольной матрицы
                    {
                        continue;
                    }
                    Console.Write("Введите расстояние из города " + i + " в город " + j + " : ");
                    arrayTowns[i, j] = Convert.ToInt32(Console.ReadLine());
                }

            }
            break;
        }
    case '2':
        {
            Random random = new Random();

            for (int i = 0; i < numOfTowns; i++)
            {
                for (int j = 0; j < numOfTowns; j++)
                {
                    arrayTowns[i, j] = random.Next(1, 10);
                    arrayTowns[j, i] = arrayTowns[i, j];
                    if (i == j)
                    {
                        arrayTowns[i, j] = 0;
                    }
                }
            }
            break;
        }
}

Console.WriteLine("Матрица смежности:");
for (int i = 0; i < numOfTowns; i++)
{
    for (int j = 0; j < numOfTowns; j++)
    {
        Console.Write(arrayTowns[i, j] + " ");
    }
    Console.WriteLine();
}

// создадим список ещё не посещённых городов J
List<int> listJ = new List<int>();

// проинициализируем его. На старте J включает все города в порядке возрастания
for (int i = 0; i < numOfTowns; i++)
{
    listJ.Add(i);
}
// создадим список для маршрута
List<int> listM = new List<int>();

// создадим массив для хранения лучшего маршрута
int[] bestRoute = new int[numOfTowns];

// флаг для прерывания отката
bool breakRollback = false;

// флаг, что откат вообще возможен
bool rollbackIsPossible = true;

int swap = 0;

for (int i = 0; i < numOfTowns - 1; i++)
{
    bestWay += arrayTowns[i, i + 1];
}
listM.CopyTo(bestRoute, 0); // будем считать первый найденный маршрут оптимальным (теоретически он может оказаться таким)

for (int i = 0; i < numOfTowns; i++) // заполняем маршрут
{
    listM.Add(listJ[0]);
    listJ.RemoveAt(0);
}

for (int i = 0; i < numOfTowns - 1; i++) // анализируем полученный маршрут
{
    currentWay += arrayTowns[ listM[i], listM[i+1] ];
}

if (currentWay <= bestWay) // если найденное расстояние меньше или равно
{ 
    bestWay = currentWay; // то оно и будет лучшим
    listM.CopyTo(bestRoute); // как и маршрут, который мы для этого сохраняем
}

// вывод первого маршрута
Console.WriteLine();
Console.Write("Длина текущего маршрута ");
for (int i = 0; i < numOfTowns; i++)
{
    Console.Write(listM[i] + " ");
}
Console.Write(" равна " + currentWay);
Console.WriteLine();
Console.WriteLine("С этим маршрутом и будем сравнивать другие маршруты");


// цикл пошёл
do // общий цикл, пока откат не станет невозможным
{
    breakRollback = false;
    do // цикл наполнения
    {
        if (listJ.Count == numOfTowns && last == listJ[listJ.Count - 1]) // если количество элементов в J равно числа городов и last равен последнему (самому большему) элементу
        {
            rollbackIsPossible = false;
            break;
        }

        last = listM[listM.Count - 1]; // последний элемент списка M кладём в переменную last
        listJ.Add(listM[listM.Count - 1]); // кладём последний элемент списка M в список J
        listM.RemoveAt(listM.Count - 1); // снимаем последний элемент списка M

        // сортируем список J
        for (int i = 0; i < listJ.Count; i++)
        {
            for (int j = 0; j < listJ.Count - 1 - i; j++)
            {
                if (listJ[j] > listJ[j + 1])
                {
                    swap = listJ[j];
                    listJ[j] = listJ[j + 1];
                    listJ[j + 1] = swap;
                }
            }
        }

        for (int i = 0; i < listJ.Count; i++) // пробуем в списке J найти город с номером больше, чем last (с минимальным номером)
        {
            if (listJ[i] > last)
            {
                listM.Add(listJ[i]); // если такой город найдётся - добавим его в M 
                listJ.RemoveAt(i); // и удалим из J
                breakRollback = true; // поставим флаг "прервать откат"
                break;
            }
        }
    }
    while (breakRollback == false);


    if (rollbackIsPossible == true)
    {
        for (; listM.Count < numOfTowns;) // пока в списке M городов меньше, чем всего, пополняем его из J
        {
            listM.Add(listJ[0]);
            listJ.RemoveAt(0);
        }

        currentWay = 0; // длина текущего маршрута = 0
        for (int i = 0; i < numOfTowns - 1; i++) // анализируем полученный маршрут
        {
            currentWay += arrayTowns[listM[i], listM[i + 1]];
        }

        if (currentWay <= bestWay) // если найденное расстояние меньше или равно
        {
            bestWay = currentWay; // то оно и будет лучшим
            listM.CopyTo(bestRoute); // как и маршрут, который мы для этого сохраняем
        }

        // блок вывода для каждой итерации
        Console.WriteLine();
        Console.Write("Длина текущего маршрута ");
        for (int i = 0; i < numOfTowns; i++)
        {
            Console.Write(listM[i] + " ");
        }
        Console.Write(" равна " + currentWay);
        Console.WriteLine();

        Console.Write("Длина лучшего маршрута равна " + bestWay + " , вот он: ");
        for (int i = 0; i < numOfTowns; i++)
        {
            Console.Write(bestRoute[i] + " ");
        }
        Console.WriteLine();
    }
}
while (rollbackIsPossible == true);

// завершающий блок вывода
Console.WriteLine();
Console.WriteLine("Кратчайшее расстояние:");
Console.WriteLine(bestWay);
Console.WriteLine("Оптимальный маршрут:");
for (int i = 0; i < numOfTowns; i++)
{
    Console.Write(bestRoute[i]);
    Console.Write(" ");
}
