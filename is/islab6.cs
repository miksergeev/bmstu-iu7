using System.Numerics;
using System.Security.Cryptography;

Console.WriteLine("Лабораторная работа #6: использование при шифровании протокола Диффи — Хеллмана");
Main();

int Main()
{
    Random rand = new();

    // генерация p и g и запись их в файлы
    // p
    BigInteger p = new(rand.Next(100000, 1000000));
    string pPath = "C://BMSTU//p.txt";
    using (FileStream fileStream = File.Open(pPath, FileMode.Create))
    {
        using (StreamWriter output = new StreamWriter(fileStream))
        {
            output.Write(p);
        }
    }
    //g
    BigInteger g;
    if (IsPrime((p - 1) / 2) == true)
    {
        g = FindPrimitiveRoot(p);
    }
    else
    {
        p = GetNearestPrime(p);
        g = FindPrimitiveRoot(p);
    }
    string gPath = "C://BMSTU//g.txt";
    using (FileStream fileStream = File.Open(gPath, FileMode.Create))
    {
        using (StreamWriter output = new StreamWriter(fileStream))
        {
            output.Write(p);
        }
    }

    // генерация a и вывод на экран
    BigInteger a = new(rand.Next(0, 1000000));
    Console.WriteLine("Программа сгенерировала число a = " + a + ". Это секретный ключ Алисы, он не записывается в файл");

    // генерация b и вывод на экран
    BigInteger b = new(rand.Next(0, 1000000));
    Console.WriteLine("Программа сгенерировала число b = " + b + ". Это секретный ключ Боба, он не записывается в файл");

    // вывод p и g
    Console.WriteLine("Также сгенерированы числа p = " + p + " и g = " + g);
    Console.WriteLine("p записан в файл " + pPath + ", g записан в файл " + gPath);

    // вычисляем открытый ключ Алисы A
    BigInteger openKeyAliceA = BigInteger.ModPow(g, a, p);
    string openKeyAliceAPath = "C://BMSTU//openKeyAliceA.txt";
    using (FileStream fileStream = File.Open(openKeyAliceAPath, FileMode.Create))
    {
        using (StreamWriter output = new StreamWriter(fileStream))
        {
            output.Write(openKeyAliceA);
        }
    }
    Console.WriteLine("Программа вычислила открытый ключ Алисы " + openKeyAliceA + " и записала его в файл " + openKeyAliceAPath);

    // вычисляем открытый ключ Боба B
    BigInteger openKeyBobB = BigInteger.ModPow(g, b, p);
    string openKeyBobBPath = "C://BMSTU//openKeyBobB.txt";
    using (FileStream fileStream = File.Open(openKeyBobBPath, FileMode.Create))
    {
        using (StreamWriter output = new StreamWriter(fileStream))
        {
            output.Write(openKeyBobB);
        }
    }
    Console.WriteLine("Программа вычислила открытый ключ Боба " + openKeyBobB + " и записала его в файл " + openKeyBobBPath);

    // генерация секретного ключа со стороны Алисы
    BigInteger openBFromFile = 0; // открытый ключ Боба B считывается из файла, а не из ранее полученной переменной
    using (StreamReader sr = new StreamReader(openKeyBobBPath))
    {
        string openBNumber = sr.ReadLine();
        openBFromFile = BigInteger.Parse(openBNumber);
    }
    BigInteger secretKeyAlice = BigInteger.ModPow(openBFromFile, a, p);
    Console.WriteLine("Алиса сгенерировала секретный ключ " + secretKeyAlice + ". Он не записывается в файл");

    // генерация секретного ключа со стороны Боба
    BigInteger openAFromFile = 0; // открытый ключ Алисы А считывается из файла, а не из ранее полученной переменной
    using (StreamReader sr = new StreamReader(openKeyAliceAPath))
    {
        string openANumber = sr.ReadLine();
        openAFromFile = BigInteger.Parse(openANumber);
    }
    BigInteger secretKeyBob = BigInteger.ModPow(openAFromFile, b, p);
    Console.WriteLine("Боб сгенерировал секретный ключ " + secretKeyBob + ". Он не записывается в файл");

    // шифрование
    Console.WriteLine("Алиса вводит сообщение на латинице без знаков препинания и пробелов: ");
    string message = Console.ReadLine();
    Console.WriteLine("С помощью полученного секретного ключа Алиса шифрует данное сообщение");

    Language language = new Language();
    language.name = "EN"; language.leftBorder = 65; language.rightBorder = 90;
    language.numOfCharactersInAlphabet = 26;
    message = message.ToUpper(); // в заглавные буквы
    char[] inputArray = message.ToCharArray(); // в массив char'ов

    int KAliceInt = (int)secretKeyAlice; // преобразование ключа Алисы в "слово"
    char[] keyChar = KAliceInt.ToString().ToCharArray();
    for (int i = 0; i < keyChar.Length; i++)
    {
        keyChar[i] = Convert.ToChar(keyChar[i] + 17);
    }
    string keyAliceString = new string(keyChar);

    Console.WriteLine("Сообщение Алисы '" + message + "' будет зашифровано шрифтом Виженера с помощью полученного ключа " + keyAliceString);

    // заполнение и вывод матрицы шифрования
    Console.WriteLine("Матрица шифрования Виженера для выбранного языка будет выглядеть следующим образом:");

    Console.Write("    ");
    for (int i = 0; i < language.numOfCharactersInAlphabet; i++)
    {
        Console.Write(Convert.ToChar(language.leftBorder + i) + " ");
    }
    Console.WriteLine("\n");

    char[,] arrayVigenere = new char[language.numOfCharactersInAlphabet, language.numOfCharactersInAlphabet];
    for (int i = 0; i < language.numOfCharactersInAlphabet; i++)
    {
        Console.Write(Convert.ToChar(language.leftBorder + i) + "   ");

        for (int j = 0; j < language.numOfCharactersInAlphabet; j++)
        {
            arrayVigenere[i, j] = Shift(Convert.ToChar(language.leftBorder + i), j, language);
            Console.Write(arrayVigenere[i, j] + " ");
        }
        Console.WriteLine();
    }
    int keyPositionAlice = 0; // индекс элемента ключа

    // шифрование
    char[] encryptedMessage = message.ToCharArray();

    for (int i = 0; i < inputArray.Length; i++)
        {
        encryptedMessage[i] = arrayVigenere[Convert.ToInt32(inputArray[i]) - language.leftBorder,
                                        Convert.ToInt32(keyAliceString[keyPositionAlice]) - language.leftBorder];

        if (keyPositionAlice < keyAliceString.Length - 1)
            keyPositionAlice++;
        else keyPositionAlice = 0;

        }


    string encryptedMessageString = new string(encryptedMessage);

    string encryptedMessagePath = "C://BMSTU//encryptedMessage.txt"; // имя файла
    using (FileStream fileStream = File.Open(encryptedMessagePath, FileMode.Create))
    {
        using (StreamWriter output = new StreamWriter(fileStream))
        {
            output.Write(encryptedMessage);
        }
    }

    Console.WriteLine("Зашифрованное сообщение Алисы " + encryptedMessageString + " записано в текстовый файл " + encryptedMessagePath);

    // считываем зашифрованное сообщение
    string encryptedMessageBobFromFile = string.Empty;
    StreamReader f = new StreamReader(encryptedMessagePath);
    {
    encryptedMessageBobFromFile = f.ReadLine();
    }

    // считываем ключ Боба
    int KBobInt = (int)secretKeyBob; // преобразование ключа Боба в "слово"
    char[] keyCharBob = KBobInt.ToString().ToCharArray();

    for (int i = 0; i < keyCharBob.Length; i++)
    {
    keyCharBob[i] = Convert.ToChar(keyCharBob[i] + 17);
    }
    string keyBobString = new string(keyCharBob);
    Console.WriteLine("Зашифрованное сообщение " + encryptedMessageBobFromFile + " расшифровывает Боб с помощью найденного им шифра " + keyBobString);

    // расшифровка шифром Виженера
    int keyPositionBob = 0; // индекс элемента ключа
    int outputCharacter = new int();
    Console.Write("Полученное Бобом сообщение: ");
    for (int i = 0; i < inputArray.Length; i++)
    {
    outputCharacter = Convert.ToInt32(encryptedMessageBobFromFile[i]) - Convert.ToInt32(keyCharBob[keyPositionBob]) + language.leftBorder;
        for (; outputCharacter < language.leftBorder;)
        {
            outputCharacter += language.numOfCharactersInAlphabet;
        }
        for (; outputCharacter > language.rightBorder;)
        {
            outputCharacter -= language.numOfCharactersInAlphabet;
        }
    Console.Write(Convert.ToChar(outputCharacter));
    keyPositionBob++;
    if (keyPositionBob == keyCharBob.Length)
        keyPositionBob = 0;
    }

    return 0;
}

// функции
static bool IsPrime(BigInteger number)
{
    if (number <= 1)
    {
        return false;
    }
    if (number <= 3)
    {
        return true;
    }

    if (number % 2 == 0 || number % 3 == 0)
    {
        return false;
    }

    for (int i = 5; i * i <= number; i += 6)
    {
        if (number % i == 0 || number % (i + 2) == 0)
        {
            return false;
        }
    }
    return true;
}

static BigInteger GetNearestPrime(BigInteger number)
{
    while (IsPrime((number - 1) / 2) == false)
    {
        number++;
    }
    return number;
}

static BigInteger Power(BigInteger x, BigInteger y, BigInteger p)
{
    BigInteger result = 1;
    x %= p;

    while (y > 0)
    {
        if (y % 2 == 1)
        {
            result = (result * x) % p;
        }
        y >>= 1;
        x = (x * x) % p;
    }
    return result;
}

static void FindPrimeFactors(HashSet<BigInteger> s, BigInteger n)
{
    while (n % 2 == 0)
    {
        s.Add(2);
        n /= 2;
    }

    for (int i = 3; i <= Math.Pow(Math.E, BigInteger.Log(n) / 2); i += 2)
    {
        while (n % i == 0)
        {
            s.Add(i);
            n /= i;
        }
    }

    if (n > 2)
    {
        s.Add(n);
    }
}

static BigInteger FindPrimitiveRoot(BigInteger number)
{
    HashSet<BigInteger> s = new HashSet<BigInteger>();

    BigInteger phi = number - 1;

    FindPrimeFactors(s, phi);

    for (int r = 2; r <= phi; r++)
    {
        bool flag = false;
        foreach (int a in s)
        {
            if (Power(r, phi / (a), number) == 1)
            {
                flag = true;
                break;
            }
        }

        if (flag == false)
        {
            return r;
        }
    }
    return -1;
}

// функция для циклического сдвига символов
char Shift(char inputCharacter, int key, Language language)
{
    char outputCharacter;
    outputCharacter = Convert.ToChar((inputCharacter - language.leftBorder + key) %
            language.numOfCharactersInAlphabet + language.leftBorder);
    // проверка на выход за пределы алфавита
    for (; Convert.ToInt32(outputCharacter) < language.leftBorder;)
    {
        outputCharacter = Convert.ToChar(outputCharacter + language.numOfCharactersInAlphabet);
    }
    for (; Convert.ToInt32(outputCharacter) > language.rightBorder;)
    {
        outputCharacter = Convert.ToChar(outputCharacter - language.numOfCharactersInAlphabet);
    }
    return outputCharacter;
}

// структура для хранения атрибутов языка
struct Language
{
    public string name; // название
    public int leftBorder; // левая граница симолов Unicode
    public int rightBorder; // правая граница символов Unicode
    public int numOfCharactersInAlphabet; // число символов в алфавите

    public Language() // конструктор
    {
        name = "";
        leftBorder = 0;
        rightBorder = 0;
        numOfCharactersInAlphabet = 0;
    }
}

