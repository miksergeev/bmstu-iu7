void InfixToPostfix()
{
    Console.WriteLine("Введите выражение в виде инфиксной записи, разделяя его элементы пробелами");
    string inputInfixString = Console.ReadLine();
    int numOfElements = inputInfixString.Count(x => x == ' ') + 1; // число элементов = количество пробелов + 1
    Element[] elements = new Element[numOfElements]; // массив подстрок типа структуры Element
    List<Element> inputInfix = new List<Element>();
    Element zero = new Element();
    zero.value = "0";
    zero.order = 0;
    zero.type = true;
    int numOfOpenBrackets = 0;
    int numOfCloseBrackets = 0;

    for (int i = 0; i < numOfElements; i++) // преобразовываем входную строку, используя структуру Element
    {
        elements[i].value = inputInfixString.Split(' ')[i];
        switch (inputInfixString.Split(' ')[i])
        {
            case "+": elements[i].order = 1; elements[i].type = false; break;
            case "-": elements[i].order = 1; elements[i].type = false; break;
            case "*": elements[i].order = 2; elements[i].type = false; break;
            case "/": elements[i].order = 2; elements[i].type = false; break;
            case ":": elements[i].order = 2; elements[i].type = false; break;
            case "^": elements[i].order = 3; elements[i].type = false; break;
            case "sin": elements[i].order = 4; elements[i].type = false; break;
            case "cos": elements[i].order = 4; elements[i].type = false; break;
            case "abs": elements[i].order = 4; elements[i].type = false; break;
            case "sqrt": elements[i].order = 4; elements[i].type = false; break;
            case "(": elements[i].order = 5; elements[i].type = false; break;
            case ")": elements[i].order = 5; elements[i].type = false; break;
            default: elements[i].order = 0; elements[i].type = true; break;
        }
        // обработка если в начале или после скобки "-"
        if ((i == 0) && (elements[i].value == "-") ||
            (i > 0) && (elements[i-1].value == "(") && (elements[i].value == "-"))
        {
            inputInfix.Add(zero);
        }
        inputInfix.Add(elements[i]);
    }

    // создаём список для выходной строки
    List<Element> outputPostfix = new List<Element>();
    // создаём стек для знаков в виде списка
    List<Element> listForSigns = new List<Element>();
    bool isCorrect = true; // флаг для проверки корректности выражения

    for (int i = 0; i < inputInfix.Count; i++) // 1. курсор на первом элементе входной строки, стек [знаков] пуст, выходная строка пуста
    {
        if (inputInfix[i].type == true) // 2. если число или переменная
        {
            outputPostfix.Add(inputInfix[i]);
        }
        else
        {
            // 3. если рассматриваемый элемент - знак операции, то перебираем несколько правил
            if ((inputInfix[i].order < 5) && (inputInfix[i].type == false))
            {
                // 3.a) если стек пуст или на верхушке лежит открытая скобка
                if ((listForSigns.Count == 0) || (listForSigns[listForSigns.Count - 1].value == "("))
                {
                    listForSigns.Add(inputInfix[i]);
                }
                else
                {
                    // 3.b) если приоритет верхнего знака в стеке меньше приоритета рассматриваемого знака
                    if (listForSigns[listForSigns.Count - 1].order < inputInfix[i].order)
                    {
                        listForSigns.Add(inputInfix[i]);
                    }
                    // 3.c) иначе вытолкнуть верхний знак из стека в выходную строку и повторить шаги 2 и 3, не смещая курсор
                    else
                    {
                        if (inputInfix[i].value != "(" && inputInfix[i].value != ")")
                        {
                            outputPostfix.Add(listForSigns[listForSigns.Count - 1]);
                            listForSigns.RemoveAt(listForSigns.Count - 1);
                            --i; // не смещая курсор
                            continue; // повторяем
                        }
                    }
                }

            }
            //4. если под курсором "(" - добавить в стек
            if (inputInfix[i].value == "(")
            {
                listForSigns.Add(inputInfix[i]);
                ++numOfOpenBrackets;
            }
            // 5. если под курсором ")" - вытолкнуть по одному знаки операций в выходную строку
            if (inputInfix[i].value == ")")
            {
                isCorrect = false;
                for (; ; )
                {
                    // условие выхода из цикла - либо встретилась "(", либо так и не встретилась, т.е. стек опустел
                    if ((listForSigns[listForSigns.Count - 1].value == "(") || (listForSigns.Count == 0))
                        break;
                    outputPostfix.Add(listForSigns[listForSigns.Count - 1]); // выталкиваем знаки из стека
                    listForSigns.RemoveAt(listForSigns.Count - 1);
                }
                if (listForSigns[listForSigns.Count - 1].value == "(")
                {
                    listForSigns.RemoveAt(listForSigns.Count - 1);
                    isCorrect = true;
                }
                ++numOfCloseBrackets;
            }
        }

        if ((isCorrect == false) || (i == (inputInfix.Count - 1) && numOfOpenBrackets != numOfCloseBrackets))
        {
            isCorrect = false;
            Console.WriteLine("Ошибка в записи. Проверьте количество скобок");
            break;
        }
    }
    
    // выталкиваем знаки из стека
    for (; listForSigns.Count != 0;)
    {
        outputPostfix.Add(listForSigns[listForSigns.Count - 1]);
        listForSigns.RemoveAt(listForSigns.Count - 1);
    }

    // выводим значение
    if (isCorrect == true)
    { 
    Console.WriteLine("В обратной польской записи:");
        for (int i = 0; i < outputPostfix.Count; i++)
        {
            Console.Write(outputPostfix[i].value + " ");
        }
    }
    Console.WriteLine();
}

void PostfixToInfix()
{
    Console.WriteLine("Введите выражение в виде постфиксной записи, разделяя его элементы пробелами");
    Console.WriteLine("Для ввода дробного числа отделите целую часть от дробной точкой");
    Console.WriteLine("Для записи переменной используйте букву 'x'");
    string inputPostfixString = Console.ReadLine();
    int numOfElements = inputPostfixString.Count(x => x == ' ') + 1; // число элементов = количество пробелов + 1
    ElementsPostfix[] elements = new ElementsPostfix[numOfElements]; // массив подстрок типа структуры Element
    List<ElementsPostfix> inputPostfix = new List<ElementsPostfix>(); // список для хранения выражения в постфиксной записи
    ElementsPostfix error = new ElementsPostfix();
    int errorNumber = 0; // код ошибки; 0 - ошибки нет

    bool isFunction = false; // флаг для определения: функция с неизвестными или числовое выражение
    double step = 1; // шаг
    double leftBorder = 0; // левая граница
    double rightBorder = 0; // правая граница

    // преобразовываем входную строку, используя структуру Element
    // а также анализируем на наличие переменных (переменой будет считаться символ 'x' во входной строке)
    for (int i = 0; i < numOfElements; i++) 
    {
        elements[i].value = inputPostfixString.Split(' ')[i];
        switch (inputPostfixString.Split(' ')[i])
        {
            case "+": elements[i].order = 1; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 2; break;
            case "-": elements[i].order = 1; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 2; break;
            case "*": elements[i].order = 2; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 2; break;
            case "/": elements[i].order = 2; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 2; break;
            case ":": elements[i].order = 2; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 2; break;
            case "^": elements[i].order = 3; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 2; break;
            case "sin": elements[i].order = 4; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 1; break;
            case "cos": elements[i].order = 4; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 1; break;
            case "abs": elements[i].order = 4; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 1; break;
            case "sqrt": elements[i].order = 4; elements[i].isANumber = false; elements[i].singleOrBinaryOperation = 1; break;
            default: elements[i].order = 0; elements[i].isANumber = true; break;
        }
        inputPostfix.Add(elements[i]);

        if (inputPostfix[i].value == "x") // если один из символов - 'x'
        {
            isFunction = true; // то выражение - функция с неизвестным
        }
    }

    // создаём стек для значений
    List<double> listStack = new List<double>();

    // и стек для выполнения операций
    List<double> listOpStack = new List<double>();

    // и список для хранения значений функции
    List<FunctionTable> listFunctionTable = new List<FunctionTable>();

    // переменная будет использоваться для заполнения списка
    FunctionTable functionTable = new FunctionTable();

    if (isFunction == true) // если выражение записано в виде функции с неизвестным
    {
        Console.WriteLine("Введите значение левой границы вычисляемой функции");
        leftBorder = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Введите значение правой границы вычисляемой функции");
        rightBorder = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Введите значение шага");
        step = Convert.ToDouble(Console.ReadLine());
    }

    if (leftBorder > rightBorder)
    {
        Console.WriteLine("Значение левой границы не может быть больше значения правой");
        return;
    }

    // надо обернуть во внешний цикл для расчёта функции по всем x
    // если это не функция - просто поставить break в конце внешнего цикла, чтобы внешний цикл выполнился всего один раз

    for (double f = leftBorder; f <= rightBorder;)
    {
        for (int i = 0; i < numOfElements; i++) // обработка (основной цикл)
        {

        if (inputPostfix[i].isANumber == true) // 1. если число или переменная, кладём в стек значение
        {
                if (inputPostfix[i].value == "x")             // если x - кладём текущее значение x,
                { 
                    listStack.Add(f);
                }
                else                                          // если просто число - добавляем просто число
                { 
                    listStack.Add(Convert.ToDouble(inputPostfix[i].value));
                }
        }
        else // иначе (т.е. если это операция) достаём из стека аргументы этой операции
        {
            for (int j = 0; j < inputPostfix[i].singleOrBinaryOperation; j++) // по количеству операндов
            {
                if (listStack.Count == 0) // если нужное число операндов извлечь не удалось, т.е. стек опустел
                {
                    errorNumber = 1; // код ошибки 1 : недостаточно операндов
                    break;
                }
                listOpStack.Add(listStack[listStack.Count - 1]);
                listStack.RemoveAt(listStack.Count - 1);
            }
            if (errorNumber != 0) // если не получилось достать нужное число аргументов для операции
            {
                break; // прерываем цикл
            }
            // здесь проверить область определения функции:
            // нельзя делить на ноль:
            if ((inputPostfix[i].value == "/" || inputPostfix[i].value == ":") && (listOpStack[listOpStack.Count-1] == 0))
            {
                if (isFunction == true) // если функция - пишем в графу значений знак вопроса
                {
                        functionTable.valuey = "?";
                }
                    errorNumber = 2; // код ошибки 2 : деление на ноль
                    break;
                }
            // нельзя извлекать корень из отрицательного числа
            if ((inputPostfix[i].value == "sqrt" || inputPostfix[i].value == ":") && (listOpStack[listOpStack.Count - 1] == 0))
            {
                    if (isFunction == true) // если функция - пишем в графу значений знак вопроса
                    {
                        functionTable.valuey = "?";
                    }
                    errorNumber = 3; // код ошибки 3: корень из отрицательного числа
                    break;
                }
            // нельзя возводить отрицательное число в дробную степень (получается корень из отрицательного числа)
            if (inputPostfix[i].value == "^" && listOpStack[listOpStack.Count - 2] < 0 && 
                Convert.ToInt32(listOpStack[listOpStack.Count - 1]) - listOpStack[listOpStack.Count - 1] !=0 )
            {
                    if (isFunction == true) // если функция - пишем в графу значений знак вопроса
                    {
                        functionTable.valuey = "?";
                    }
                    errorNumber = 4; // код ошибки 4: возведение отрицательного числа в дробную степень
                    break;
                }
            // нельзя возводить 0 в отрицательную степень
            if (inputPostfix[i].value == "^" && listOpStack[listOpStack.Count - 2] == 0 &&
               Convert.ToInt32(listOpStack[listOpStack.Count - 1]) < 0)
            {
                    if (isFunction == true) // если функция - пишем в графу значений знак вопроса
                    {
                        functionTable.valuey = "?";
                    }
                    errorNumber = 5;
                    break;
                }

            switch (inputPostfix[i].value)
            {
                case "+":
                    listStack.Add(listOpStack[listOpStack.Count - 1] + listOpStack[listOpStack.Count - 2]); // в стек значений
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций второе число
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций первое число
                    break;

                case "-":
                    listStack.Add(listOpStack[listOpStack.Count - 1] - listOpStack[listOpStack.Count - 2]); // в стек значений
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций второе число
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций первое число
                    break;

                case "*":
                    listStack.Add(listOpStack[listOpStack.Count - 1] * listOpStack[listOpStack.Count - 2]); // в стек значений
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций второе число
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций первое число
                    break;

                case "/":
                    listStack.Add(listOpStack[listOpStack.Count - 1] / listOpStack[listOpStack.Count - 2]); // в стек значений
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций второе число
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций первое число
                    break;

                case ":":
                    listStack.Add(listOpStack[listOpStack.Count - 1] / listOpStack[listOpStack.Count - 2]); // в стек значений
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций второе число
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций первое число
                    break;

                case "^":
                    listStack.Add(Math.Pow(listOpStack[listOpStack.Count - 1], listOpStack[listOpStack.Count - 2]));
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций второе число
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций первое число
                    break;

                case "sin":
                    listStack.Add(Math.Sin(listOpStack[listOpStack.Count - 1]));
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций  число
                    break;

                case "cos":
                    listStack.Add(Math.Cos(listOpStack[listOpStack.Count - 1]));
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций  число
                    break;

                case "abs":
                    listStack.Add(Math.Abs(listOpStack[listOpStack.Count - 1]));
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций  число
                    break;

                case "sqrt":
                    listStack.Add(Math.Sqrt(listOpStack[listOpStack.Count - 1]));
                    listOpStack.RemoveAt(listOpStack.Count - 1); // удаляем из стека операций  число
                    break;
            }
        }
        }
        if (listStack.Count != 1)
        {
            errorNumber = 6; // код ошибки 6 : слишком много операндов (остались лишние в стеке)
        }
        if (isFunction == false) // если не функция - прервать внешний цикл (нам нужно вычислить значение лишь один раз
        {
            break;
        }
        else // иначе - добавить в таблицу значений то, что получилось
        {
            functionTable.valuex = f;
            functionTable.valuey = Convert.ToString(listStack[listStack.Count - 1]);
            listFunctionTable.Add(functionTable);

            listStack.RemoveAt(listStack.Count - 1);

            f += step; // увеличить значение x на шаг
            if (f >= rightBorder && f < rightBorder + step) // если вышли за правую границу не более, чем на расстояние одного шага
            {
                f = rightBorder; // 
                continue; // продолжить
            }
            if (f >= rightBorder + step) // если вышли за правую границу не более, чем на расстояние одного шага
            {
                break; // прервать внешний цикл
            }
        }
    }

    if (errorNumber != 0)
    {
        Console.WriteLine("Ошибка " + errorNumber);
        switch (errorNumber)
        {
            case 1: Console.WriteLine("Недостаточно операндов для выполнения одной из операций. Проверьте выражение"); break;
            case 2: Console.WriteLine("Деление на 0"); break;
            case 3: Console.WriteLine("Извлечение корня из отрицательного числа"); break;
            case 4: Console.WriteLine("Возведение отрицательного числа в дробную степень"); break;
            case 5: Console.WriteLine("Возведение нуля в дробную степень"); break;
            case 6: Console.WriteLine("Введены лишние операнды. Проверьте выражение"); break;
        }
    }

    if (errorNumber == 0)
    {
        if (isFunction == false)
        {
            Console.WriteLine("Значение введённого выражения: " + listStack[0]);
        }
        else
        {
            Console.WriteLine("Таблица для данной функции:");
            for (int i = 0; i < listFunctionTable.Count; i++)
                Console.Write(listFunctionTable[i].valuex + "   " + listFunctionTable[i].valuey + '\n');
        }
    }

}

Main();

int Main() 
{
    Console.WriteLine("Данная программа позволяет выполнять перевод выражения из инфиксной записи в постфиксную (ОПЗ)");
    for(; ; )
    { 
    Console.WriteLine("Выберите режим работы программы:");
    Console.WriteLine("1 - перевести из инфиксной записи в постфиксную");
    Console.WriteLine("2 - перевести из постфиксной записи в инфиксную");
    Console.WriteLine("3 - выйти из программы");

    int choice = 0;
    choice = Convert.ToInt32(Console.ReadLine());
    switch (choice)
    {
        case 1: InfixToPostfix(); break;
        case 2: PostfixToInfix(); break;
        case 3: return 0;
    }
    }
}

struct Element
{
    public string value; // значение
    public int order; // приоритет
    public bool type; // false - знак, true - число
}

struct ElementsPostfix
{
    public string value; // значение
    public int order; // приоритет
    public bool isANumber; // false - знак, true - число
    public int singleOrBinaryOperation; // 1 - унарная, 2 - бинарная
    //public double valueOfNumber;
}

struct FunctionTable
{
    public double valuex; // значения по оси x
    public string valuey; // значени по оси y. Тип string - для возможности вывести знак вопроса, если функция не определена в точке
}
