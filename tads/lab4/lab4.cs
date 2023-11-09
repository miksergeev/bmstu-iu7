using System.Xml;

Console.WriteLine("Схема Чанга-Густавсона");

void Sum()
{
    Console.Write("Введите количество строк: ");
    int m = Convert.ToInt32(Console.ReadLine());
    Console.Write("Введите количество столбцов: ");
    int n = Convert.ToInt32(Console.ReadLine());

    // выделяем память под матрицу A
    int[,] arrayA = new int[m, n];

    // выделяем память под матрицу B
    int[,] arrayB = new int[m, n];

    // выделяем память под матрицу C = A + B
    int[,] arrayC = new int[m, n];

    // ввести вручную все или только ненулевые?
    Console.WriteLine("1 - ввести все значения матрицы");
    Console.WriteLine("2 - ввести только ненулевые значение");
    char choice = Convert.ToChar(Console.ReadLine());

    switch (choice)
    {
        case '1':
            {
                // ввод значений для матрицы A
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Console.Write("Введите элемент [" + i + "][" + j + "]  массива A: ");
                        arrayA[i, j] = Convert.ToInt32(Console.ReadLine());
                    }
                }

                // ввод значений для матрицы B
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Console.Write("Введите элемент [" + i + "][" + j + "]  массива B: ");
                        arrayB[i, j] = Convert.ToInt32(Console.ReadLine());
                    }
                }
                break;
            }
        case '2':
            {
                // обнуляем массивы A и B
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        arrayA[i, j] = 0;
                        arrayB[i, j] = 0;
                    }
                }
                // ввод определённых значений для матрицы

                //Ввод для A
                Console.WriteLine("Вы вводите значения элементов массива A. " +
                                  "Для выхода введите номер строки/столбца больший, чем есть в таблице");
                for (int i = 0, j = 0; ;)
                {
                    Console.WriteLine("Номер строки (начиная с 0):");
                    i = Convert.ToInt32(Console.ReadLine());
                    if (i > m) break;
                    Console.WriteLine("Номер столбца (начиная с 0):");
                    j = Convert.ToInt32(Console.ReadLine());
                    if (j > n) break;
                    Console.WriteLine("Значение элемента:");
                    arrayA[i, j] = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Значение элемента массива A[" + i + "][" + j + "] равно " + arrayA[i, j]);
                }

                //Ввод для B
                Console.WriteLine("Вы вводите значения элементов массива B. " +
                                  "Для выхода введите номер строки/столбца больший, чем есть в таблице");
                for (int i = 0, j = 0; ;)
                {
                    Console.WriteLine("Номер строки (начиная с 0):");
                    i = Convert.ToInt32(Console.ReadLine());
                    if (i >= m) break;
                    Console.WriteLine("Номер столбца (начиная с 0):");
                    j = Convert.ToInt32(Console.ReadLine());
                    if (j >= n) break;
                    Console.WriteLine("Значение элемента:");
                    arrayB[i, j] = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Значение элемента массива B[" + i + "][" + j + "] равно " + arrayB[i, j]);
                }
            }
            break;
    }


    // упакуем A в A.AN, A.JA и A.JR
    List<int> listAAN = new List<int>();
    List<int> listAJA = new List<int>();
    int[] arrayAJR = new int[m + 1];
    int counterA = 1;
    arrayAJR[0] = counterA;

    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if (arrayA[i, j] != 0)
            {
                listAAN.Add(arrayA[i, j]); // упаковка элементов матрицы в A.AN
                listAJA.Add(j); // упаковка значений величины столбцов в A.JA
                counterA++; // увеличение счётчика при каждом ненулевом элементе
            }
        }
        arrayAJR[i + 1] = counterA; // упаковка количества ненулевых элементов в A.JR
    }

    // упакуем B в B.AN, B.JA и B.JR
    List<int> listBAN = new List<int>();
    List<int> listBJA = new List<int>();
    int[] arrayBJR = new int[m + 1];
    int counterB = 1;
    arrayBJR[0] = counterB;

    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if (arrayB[i, j] != 0)
            {
                listBAN.Add(arrayB[i, j]); // упаковка элементов матрицы в B.AN
                listBJA.Add(j); // упаковка значений величины столбцов в B.JA
                counterB++; // увеличение счётчика при каждом ненулевом элементе
            }
        }
        arrayBJR[i + 1] = counterB; // упаковка количества ненулевых элементов в B.JR
    }

    // вывод A.AN, A.JA и A.JR
    Console.WriteLine("Матрица A.AN:");
    for (int i = 0; i < (arrayAJR[m] - 1); i++)
    {
        Console.Write(listAAN[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица A.JA:");
    for (int i = 0; i < (arrayAJR[m] - 1); i++)
    {
        Console.Write(listAJA[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица A.JR:");
    for (int i = 0; i <= m; i++)
    {
        Console.Write(arrayAJR[i] + " ");
    }
    Console.WriteLine();

    // вывод B.AN, B.JA и B.JR
    Console.WriteLine("Матрица B.AN:");
    for (int i = 0; i < (arrayBJR[m] - 1); i++)
    {
        Console.Write(listBAN[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица B.JA:");
    for (int i = 0; i < (arrayBJR[m] - 1); i++)
    {
        Console.Write(listBJA[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица B.JR:");
    for (int i = 0; i <= m; i++)
    {
        Console.Write(arrayBJR[i] + " ");
    }
    Console.WriteLine();

    // сложение упакованных матриц A и B в упакованную матрицу C

    // счётчики пробегают вправо по соответствующим массивам
    int counterAJR = 0; // счётчик по A.JR
    int counterBJR = 0; // счётчик по B.JR
    int counterAJA = 0; // счётчик по A.JA
    int counterBJA = 0; // счётчик по B.JA

    List<int> listСAN = new List<int>(); //C.AN в виде списка
    List<int> listСJA = new List<int>(); //C.JA в виде списка
    int[] arrayСJR = new int[m + 1]; // выделяем память под C.JR
    arrayСJR[0] = 1; // начальный элемент массива C.JR

    for (int i = 1; i <= m; i++)
    {
        // на каждом шаге цикла counterAJR и counterBJR - количество ненулевых элементов в соответствующей строке матриц A и B
        counterAJR = arrayAJR[i] - arrayAJR[i - 1];
        counterBJR = arrayBJR[i] - arrayBJR[i - 1];
        arrayСJR[i] = arrayСJR[i - 1];

        for (; ; )
        {
            if ((counterAJR != 0) && (counterBJR != 0)) // если не обработали все ненулевые элементы в A и B
            {
                if (listAJA[counterAJA] < listBJA[counterBJA])
                {
                    listСAN.Add(listAAN[counterAJA]);
                    counterAJR--;
                    listСJA.Add(listAJA[counterAJA]);
                    arrayСJR[i]++;
                    counterAJA++;
                }
                else
                {
                    if (listAJA[counterAJA] > listBJA[counterBJA])
                    {
                        listСAN.Add(listBAN[counterBJA]);
                        counterBJR--;
                        listСJA.Add(listBJA[counterBJA]);
                        arrayСJR[i]++;
                        counterBJA++;
                    }
                    else
                    {
                        if (listAJA[counterAJA] == listBJA[counterBJA]) // сравниваем элементы, стоящие в столбцах с одинаковым номером
                        {
                            if (listAAN[counterAJA] + listBAN[counterBJA] == 0) // если сумма 0 - не пишем, просто увеличиваем счётчики
                            {
                                counterAJA++;
                                counterBJA++;
                                counterAJR--;
                                counterBJR--;
                            }
                            else
                            {
                                listСAN.Add(listAAN[counterAJA] + listBAN[counterBJA]); // если не 0 - пишем в C.AN
                                listСJA.Add(listAJA[counterAJA]); // и C.JA
                                arrayСJR[i]++; // и увеличиваем количество ненулевых в C.JR
                                counterAJA++;
                                counterBJA++;
                                counterAJR--;
                                counterBJR--;
                            }
                        }
                    }
                }

            }

            if ((counterAJR != 0) && (counterBJR == 0)) // если в строке одной матрице закончились ненулевые элементы
            {
                listСAN.Add(listAAN[counterAJA]); // то просто добавляем ненулевые элементы из строки другой матрицы
                counterAJR--;
                listСJA.Add(listAJA[counterAJA]); // не забывая увеличить C.JA
                arrayСJR[i]++;
                counterAJA++;
            }
            else
            {
                if ((counterAJR == 0) && (counterBJR != 0)) // то же самое
                {

                    listСAN.Add(listBAN[counterBJA]);
                    counterBJR--;
                    listСJA.Add(listBJA[counterBJA]);
                    arrayСJR[i]++;
                    counterBJA++;
                }
            }
            if ((counterAJR == 0) && (counterBJR == 0)) // если ненулевые элементы закончились - выходим из цикла
                break;
        }


    }

    // вывод C.AN
    Console.WriteLine("Матрица C.AN:");
    for (int i = 0; i < (arrayСJR[m] - 1); i++)
    {
        Console.Write(listСAN[i] + " ");
    }
    Console.WriteLine();

    // вывод C.JA
    Console.WriteLine("Матрица C.JA:");
    for (int i = 0; i < (arrayСJR[m] - 1); i++)
    {
        Console.Write(listСJA[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица C.JR:");
    for (int i = 0; i <= m; i++)
    {
        Console.Write(arrayСJR[i] + " ");
    }
    Console.WriteLine();

    // распаковка массива C

    // инициализация нулями
    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < n; j++)
        {
            arrayC[i, j] = 0;
        }
    }

    // распаковка ненулевых элементов
    int counterCJA = 0;
    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < (arrayСJR[i + 1] - arrayСJR[i]); j++)
        {
            arrayC[i, listСJA[counterCJA]] = listСAN[counterCJA];
            counterCJA++;
        }
    }

    Console.WriteLine("Матрица C:");
    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < n; j++)
        {

            Console.Write(arrayC[i, j] + " ");
        }
        Console.WriteLine();
    }
}

void Prod()
{
    Console.WriteLine("Введите m (число строк первой матрицы)");
    int m = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("Введите n (число столбцов первой матрицы / число строк второй матрицы)");
    int n = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("Введите q (число столбцов второй матрицы)");
    int q = Convert.ToInt32(Console.ReadLine());


    // выделяем память под матрицу A
    int[,] arrayA = new int[m, n];

    // выделяем память под матрицу B
    int[,] arrayB = new int[n, q];

    // выделяем память под матрицу C = A * B
    int[,] arrayC = new int[m, q];

    // ввести вручную все или только ненулевые?
    Console.WriteLine("Выберите опцию ввода значений матриц");
    Console.WriteLine("1 - ввести все значения матриц");
    Console.WriteLine("2 - ввести только ненулевые значения матриц");
    char choice = Convert.ToChar(Console.ReadLine());

    switch (choice)
    {
        case '1':
            {
                // ввод значений для матрицы A
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Console.Write("Введите элемент [" + i + "][" + j + "]  массива A: ");
                        arrayA[i, j] = Convert.ToInt32(Console.ReadLine());
                    }
                }

                // ввод значений для матрицы B
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < q; j++)
                    {
                        Console.Write("Введите элемент [" + i + "][" + j + "]  массива B: ");
                        arrayB[i, j] = Convert.ToInt32(Console.ReadLine());
                    }
                }
                break;
            }
        case '2':
            {
                // обнуляем массивы A и B
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        arrayA[i, j] = 0;
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < q; j++)
                    {
                        arrayB[i, j] = 0;
                    }
                }
                // ввод определённых значений для матрицы

                //Ввод для A
                Console.WriteLine("Вы вводите значения элементов массива A. " +
                                  "Для выхода введите номер строки/столбца больший, чем есть в таблице");
                for (int i = 0, j = 0; ;)
                {
                    Console.WriteLine("Номер строки (начиная с 0):");
                    i = Convert.ToInt32(Console.ReadLine());
                    if (i >= m) break;
                    Console.WriteLine("Номер столбца (начиная с 0):");
                    j = Convert.ToInt32(Console.ReadLine());
                    if (j >= n) break;
                    Console.WriteLine("Значение элемента:");
                    arrayA[i, j] = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Значение элемента массива A[" + i + "][" + j + "] равно " + arrayA[i, j]);
                }

                //Ввод для B
                Console.WriteLine("Вы вводите значения элементов массива B. " +
                                  "Для выхода введите номер строки/столбца больший, чем есть в таблице");
                for (int i = 0, j = 0; ;)
                {
                    Console.WriteLine("Номер строки (начиная с 0):");
                    i = Convert.ToInt32(Console.ReadLine());
                    if (i >= n) break;
                    Console.WriteLine("Номер столбца (начиная с 0):");
                    j = Convert.ToInt32(Console.ReadLine());
                    if (j >= q) break;
                    Console.WriteLine("Значение элемента:");
                    arrayB[i, j] = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Значение элемента массива B[" + i + "][" + j + "] равно " + arrayB[i, j]);
                }
            }
            break;
    }

    // вывод значений матриц
    // A
    Console.WriteLine("Матрица A:");
    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < n; j++)
        {
            Console.Write(arrayA[i, j] + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    // B
    Console.WriteLine("Матрица B:");
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < q; j++)
        {
            Console.Write(arrayB[i, j] + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    //BT
    Console.WriteLine("Транспонированная матрица B (назовём её BT) выглядела бы вот так:");
    for (int j = 0; j < q; j++)
    {
        for (int i = 0; i < n; i++)
        {
            Console.Write(arrayB[i, j] + " ");
        }
        Console.WriteLine();
    }

    // упакуем A в A.AN, A.JA и A.JR
    List<int> listAAN = new List<int>();
    List<int> listAJA = new List<int>();
    int[] arrayAJR = new int[m + 1];
    int counterA = 1;
    arrayAJR[0] = counterA;

    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if (arrayA[i, j] != 0)
            {
                listAAN.Add(arrayA[i, j]); // упаковка элементов матрицы в A.AN
                listAJA.Add(j); // упаковка значений величины столбцов в A.JA
                counterA++; // увеличение счётчика при каждом ненулевом элементе
            }
        }
        arrayAJR[i + 1] = counterA; // упаковка количества ненулевых элементов в A.JR
    }

    // упакуем B в B.AN, B.JA и B.JR
    List<int> listBAN = new List<int>();
    List<int> listBJA = new List<int>();
    int[] arrayBJR = new int[n + 1];
    int counterB = 1;
    arrayBJR[0] = counterB;

    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < q; j++)
        {
            if (arrayB[i, j] != 0)
            {
                listBAN.Add(arrayB[i, j]); // упаковка элементов матрицы в B.AN
                listBJA.Add(j); // упаковка значений величины столбцов в B.JA
                counterB++; // увеличение счётчика при каждом ненулевом элементе
            }
        }
        arrayBJR[i + 1] = counterB; // упаковка количества ненулевых элементов в B.JR
    }

    // А ещё упакуем B в транспонированном виде в BT.AN, BT.JA и BT.JR
    List<int> listBTAN = new List<int>();
    List<int> listBTJA = new List<int>();
    int[] arrayBTJR = new int[q + 1];
    int counterBT = 1;
    arrayBTJR[0] = counterBT;

    for (int j = 0; j < q; j++)
    {
        for (int i = 0; i < n; i++)
        {
            if (arrayB[i, j] != 0)
            {
                listBTAN.Add(arrayB[i, j]); // упаковка элементов матрицы в B.AN
                counterBT++; // увеличение счётчика при каждом ненулевом элементе
                listBTJA.Add(i); // упаковка значений величины столбцов в B.JA
            }
        }
        arrayBTJR[j + 1] = counterBT; // упаковка количества ненулевых элементов в B.JR
    }

    // вывод A.AN, A.JA и A.JR
    Console.WriteLine("Матрица A.AN:");
    for (int i = 0; i < (arrayAJR[m] - 1); i++)
    {
        Console.Write(listAAN[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица A.JA:");
    for (int i = 0; i < (arrayAJR[m] - 1); i++)
    {
        Console.Write(listAJA[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица A.JR:");
    for (int i = 0; i <= m; i++)
    {
        Console.Write(arrayAJR[i] + " ");
    }
    Console.WriteLine();

    // вывод B.AN, B.JA и B.JR
    Console.WriteLine("Матрица B.AN:");
    for (int i = 0; i < (arrayBJR[n] - 1); i++)
    {
        Console.Write(listBAN[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица B.JA:");
    for (int i = 0; i < (arrayBJR[n] - 1); i++)
    {
        Console.Write(listBJA[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица B.JR:");
    for (int i = 0; i <= n; i++)
    {
        Console.Write(arrayBJR[i] + " ");
    }
    Console.WriteLine();

    // вывод BT.AN, BT.JA и BT.JR
    Console.WriteLine("Матрица BT.AN:");
    for (int i = 0; i < (arrayBTJR[q] - 1); i++)
    {
        Console.Write(listBTAN[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица BT.JA:");
    for (int i = 0; i < (arrayBTJR[q] - 1); i++)
    {
        Console.Write(listBTJA[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица BT.JR:");
    for (int i = 0; i <= q; i++)
    {
        Console.Write(arrayBTJR[i] + " ");
    }
    Console.WriteLine();

    // логические массивы 0/1
    int[] rowAbool = new int[n];
    int[] rowBbool = new int[n];
    int[] rowCbool = new int[n];

    // и ещё результирующие счётчики
    int counterResultAAN = 0;
    int counterResultBAN = 0;
    int counterResultBTAN = 0;
    int counterResultCAN = 0;
    int counterResultAJA = 0;
    int counterResultBJA = 0;
    int counterResultBTJA = 0;
    int counterResultCJA = 0;
    int counterResultAJR = 1;
    int counterResultBJR = 1;
    int counterResultBTJR = 1;
    int counterResultCJR = 1;

    // переменные для умножение
    int resultC = 0;

    //переменная массива C
    int elementC = 0;

    // для упаковки полученного произведения C ( = A x B )
    List<int> listCAN = new List<int>();
    List<int> listCJA = new List<int>();
    int[] arrayCJR = new int[m + 1];
    int counterC = 1;
    arrayCJR[0] = counterC;

    for (int i = 0; i < m; i++) // m
    {
        // обнуляем счётчики для BT
        counterResultBTJR = 1;
        counterResultBTJA = 0;
        counterResultBTAN = 0;

        // задаём счётчики для A
        counterResultAJA = /*arrayAJR[i + 1] -*/ arrayAJR[i] - 1;
        counterResultAAN = arrayAJR[i] - 1;
        counterResultAJR = i + 1; // попробуем так

        // обнуляем текущую переменную массива C
        elementC = 0;
        resultC = 0;


        // в начале каждого цикла обнуляем булевую матрицу A
        for (int p = 0; p < n; p++)
        {
            rowAbool[p] = 0;
        }

        // дальше проставляем в ней единицы только для ненулевых элементов
        for (int l = 0; l < arrayAJR[counterResultAJR] - arrayAJR[counterResultAJR - 1]; l++)
        {
            rowAbool[listAJA[counterResultAJA]] = 1;
            counterResultAJA++;

        }
        // и прибавляем счётчики
        //counterResultAJR++;


        for (int j = 0; j < q; j++)
        {
            // задаём счётчики для A
            counterResultAJA = arrayAJR[i + 1] - arrayAJR[i] - 1;
            counterResultAAN = arrayAJR[i] - 1;

            //counterResultBTAN = 0;
            elementC = 0;

            // в начале каждого цикла обнуляем булевые матрицы B и C
            for (int p = 0; p < n; p++)
            {
                rowBbool[p] = 0;
                rowCbool[p] = 0;
            }

            // выведем, что у нас получилось 
            Console.WriteLine(); // отладка
            Console.WriteLine("i = " + i + " , j = " + j);
            Console.WriteLine("counterResultAAN = " + counterResultAAN +
                              " , counterResultAJA = " + counterResultAJA +
                              " , counterResultAJR = " + counterResultAJR);
            Console.WriteLine("counterResultBTAN = " + counterResultBTAN +
                  " , counterResultBTJA = " + counterResultBTJA +
                  " , counterResultBTJR = " + counterResultBTJR);

            // дальше проставляем в булевой матрице B единицы только для ненулевых элементов
            for (int l = 0; l < arrayBTJR[counterResultBTJR] - arrayBTJR[counterResultBTJR - 1]; l++)
            {
                rowBbool[listBTJA[counterResultBTJA]] = 1;
                counterResultBTJA++;
            }
            // и прибавляем счётчики
            counterResultBTJR++;

            // тут получим, какие надо перемножать. Это поможет потом - будем перемножать только нужные
            Console.WriteLine("Булевая матрица строки " + i + " матрицы A:"); // отладка
            for (int abc = 0; abc < n; abc++)
            {
                Console.Write(rowAbool[abc] + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Булевая матрица строки " + j + " матрицы B:"); // отладка
            for (int abc = 0; abc < n; abc++)
            {
                Console.Write(rowBbool[abc] + " ");
            }
            Console.WriteLine();

            Console.WriteLine("Теперь"); // отладка
            Console.WriteLine("counterResultAAN = " + counterResultAAN +
                              " , counterResultAJA = " + counterResultAJA +
                              " , counterResultAJR = " + counterResultAJR);
            Console.WriteLine("counterResultBTAN = " + counterResultBTAN +
                  " , counterResultBTJA = " + counterResultBTJA +
                  " , counterResultBTJR = " + counterResultBTJR);
            Console.WriteLine();


            for (int k = 0; k < n; k++)
            {

                if (rowAbool[k] == 1 && rowBbool[k] == 0)
                {
                    counterResultAAN++;
                    counterResultAJA++;
                }

                if (rowAbool[k] == 0 && rowBbool[k] == 1)
                {
                    counterResultBTAN++;
                    counterResultBJA++;
                }

                if ((rowAbool[k] == 1) && (rowBbool[k] == 1))
                {
                    rowCbool[k] = 1;
                    resultC = listAAN[counterResultAAN] * listBTAN[counterResultBTAN];
                    Console.WriteLine("resultC = " + listAAN[counterResultAAN] + " * " + listBTAN[counterResultBTAN]);
                    elementC += resultC;
                    counterResultAAN++;
                    counterResultBTAN++; // попробуем так
                    //counterResultBTJA++; // попробуем так
                }
            }


            // присвоение значений матрицы - использовалось на этапе отладки, оставлено
            arrayC[i, j] = elementC;

            // упаковка произведения НЕ из матрицы, а в ходе произведения
            if (elementC != 0)
            {
                listCAN.Add(elementC); // упаковка элементов матрицы в C.AN
                counterC++; // увеличение счётчика при каждом ненулевом элементе
                listCJA.Add(j); // упаковка значений величины столбцов в C.JA
            }

        }
        arrayCJR[i + 1] = counterC;
    }

    // Вывод последней битовой маски по матрице A (использовалось на этапе отладки)
    for (int k = 0; k < n; k++)
    {
        Console.Write(rowAbool[k]);
        Console.Write(" ");
    }
    Console.WriteLine();

    // Вывод последней битовой маски по матрице B (использовалось на этапе отладки)
    for (int k = 0; k < n; k++)
    {
        Console.Write(rowBbool[k]);
        Console.Write(" ");
    }
    Console.WriteLine();

    // Вывод матрицы C
    Console.WriteLine("Матрица C:");
    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < q; j++)
        {
            Console.Write(arrayC[i, j] + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();

    // вывод C.AN, C.JA и C.JR
    Console.WriteLine("Матрица C.AN:");
    for (int i = 0; i < (arrayCJR[m] - 1); i++)
    {
        Console.Write(listCAN[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица C.JA:");
    for (int i = 0; i < (arrayCJR[m] - 1); i++)
    {
        Console.Write(listCJA[i] + " ");
    }
    Console.WriteLine();

    Console.WriteLine("Матрица C.JR:");
    for (int i = 0; i <= m; i++)
    {
        Console.Write(arrayCJR[i] + " ");
    }
    Console.WriteLine();

}

int Main()
{
    Console.WriteLine("Выберите режим программы:");
    Console.WriteLine("1 - Сложение");
    Console.WriteLine("2 - Умножение");
    Console.WriteLine("3 - Выход");

    int choice = Convert.ToInt32(Console.ReadLine());
    switch (choice)
    {
        case 1:
            { Sum(); }
            break;
        case 2:
            { Prod(); }
            break;
        case 3:
            { return 0; }
    }
    return 0;
}

Main();
