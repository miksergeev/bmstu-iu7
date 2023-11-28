using System.Collections;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Лабораторная работа #3: реализация стеганографии");
Main();

int Main()
{
    for (; ; )
    {
        Console.WriteLine();
        Console.WriteLine("Выберите режим работы программы:");
        Console.WriteLine("1 - зашифровать текстовое сообщение в изображении");
        Console.WriteLine("2 - расшифровать текстовое сообщение из изображения");
        Console.WriteLine("3 - выйти из программы");

        int choice = 0;
        choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1: EncryptMessageToBMP();  break;
            case 2: DecryptMessageFromBMP();  break;
            case 3: return 0;
        }
    }
}

void EncryptMessageToBMP()
{
    Console.WriteLine("Введите путь к файлу, указав его расширение .bmp. Все символы '\\' должны вводиться два раза. Например: D:\\\\IS\\\\test.bmp");
    string sourceToFile = Console.ReadLine();

    //! первые 54 байта - заголовок, не трогать
    byte[] byteArray = initFile(sourceToFile);

    Console.WriteLine("Введите сообщение для шифровки, используя только латинские буквы или следующие символы: " +
                  "пробел, точка, запятая, вопросительный знак, восклицательный знак или тире");
    string message = Console.ReadLine().ToUpper();
    int messageLength = message.Length;
    int[] inputArray = new int[message.Length];

    for (int i = 0; i < inputArray.Length; i++)
    {
        switch (message[i])
        {
            case (' '): inputArray[i] = 26; break;
            case ('.'): inputArray[i] = 27; break;
            case (','): inputArray[i] = 28; break;
            case ('?'): inputArray[i] = 29; break;
            case ('!'): inputArray[i] = 30; break;
            case ('-'): inputArray[i] = 31; break;
            default: inputArray[i] = message[i] - 65; break;
        }

    }

    Console.WriteLine("Длина сообщения: " + messageLength);
    // дальше будут использоваться битовые маски 111111
    // первая цифра - флаг, закодирован или нет символ в пикселе
    // вторая-шестая - биты
    // берётся три значения байта и по два младших бита в каждом и будут содержать скрытый символ

    // сначала - зачистить все пиксели
    int pixelPosition = 54; // начальная позиция пикселя - 54
    int charPosition = 0; // индекс для строки
    for (int i = 0; i < byteArray[22]; i++) // размер по вертикали
    {
        for (int j = 0; j < byteArray[18]; j++) // размер по горизонтали
        {
            for (int k = 0; k < 3; k++)
            {
                uint pixel = (uint)byteArray[pixelPosition];
                pixel >>= 2;
                pixel <<= 2;
                byteArray[pixelPosition] = (byte)pixel;
                pixelPosition++;
            }
        }
        pixelPosition += byteArray[22];
    }

    // функция шифровки пикселя
    void encryptByte(int pixelPosition, int character)
    {
        string binaryChar = Convert.ToString(character, 2).PadLeft(5, '0'); // представляем символ в двоичном коде
        binaryChar = "1" + binaryChar; // в начале - флаг шифрования

        string binaryBlue = binaryChar.Substring(0, 2); // Blue
        int binaryBlueByte = Convert.ToInt32(binaryBlue, 2);
        //Console.WriteLine("binaryBlueByte = " + binaryBlueByte);
        //Console.WriteLine("byteArray[pixelPosition] до = " + byteArray[pixelPosition]);
        byteArray[pixelPosition] += (byte)binaryBlueByte;
        //Console.WriteLine("byteArray[pixelPosition] после = " + byteArray[pixelPosition]);
        pixelPosition++;

        string binaryGreen = binaryChar.Substring(2, 2); // Green
        int binaryGreenByte = Convert.ToInt32(binaryGreen, 2);
        //Console.WriteLine("binaryGreenByte = " + binaryGreenByte);
        //Console.WriteLine("byteArray[pixelPosition] до = " + byteArray[pixelPosition]);
        byteArray[pixelPosition] += (byte)binaryGreenByte;
        //Console.WriteLine("byteArray[pixelPosition] после = " + byteArray[pixelPosition]);
        pixelPosition++;

        string binaryRed = binaryChar.Substring(4, 2); // Red
        int binaryRedByte = Convert.ToInt32(binaryRed, 2);
        //Console.WriteLine("binaryRedByte = " + binaryRedByte);
        //Console.WriteLine("byteArray[pixelPosition] до = " + byteArray[pixelPosition]);
        byteArray[pixelPosition] += (byte)binaryRedByte;
        //Console.WriteLine("byteArray[pixelPosition] после = " + byteArray[pixelPosition]);
        pixelPosition++;
    }

    // шифровка
    pixelPosition = 54;
    charPosition = 0;
    for (int i = 0; (i < byteArray[22]) && (charPosition < message.Length); i++) // размер по вертикали
    {
        for (int j = 0; (j < byteArray[18]) && (charPosition < message.Length); j++) // размер по горизонтали
        {
            //Console.WriteLine("pixelPosition = " + pixelPosition);
            //Console.WriteLine("charPosition = " + charPosition);
            // Console.WriteLine("До шифрования byteArray[pixelPosition] = " + byteArray[pixelPosition]);
            encryptByte(pixelPosition, inputArray[charPosition]);
            //Console.WriteLine("После шифрования byteArray[pixelPosition] = " + byteArray[pixelPosition]);
            //Console.WriteLine();
            pixelPosition += 3;
            charPosition++;
        }
        pixelPosition += byteArray[22];
    }

    //Bitmap bitmap2 = new Bitmap(byteArray[18], byteArray[22]);

    using (MemoryStream ms = new MemoryStream(byteArray))
    {
        using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms, false, false))
        {
            img.Save(sourceToFile.Insert(sourceToFile.Length - 4, "_encrypted"));
        }
    }
}


void DecryptMessageFromBMP()
{
    Console.WriteLine("Введите путь к файлу, указав его расширение .bmp. Все символы '\\' должны вводиться два раза. Например: D:\\\\IS\\\\test.bmp");
    string sourceToFile = Console.ReadLine();

    byte[] byteArray = initFile(sourceToFile);

    Console.WriteLine("Расшифрованное сообщение:");
    int pixelPosition = 54;
    for (int i = 0; (i < byteArray[22]) && (decryptionIsNecessary(pixelPosition, byteArray)); i++) // размер по вертикали
    {
        for (int j = 0; (j < byteArray[18]) && (decryptionIsNecessary(pixelPosition, byteArray)); j++) // размер по горизонтали
        {
            Console.Write(decryptByte(pixelPosition, byteArray));
            pixelPosition += 3;
        }
        pixelPosition += byteArray[22];
    }
}

// функция дешифровки
char decryptByte(int pixelPosition, byte[] byteArray)
{
    string character = "";
    for (int i = 0; i < 3; i++)
    {
        uint pixel = (uint)byteArray[pixelPosition];
        pixel <<= 30; // uint занимает 32 бита, затираем первые 30
        pixel >>= 30;
        character += Convert.ToString((byte)pixel, 2).PadLeft(2, '0');
        pixelPosition++;
    }
    character = character.Remove(0, 1);

    int decryptedByte = Convert.ToInt32(character, 2);

    char charReturn;
    switch (decryptedByte)
    {
        case (26): charReturn = ' '; break;
        case (27): charReturn = '.'; break;
        case (28): charReturn = ','; break;
        case (29): charReturn = '?'; break;
        case (30): charReturn = '!'; break;
        case (31): charReturn = '-'; break;
        default: charReturn = Convert.ToChar(decryptedByte + 65); break;
    }
    return charReturn;
}

// функция необходимости дешифровки
bool decryptionIsNecessary(int pixelPosition, byte[] byteArray)
{
    string testCharacter = "";

    uint pixel = (uint)byteArray[pixelPosition];
    pixel <<= 30; // uint занимает 32 бита, затираем первые 30
    pixel >>= 30;
    pixel >>= 1;
    testCharacter += Convert.ToString(pixel, 2);
    if (testCharacter == "1")
    {
        return true;
    }
    else
    {
        return false;
    }
}


byte[] initFile(string fileSource)
{
    Bitmap bitmap = new Bitmap(fileSource);
    static byte[] ImageToByte(Bitmap img)
    {
        ImageConverter converter = new ImageConverter();
        return (byte[])converter.ConvertTo(img, typeof(byte[]));
    }
    //! первые 54 байта - заголовок, не трогать
    byte[] byteArray = ImageToByte(bitmap);
    return byteArray;
}