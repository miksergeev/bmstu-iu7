// ОБХОД ГРАФА, ИСПОЛЬЗУЯ СТЕК

using System.Drawing;
using System.IO;

Main();

int Main()
{
    // ЧТЕНИЕ ИЗ ФАЙЛА
    String line;

    StreamReader sr = new StreamReader("C:\\Matrix.txt"); // открываем файл
    line = sr.ReadLine(); // считываем первую строку

    int n = line.Count(x => x == ' ') + 1; // число строк/столбцов равно числу элементов в первой строке, разделённых пробелами
    bool[,] arrayMatrix = new bool[n, n]; // создадим квадратный массив типа bool под матрицу смежности

    for (int i = 0; i < n; i++) // заполняем массив из файла
    {
        for (int j = 0; j < n; j++)
        {
            arrayMatrix[i, j] = Convert.ToBoolean(Convert.ToInt32(line.Split(' ')[j])); // пробел - разделитель
            if (j >= i) // исключаем петли и двунаправленность графа
            {
                arrayMatrix[i, j] = false;
            }
            // arrayMatrix[j, i] = arrayMatrix[i, j]; // для графа нужна эта строка, и фактически поиск предка будет ненужным
        }
        line = sr.ReadLine(); // переход к следующей строке
    }
    sr.Close(); // закрываем файл

    // РЕАЛИЗАЦИЯ СТЕКА
    stackElement[] stack = new stackElement[n]; // создаём массив ёмкостью n для хранения стека, причём его нулевой элемент будет содержать количество объектов (которых всего будет n-1)
    stack[0].vertex = 0; // текущее количество объектов в стеке
    stack[0].isObject = false;

    bool[] visitedVertices = new bool[n]; // создаём список посещённых вершин
    List<int> outputOrder = new List<int>(); ; // здесь будет записываться порядок обхода

    // ФУНКЦИИ СТЕКА
    bool isStackFull(stackElement[] stack) // проверка на полноту
    {
        if (stack[0].vertex == n - 1) // если элементов = n-1
            return true; // то стек полон
        else
            return false; // иначе не полон
    }

    bool isStackEmpty(stackElement[] stack) // проверка на пустоту
    {
        if (stack[0].vertex == 0) // если элементов нет
            return true; // то стек пуст
        else
            return false; // иначе не пуст
    }

    bool stackPush(stackElement[] stack, int element) // добавление элемента в стек
    {
        if (isStackFull(stack) != true) // если стек не полон
        {
            ++stack[0].vertex; // увеличиваем счётчик элементов стека
            stack[stack[0].vertex].vertex = element; // инициализируем добавленный элемент стека поступившим значением
            stack[stack[0].vertex].isObject = true;
            return true; // если получилось добавить
        }
        else
        {
            return false; // если не получилось добавить (т.к. стек полон)
        }
    }

    int stackPeek(stackElement[] stack)
    {
        if (isStackEmpty(stack) != true) // если стек не пустой
        {
            return stack[stack[0].vertex].vertex;
        }
        else
        {
            return 0;
        }
    }

    int stackPop(stackElement[] stack) // взятие верхнего элемента: если успешно - вернёт число, если неуспешно - 0 (вершины со значением "0" в графе нет)
    {
        if (isStackEmpty(stack) != true) // если стек не пустой
        {
            stackElement forReturn; // заводим временную переменную
            forReturn = stack[stack[0].vertex]; // кладём значение верхнего элемента во временную переменную
            stack[stack[0].vertex].vertex = 0; // помечаем верхний элемент как удалённый
            stack[stack[0].vertex].isObject = false;
            --stack[0].vertex; // уменьшаем количество элементов
            return forReturn.vertex;
        }
        else
        {
            return 0;
        }
    }

    Console.WriteLine("Введите номер стартовой вершины");
    int startPoint = Convert.ToInt32(Console.ReadLine());
    if (startPoint > n || startPoint < 1)
    {
        Console.WriteLine("Стартовая вершина с таким номером отсутствует в заданном графе");
        return -1;
    }
    else
    {
        stackPush(stack, startPoint); // помещаем в стек стартовую точку
        bool added = false; // флаг, что получилось добавить
        int numOfDescendants = 0;
        for(; isStackEmpty(stack) == false;) // запускаем цикл, который будет продолжаться, пока стек не опустеет
        {
            
            int currentVertice = stackPop(stack); // берём с верха стека вершину
            outputOrder.Add(currentVertice); // добавляем в порядок обхода
            visitedVertices[currentVertice - 1] = true; // добавляем в список посещённых

            // добавляем в стек всех непосещённых потомков
            added = false; // флаг, что получилось добавить потомка
            numOfDescendants = 0;
            for (int i = n - 1; i >= 0; i--) // для этого просматриваем матрицу смежности в обратном порядке
            {
                if (arrayMatrix[i, currentVertice-1] == true && visitedVertices[i] == false) // если есть связь и потомок непосещённый
                {
                    stackPush(stack, i + 1); // добавляем в стек номер вершины
                    visitedVertices[i] = true; // отмечаем эту вершину посещённой
                    added = true;
                }
                if (arrayMatrix[i, currentVertice - 1] == true) // если есть потомок
                {
                    ++numOfDescendants; // увеличиваем число потомков
                }
            }

            if (numOfDescendants == 0) // если потомков у данной точки нет
            {
                currentVertice = stackPop(stack); // берём с верха стека вершину
                outputOrder.Add(currentVertice);
            }

            // если непосещённых потомков нет (например, мы смотрим дерево "с середины") - добавляем ближайшего непосещённого предка вершины
            if (added == false)
            {
                    for (int j = n - 1; j >= 0; --j) // просматриваем матрицу смежности уже в поисках предка (слева направо - порядок уже не имеет значения, т.к. предок только один)
                    {
                        if (arrayMatrix[currentVertice - 1, j] == true && visitedVertices[j] == false) // если предок непосещённый
                        {
                            stackPush(stack, j + 1); // добавляем в стек номер вершины
                            visitedVertices[j] = true; // отмечаем эту вершину посещённой
                            added = true;
                            break;
                        }
                        if (arrayMatrix[currentVertice - 1, j] == true && visitedVertices[j] == true) // если предок уже посещённый
                        {
                            currentVertice = j + 1; // ищем дальше
                        }
                    }
            }
        }
    }

    // ОБХОД В ГЛУБИНУ
    Console.WriteLine("Обход графа в глубину с заданной точки:");
    for (int i = 0; i < outputOrder.Count; i++)
    { 
        Console.Write(outputOrder[i]);
        Console.Write(" ");
    }
    return 0;
}

struct stackElement // 
{
    public int vertex;
    public bool isObject;
}
