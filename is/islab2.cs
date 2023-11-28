using System;
using System.Diagnostics.SymbolStore;
using System.Security.Authentication;



int Main()
{
    Console.WriteLine("Введите выражение, не используя символов, отличных от букв латинского/кириллического алфавита.");
    string inputString = Console.ReadLine();
    inputString = inputString.ToUpper(); // в заглавные буквы
    char[] inputArray = inputString.ToCharArray(); // в массив char'ов
    for (int i = 0; i < inputArray.Length; i++) // замена букв "Ё" на "Е" при наличии
    {
        if (inputArray[i] == 'Ё')
            inputArray[i] = 'Е';
    }
    Console.WriteLine("Введите язык ввода");
    Language language = new Language();
    Console.WriteLine("1 - английский, 2 - русский");

    int choice = Convert.ToInt32(Console.ReadLine());     // выбор языка
    switch (choice)
    {
        case 1: language.name = "EN"; language.leftBorder = 65; language.rightBorder = 90;
                language.numOfCharactersInAlphabet = 26; break;
        case 2: language.name = "RU"; language.leftBorder = 1040; language.rightBorder = 1071;
                language.numOfCharactersInAlphabet = 32;  break;
        default: break;
    }

    bool encryption = false; // направление, false - расшифровать, true - зашифровать
    Console.WriteLine("Выберите направление шифрования: 1 - расшифровать, 2 - зашифровать");
    choice = Convert.ToInt32(Console.ReadLine());
    if (choice == 2) encryption = true;

    Console.WriteLine("Выберите способ шифрования: 1 - ключ Цезаря, 2 - ключ Виженера");
    choice = Convert.ToInt32(Console.ReadLine());

    if (choice == 1) // ключ Цезаря
    { 
    Console.Write("Введите ключ шифрования/дешифрования: ");
    int key = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("Ключ: " + key + " , шифровать: " + encryption + ", язык: " + language.name);
    Сipher(inputArray, key, encryption, language);
    }
    if (choice == 2) // ключ Виженера
    {
        Console.WriteLine("Введите ключ шифрования/дешифрования: ");
        string key = Convert.ToString(Console.ReadLine());
        key = key.ToUpper();

        // заполнение и вывод матрицы шифрования
        Console.WriteLine("Матрица шифрования Виженера для выбранного языка будет выглядеть следующим образом:");
        Console.Write("    ");
        for (int i = 0; i < language.numOfCharactersInAlphabet; i++)
        {
            Console.Write(Convert.ToChar(language.leftBorder + i) + " ");
        }
        Console.WriteLine("\n");

        char[,] arrayVigenere= new char[language.numOfCharactersInAlphabet, language.numOfCharactersInAlphabet];
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

        int keyPosition = 0; // индекс элемента ключа
        // шифрование
        if (encryption == true)
        { 

            Console.Write("Зашифрованное сообщение:");
            for (int i = 0; i < inputArray.Length; i++)
            {
                Console.Write(arrayVigenere[Convert.ToInt32(inputArray[i]) - language.leftBorder, 
                                                Convert.ToInt32(key[keyPosition]) - language.leftBorder]);
                if (keyPosition < key.Length - 1)
                    keyPosition++;
                else keyPosition = 0;
            }
        }
        // дешифрование
        else
        {
            char outputCharacter = new char();
            int outputCharacterInt = new int();
            for (int i = 0; i < inputArray.Length; i++)
            {
                outputCharacterInt = Convert.ToInt32(inputArray[i]) - Convert.ToInt32(key[keyPosition]) + language.leftBorder;
                for (; outputCharacterInt < language.leftBorder;)
                {
                    outputCharacterInt += language.numOfCharactersInAlphabet;
                }
                for (; outputCharacterInt > language.rightBorder;)
                {
                    outputCharacterInt -= language.numOfCharactersInAlphabet;
                }
                Console.Write(Convert.ToChar(outputCharacterInt));
                keyPosition++;
                if (keyPosition == key.Length)
                keyPosition = 0;
            }
        }

    }
    Console.WriteLine();
    return 0;
}

Main();
{
    Main();
    Console.WriteLine();
    return 0;
}

void Сipher(char[] inputArray, int key, bool encryption, Language language)
{
    if (!encryption) key *= -1; // если РАСшифровываем - то выполняем операцию, обратную шифрованию

    for (int i = 0; i < inputArray.Length; i++)
    {
        Console.Write(Shift(inputArray[i], key, language));
    }

}

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

struct Language
{
    public string name;
    public int leftBorder;
    public int rightBorder;
    public int numOfCharactersInAlphabet;

    public Language()
    {
        name = "";
        leftBorder = 0;
        rightBorder = 0;
        numOfCharactersInAlphabet = 0;
    }
}