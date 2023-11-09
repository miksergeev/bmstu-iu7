Console.WriteLine("Данная программа позволяет выполнить упаковку, обработку и распаковку симметричных разреженных матриц");
Console.Write("Введите размерность матриц: ");
int n = Convert.ToInt32(Console.ReadLine());

    // выделяем память под матрицу A
    int[,] arrayA = new int[n, n];

    // выделяем память под матрицу B
    int[,] arrayB = new int[n, n];

    // выделяем память под матрицу C (туда будем распаковывать)
    int[,] arrayC = new int[n, n];

    // выделяем память под матрицу A.D
    int[] arrayAD = new int[n];

    // выделяем память под матрицу B.D
    int[] arrayBD = new int[n];

    // выделяем память под матрицу C.D
    int[] arrayCD = new int[n];

    // ввод массива A
    for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
        {
            Console.Write("Введите элемент [" + i + "][" + j + "]  массива A: ");
            arrayA[i,j] = Convert.ToInt32(Console.ReadLine());
        }

    // проверка на симметричность и заодно вывод матрицы
    bool flagNotSymA = false;
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if (arrayA[i,j] != arrayA[j,i])
                flagNotSymA = true;
            Console.Write(arrayA[i,j] + " ");
        }
        Console.WriteLine();
    }

    if (flagNotSymA == false)
        Console.WriteLine("Матрица A симметрична");
    else
        Console.WriteLine("Матрица A не симметрична. Она будет преобразована в симметричную.");

    // преобразование матрицы в симметричную
    if (flagNotSymA == true)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                arrayA[i,j] = arrayA[j,i];
            }
        }
    }

    //вывод массива A
    if (flagNotSymA == true)
    { 
    Console.WriteLine("Матрица A:");
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            Console.Write(arrayA[i,j] + " ");
        }
    Console.WriteLine();
    }
}

    // оставим только то, что под нижней диагональю матрицы A
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if (i < j)
                arrayA[i,j] = 0;
        }
    }

    //вывод массива A
    Console.WriteLine("Матрица A, с которой будем работать:");
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            Console.Write(arrayA[i,j] + " ");
        }
    Console.WriteLine();
    }

// ввод массива B
for (int i = 0; i < n; i++)
    {     
        for (int j = 0; j < n; j++)
        {
            Console.Write("Введите элемент [" + i + "][" + j + "]  массива B: ");
            arrayB[i, j] = Convert.ToInt32(Console.ReadLine());
        }
    }

// проверка на симметричность и заодно вывод матрицы
bool flagNotSymB = false;
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        if (arrayB[i, j] != arrayB[j, i])
            flagNotSymB = true;
        Console.Write(arrayB[i, j] + " ");
    }
    Console.WriteLine();
}

if (flagNotSymB == false)
    Console.WriteLine("Матрица B симметрична");
else
    Console.WriteLine("Матрица B не симметрична. Она будет преобразована в симметричную.");

// преобразование матрицы в симметричную
if (flagNotSymB == true)
{
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            arrayB[i, j] = arrayB[j, i];
        }
    }
}

//вывод массива B
if (flagNotSymB == true)
{
    Console.WriteLine("Матрица B:");
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            Console.Write(arrayB[i, j] + " ");
        }
        Console.WriteLine();
    }
}

// оставим только то, что под нижней диагональю матрицы B
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        if (i < j)
            arrayB[i, j] = 0;
    }
}

//вывод массива B
Console.WriteLine("Матрица B, с которой будем работать:");
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        Console.Write(arrayB[i, j] + " ");
    }
    Console.WriteLine();
}

    // флаг будет использоваться, чтобы отмечать, что количество "неразреженных" элементах в матрице A и B подсчитано
    bool flagCountedD = false;

    // заполняем матрицу A.D
    arrayAD[0] = 1;

for (int i = 1; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        if ((flagCountedD == false) && (arrayA[i, j] != 0))
        {
            arrayAD[i] = i - j; // расчёт количества ненулевых элементов в каждой последующей строке
            flagCountedD = true; // если посчитали в строке - далее уже не считаем
        }
    }
    // сбрасываем флаг каждую строчку для расчёта ненулевых элементов по новой
    arrayAD[i]  += arrayAD[i - 1] + 1; // плюс один -  за элемент для каждой диагонали
    flagCountedD = false; // сбрасываем флаг каждую строчку для расчёта ненулевых элементов по новой строке
}

Console.WriteLine("Матрица A.D: ");
    for (int i = 0; i < n; i++)
    {
        Console.Write(arrayAD[i] + " ");
    }
    Console.WriteLine();

    // заполняем матрицу B.D
    arrayBD[0] = 1;

    for (int i = 1; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if ((flagCountedD == false) && (arrayB[i, j] != 0))
            {
                arrayBD[i] = i - j; // расчёт количества ненулевых элементов в каждой последующей строке
                flagCountedD = true; // если посчитали в строке - далее уже не считаем
            }
        }
    arrayBD[i] += arrayBD[i - 1] + 1; // плюс один -  за элемент для каждой диагонали
    flagCountedD = false; // сбрасываем флаг каждую строчку для расчёта ненулевых элементов по новой строке
}

    Console.WriteLine("Матрица B.D: ");
    for (int i = 0; i < n; i++)
    {
        Console.Write(arrayBD[i] + " ");
    }
    Console.WriteLine();

// создадим матрица A.AN и B.AN
    int[] arrayAAN = new int[arrayAD[n - 1]]; // памяти на это - как в последней ячейке D
    int[] arrayBAN = new int[arrayBD[n - 1]];

    int counter = 1;
    // заполним их

    arrayAAN[0] = arrayA[0,0]; // первый элемент
    for (int i = 1; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if (i > (i - (arrayAD[i] - arrayAD[i - 1]) + j))
            {
                arrayAAN[counter] = arrayA[i,(i - (arrayAD[i] - arrayAD[i - 1]) + j + 1)];
                counter++;
            }
        }
    }

    Console.WriteLine("Массив A.AN: ");
    for (int i = 0; i < arrayAD[n - 1]; i++)
    {
        Console.Write(arrayAAN[i] + " ");
    }
    Console.WriteLine();

    counter = 1;
    arrayBAN[0] = arrayB[0,0]; // первый элемент
    for (int i = 1; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if (i > (i - (arrayBD[i] - arrayBD[i - 1]) + j))
            {
                arrayBAN[counter] = arrayB[i,(i - (arrayBD[i] - arrayBD[i - 1]) + j + 1)];
                counter++;
            }
        }
    }

    Console.WriteLine("Массив B.AN: ");
    for (int i = 0; i < arrayBD[n - 1]; i++)
    {
        Console.Write(arrayBAN[i] + " ");
    }

Console.WriteLine();

    // заполним первый элемент матрицы C.D
    arrayCD[0] = 1;

    // создадим список для хранения матрицы C.AN
    // проинициализируем его первый элемент суммой первых элементов матрицы A и B
    List<int> listCAN = new List<int>() { arrayA[0,0] + arrayB[0,0] };
    // далее следует основная обработка
    int counterA = 1; // счётчик для A.AN
    int counterB = 1; // счётчик для B.AN
    int counterC = 1; // общий счётчик для C.AN

    bool notZeros; // флаг для определения, есть ли в строке матрицы C ненулевые элементы
    int numOfPairsForEachRow; // счётчик количества "парных" чисел в каждой строке, т.е. имеющих одинаковые i, j и не равные нулю 
    for (int i = 1; i < n; i++)
    {
            notZeros = false;
        // переписываем непарные из A (если есть)
            if ((arrayAD[i] - arrayAD[i - 1]) > (arrayBD[i] - arrayBD[i - 1])) 
            {
                // переписываем столько непарных, насколько A.D[i] больше B.D[i]
                for (int k = 0; k < ((arrayAD[i] - arrayAD[i - 1]) - (arrayBD[i] - arrayBD[i - 1])); k++)
                {
                    listCAN.Add(arrayAAN[counterA]);
                    counterA++;
                    counterC++;
                    notZeros = true;
                }
            }

        // переписываем непарные из B (если есть)
            if ((arrayAD[i] - arrayAD[i - 1]) < (arrayBD[i] - arrayBD[i - 1])) 
            {
                // переписываем столько непарных, насколько B.D[i] больше A.D[i]
                for (int k = 0; k < ((arrayBD[i] - arrayBD[i - 1]) - (arrayAD[i] - arrayAD[i - 1])); k++)
                {
                    listCAN.Add(arrayBAN[counterB]);
                    counterB++;
                    counterC++;
                    notZeros = true;
                }
            }

            // дальше надо обработать парные элементы
            // таких элементов в каждой строке будет
            numOfPairsForEachRow = Math.Min( (arrayAD[i] - arrayAD[i - 1]), (arrayBD[i] - arrayBD[i - 1]) );


            //Console.WriteLine("В строке матрицы C с индексом i=" + i + " находится" + numOfPairsForEachRow + " парных элементов");
            for(int p = 0; p < numOfPairsForEachRow; p++)
            {
           
             if (((arrayAAN[counterA] + arrayBAN[counterB]) != 0)) // если сумма не равна 0
            {
                // то дальше просто добавляем сумму парных элементов
                listCAN.Add((arrayAAN[counterA] + arrayBAN[counterB]));
                counterA++;
                counterB++;
                counterC++;
                notZeros=true;
            }

            else
            {     
            // если это диагональные элементы, то в любом случае добавляем их сумму
            if ( (arrayAAN[arrayAD[i] - 1] == arrayAAN[counterA]) 
                && (arrayBAN[arrayBD[i] - 1 ] == arrayBAN[counterB]))
            {
                    listCAN.Add((arrayAAN[counterA] + arrayBAN[counterB]));
                    counterA++;
                    counterB++;
                    counterC++;
            }
            else
            {
             //если сумма = 0 и это не диагональные элементы и в C.AN уже есть элементы для i-й строки
            if ( ((arrayAAN[counterA] + arrayBAN[counterB]) == 0) 
                    && (arrayAD[i] - 1 != counterA) 
                    && (arrayBD[i] - 1 != counterB)
                    && notZeros == true)
                {
                    listCAN.Add((arrayAAN[counterA] + arrayBAN[counterB]));
                    counterA++;
                    counterB++;
                    counterC++;
                }
                else
                {
                    // иначе (т.е. когда получается нулевая сумма ненулевых элементов, а в ряду матрицы C ничего не записано)
                    // просто идём дальше по счётчику элементов в A.AN и B.AN
                    counterA++;
                    counterB++;
                }
            }
            }

            }
    arrayCD[i] = counterC; // записываем значение счётчика C в массив C.D
}


    Console.WriteLine("Матрица C.AN:");
for (int i = 0; i < arrayCD[n-1]; i++)
{
    Console.Write(listCAN[i] + " ");
}
    Console.WriteLine();

    Console.WriteLine("Матрица C.D:");
for (int i = 0; i < n; i++)
{
    Console.Write(arrayCD[i] + " ");
}
    Console.WriteLine();

    // теперь распакуем данные в массив C
    // проинициализируем нулями
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)

        {
        arrayC[i, j] = 0;
        }
    }
    // скопируем первый элемент
    arrayC[0, 0] = listCAN[0];

    int d;
    for (int i = 1; i < n; i++) // перебор по строкам
    {
        d = i; // начинаем с диагонали
        counterC = arrayCD[i] - 1;
        for (int j = 0; j < arrayCD[i] - arrayCD[i - 1]; j++) // перебор по столбцам
        {
            arrayC[i, d] = listCAN[counterC]; // начинаем с диагонали
            d--;   // двигаемся влево по строке матрицы
            counterC--; //двигаемся влево по массиву C.AN для получения элементов
        }
    }

//вывод массива C
Console.WriteLine("Матрица C:");
for (int i = 0; i < n; i++)
{
    for (int j = 0; j < n; j++)
    {
        Console.Write(arrayC[i, j] + " ");
    }
    Console.WriteLine();
}
