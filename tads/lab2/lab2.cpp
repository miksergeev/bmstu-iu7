#include <iostream>
#include "stdlib.h"
#include <chrono>
using namespace std;

void startMessage()
{
    std::cout << "Эта программа позволяет выполнить умножение матриц, а также сравнить время выполнения трёх алгоритмов" << std::endl;
}

void timeMode()
{
    std::cout << "Для удобства графического представления эффективности работы алгоритмов " <<
                 "данная программа будет работать с двумя квадратными матрицами (одинаковой размерности)" << std::endl;
    int size = 1;
    for (;size > 0;)
    {
        std::cout << "Введите значение размерности матриц или 0, чтобы вернуться назад: ";
        std::cin >> size;

            int m, n, q; // размерность матрицы
            m = n = q = size;

            int** arrayA;

            arrayA = new int* [m];
            for (int i = 0; i < m; i++)
                arrayA[i] = new int[n];

            int** arrayB;

            arrayB = new int* [n];
            for (int i = 0; i < n; i++)
                arrayB[i] = new int[q];

            int** arrayC;

            arrayC = new int* [m];
            for (int i = 0; i < m; i++)
                arrayC[i] = new int[q];

            //генерация массива A
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    arrayA[i][j] = rand() % 10;
                }

            //генерация массива B
            for (int i = 0; i < n; i++)
                for (int j = 0; j < q; j++)
                {
                    arrayB[i][j] = rand() % 10;
                }

            //инициализация массива C нулями
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < q; j++)
                {
                    arrayC[i][j] = 0;
                }
            }

            // Классический алгоритм
            std::chrono::high_resolution_clock::time_point t1 = std::chrono::high_resolution_clock::now();
            for (int a = 0; a < 20; a++) // цикл для точности замера процессорного времени
            {
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < q; j++)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            arrayC[i][j] = arrayC[i][j] + (arrayA[i][k] * arrayB[k][j]);
                        }
                    }
                }
            }
            std::chrono::high_resolution_clock::time_point t2 = std::chrono::high_resolution_clock::now();
            auto duration1 = std::chrono::duration_cast<std::chrono::microseconds>(t2 - t1).count();
            duration1 /= 20;
            std::cout << "Время выполнения классического алгоритма: " << duration1 << " микросекунд" << std::endl;


            // Классический алгоритм Винограда

            // Вектор mulH
            int* mulH = new int[m]; // Выделение памяти для массива
            for (int i = 0; i < m; i++)
            {
                // Заполнение массива
                mulH[i] = 0;
            }

            // Вектор mulV
            int* mulV = new int[q]; // Выделение памяти для массива
            for (int j = 0; j < q; j++) {
                // Заполнение массива
                mulV[j] = 0;
            }

            std::chrono::high_resolution_clock::time_point t3 = std::chrono::high_resolution_clock::now();
            for (int a = 0; a < 20; a++) // цикл для точности замера процессорного времени
            {
                // Заполняем массив mulH
                for (int i = 0; i < m; i++)
                {
                    for (int k = 0; k < n / 2; k++)
                    {
                        mulH[i] = mulH[i] + arrayA[i][2 * k] * arrayA[i][2 * k + 1];
                    }
                }

                // Заполняем массив mulV
                for (int j = 0; j < q; j++)
                {
                    for (int k = 0; k < n / 2; k++)
                    {
                        mulV[j] = mulV[j] + arrayB[2 * k][j] * arrayB[2 * k + 1][j];
                    }
                }

                // Вычисляем произведение матриц, используя алгоритм Винограда

                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < q; j++)
                    {
                        arrayC[i][j] = -mulH[i] - mulV[j];
                        for (int k = 0; k < n / 2; k++)
                        {
                            arrayC[i][j] = arrayC[i][j] + (arrayA[i][2 * k] + arrayB[2 * k + 1][j]) * (arrayA[i][2 * k + 1] + arrayB[2 * k][j]);
                        }
                    }
                }
            }

            // Дополнение для нечётного N
            if (n % 2 == 1)
            {
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < q; j++)
                    {
                        arrayC[i][j] = arrayC[i][j] + (arrayA[i][n - 1] * arrayB[n - 1][j]);
                    }
                }
            }
            std::chrono::high_resolution_clock::time_point t4 = std::chrono::high_resolution_clock::now();
            auto duration2 = std::chrono::duration_cast<std::chrono::microseconds>(t4 - t3).count();
            duration2 /= 20;
            std::cout << "Время выполнения неоптимизированного алгоритма Винограда: " << duration2 << " микросекунд" << std::endl;

            // Оптимизированный алгоритм Винограда
            for (int i = 0; i < m; i++) {
                // Обнуление вектора mulH
                mulH[i] = 0;
            }

            // Вектор mulV
            for (int j = 0; j < q; j++) {
                // Обнуление вектора mulV
                mulV[j] = 0;
            }

            //инициализация массива C нулями
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < q; j++)
                {
                    arrayC[i][j] = 0;
                }
            }

            std::chrono::high_resolution_clock::time_point t5 = std::chrono::high_resolution_clock::now();
            for (int a = 0; a < 20; a++) // цикл для точности замера процессорного времени
            {
                // Заполняем массив mulH
                for (int i = 0; i < m; i++)
                {
                    for (int k = 0; k < n / 2; k++)
                    {
                        mulH[i] += arrayA[i][2 * k] * arrayA[i][2 * k + 1]; // оптимизация 1
                    }
                }

                // Заполняем массив mulV
                for (int j = 0; j < q; j++)
                {
                    for (int k = 0; k < n / 2; k++)
                    {
                        mulV[j] += arrayB[2 * k][j] * arrayB[2 * k + 1][j]; // оптимизация 1
                    }
                }

                // Вычисляем произведение матриц, используя алгоритм Винограда

                // оптимизация 2
                if (n % 2 == 0) // для чётного N
                {
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < q; j++)
                        {
                            arrayC[i][j] = -mulH[i] - mulV[j];
                            for (int k = /*1; k < n; k+=2)*/0; k < n / 2; k++)
                            {
                                arrayC[i][j] += (arrayA[i][2 * k] + arrayB[2 * k + 1][j]) * (arrayA[i][2 * k + 1] + arrayB[2 * k][j]);
                                   // (arrayA[i][k] + arrayB[k + 1][j]) * (arrayA[i][k + 1] + arrayB[k][j]);
                            }
                        }
                    }
                }
                else // для нечётного N
                {
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < q; j++)
                        {
                            arrayC[i][j] = -mulH[i] - mulV[j] + (arrayA[i][n - 1] * arrayB[n - 1][j]);
                            for (int k = 0; k < n / 2; k++)
                            {
                                arrayC[i][j] += (arrayA[i][2 * k] + arrayB[2 * k + 1][j]) * (arrayA[i][2 * k + 1] + arrayB[2 * k][j])
                                               // + (arrayA[i][n - 1] * arrayB[n - 1][j])
                                    ;
                            }
                        }
                    }
                }
            }
            std::chrono::high_resolution_clock::time_point t6 = std::chrono::high_resolution_clock::now();
            auto duration3 = std::chrono::duration_cast<std::chrono::microseconds>(t6 - t5).count();
            duration3 /= 20;
            std::cout << "Время выполнения оптимизированного алгоритма Винограда: " << duration3 << " микросекунд" << std::endl;

            std::cout << std::endl;
    }
}

void outputMode() // ручной режим
{
    int m, n, q;
    std::cout << "Введите M" << std::endl;
    std::cin >> m;
    std::cout << "Введите N" << std::endl;
    std::cin >> n;
    std::cout << "Введите Q" << std::endl;
    std::cin >> q;

    int** arrayA;

    arrayA = new int* [m];
    for (int i = 0; i < m; i++)
        arrayA[i] = new int[n];

    int** arrayB;

    arrayB = new int* [n];
    for (int i = 0; i < n; i++)
        arrayB[i] = new int[q];

    int** arrayC;

    arrayC = new int* [m];
    for (int i = 0; i < m; i++)
        arrayC[i] = new int[q];

    //инициализация массива C нулями
    for (int i = 0; i < m; i++)
    {
        for (int j = 0; j < q; j++)
        {
            arrayC[i][j] = 0;
        }
    }

        //ввести вручную?
        int choice = 0;
        std::cout << "1 - ввести значения массивов вручную" << std::endl;
        std::cout << "2 - сгенерировать значения массивов автоматически" << std::endl;
        std::cin >> choice;

        switch (choice)
        {
        case 1:
        {
            // ввод массива A
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    std::cout << "Введите элемент " << "[" << i << "][" << j << "]  массива A: ";
                    std::cin >> arrayA[i][j];
                }

            std::cout << endl;

            //вывод массива A
            std::cout << "Массив A:" << std::endl;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    std::cout << arrayA[i][j] << " ";
                }
                std::cout << endl;
            }

            // ввод массива B
            for (int i = 0; i < n; i++)
                for (int j = 0; j < q; j++)
                {
                    std::cout << "Введите элемент " << "[" << i << "][" << j << "]  массива B: ";
                    std::cin >> arrayB[i][j];
                }

            std::cout << endl;

            //вывод массива B
            std::cout << "Массив B:" << std::endl;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < q; j++)
                {
                    std::cout << arrayB[i][j] << " ";
                }
                std::cout << endl;
            }
        }
        break;

        case 2:
        {
            //генерация массива A
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    arrayA[i][j] = rand() % 10;
                }

            //вывод массива A
            std::cout << "Массив A:" << std::endl;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    std::cout << arrayA[i][j] << " ";
                }
                std::cout << endl;
            }

            //генерация массива B
            for (int i = 0; i < n; i++)
                for (int j = 0; j < q; j++)
                {
                    arrayB[i][j] = rand() % 10;
                }

            //вывод массива B
            std::cout << "Массив B:" << std::endl;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < q; j++)
                {
                    std::cout << arrayB[i][j] << " ";
                }
                std::cout << endl;
            }

        }

        }

        // получим C=AxB

        // Классический алгоритм
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < q; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    arrayC[i][j] = arrayC[i][j] + (arrayA[i][k] * arrayB[k][j]);
                }
            }
        }

        //вывод C
        std::cout << "Массив C (классический алгоритм):" << std::endl;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < q; j++)
            {
                std::cout << arrayC[i][j] << " ";
            }
            std::cout << endl;
        }

        // Классический алгоритм Винограда

        // Вектор mulH
        int* mulH = new int[m]; // Выделение памяти для массива
        for (int i = 0; i < m; i++) {
            // Заполнение массива
            mulH[i] = 0;
        }

        // Вектор mulV
        int* mulV = new int[q]; // Выделение памяти для массива
        for (int j = 0; j < q; j++) {
            // Заполнение массива
            mulV[j] = 0;
        }

        // Заполняем массив mulH
        for (int i = 0; i < m; i++)
        {
            for (int k = 0; k < n / 2; k++)
            {
                mulH[i] = mulH[i] + arrayA[i][2 * k] * arrayA[i][2 * k + 1];
            }
        }

        // Заполняем массив mulV
        for (int j = 0; j < q; j++)
        {
            for (int k = 0; k < n / 2; k++)
            {
                mulV[j] = mulV[j] + arrayB[2 * k][j] * arrayB[2 * k + 1][j];
            }
        }

        // Вычисляем произведение матриц, используя алгоритм Винограда
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < q; j++)
            {
                arrayC[i][j] = -mulH[i] - mulV[j];
                for (int k = 0; k < n / 2; k++)
                {
                    arrayC[i][j] = arrayC[i][j] + (arrayA[i][2 * k] + arrayB[2 * k + 1][j]) * (arrayA[i][2 * k + 1] + arrayB[2 * k][j]);
                }
            }
        }

        // Дополнение для нечётного N
        if (n % 2 == 1)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < q; j++)
                {
                    arrayC[i][j] = arrayC[i][j] + (arrayA[i][n - 1] * arrayB[n - 1][j]);
                }
            }
        }

        //вывод C
        std::cout << "Массив C (алгоритм Винограда):" << std::endl;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < q; j++)
            {
                std::cout << arrayC[i][j] << " ";
            }
            std::cout << endl;
        }

        // Оптимизированный алгоритм Винограда

           // Вектор mulH
        for (int i = 0; i < m; i++) {
            // Обнуление вектора mulH
            mulH[i] = 0;
        }

        // Вектор mulV
        for (int j = 0; j < q; j++) {
            // Обнуление вектора mulV
            mulV[j] = 0;
        }

        //инициализация массива C нулями
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < q; j++)
            {
                arrayC[i][j] = 0;
            }
        }

        // Заполняем массив mulH
        for (int i = 0; i < m; i++)
        {
            for (int k = 0; k < n / 2; k++)
            {
                mulH[i] += arrayA[i][2 * k] * arrayA[i][2 * k + 1]; // оптимизация 1
            }
        }

        // Заполняем массив mulV
        for (int j = 0; j < q; j++)
        {
            for (int k = 0; k < n / 2; k++)
            {
                mulV[j] += arrayB[2 * k][j] * arrayB[2 * k + 1][j]; // оптимизация 1
            }
        }

        // Вычисляем произведение матриц, используя алгоритм Винограда

        // для чётного N
        if (n % 2 == 0) // оптимизация 2
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < q; j++)
                {
                    arrayC[i][j] = -mulH[i] - mulV[j] + (arrayA[i][n - 1] * arrayB[n - 1][j]);
                    for (int k = 0; k < n / 2; k++)
                    {
                        arrayC[i][j] += (arrayA[i][2 * k] + arrayB[2 * k + 1][j]) * (arrayA[i][2 * k + 1] + arrayB[2 * k][j]);
                            
                    }
                }
            }
        }
        else // для нечётного N
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < q; j++)
                {
                    arrayC[i][j] = -mulH[i] - mulV[j] + (arrayA[i][n - 1] * arrayB[n - 1][j]); // добавка для нечётного
                    for (int k = 0; k < n / 2; k++)
                    {
                        arrayC[i][j] += (arrayA[i][2 * k] + arrayB[2 * k + 1][j]) * (arrayA[i][2 * k + 1] + arrayB[2 * k][j]);
                    }
                }
            }
        }

        //вывод C
        std::cout << "Массив C (оптимизированный алгоритм Винограда):" << std::endl;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < q; j++)
            {
                std::cout << arrayC[i][j] << " ";
            }
            std::cout << endl;
        }
}

int main()
{
    setlocale(LC_ALL, "Russian");

    startMessage();

    std::cout << "Выберите режим программы:" << std::endl;
    std::cout << "1 - умножить матрицы и получить результат" << std::endl;
    std::cout << "2 - замерить процессорное время для измерения эффективности алгоритмов" << std::endl;
    std::cout << "3 - выйти из программы" << std::endl;

    int choice;
    std::cin >> choice;
    switch (choice)
    {
    case 1:
    {outputMode(); }
    break;
    case 2:
    {timeMode(); }
    break;
    case 3:
    {return 0; }
    }
    main();
}
