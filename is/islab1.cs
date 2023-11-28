using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

static Euclidian ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b) // расширенный алгоритм Евклида
{
    Euclidian euclidianReturn = new Euclidian();
    BigInteger d, x, y, x1, y1, x2, y2, q, r;
    if (b == 0) // 1.
    {
        euclidianReturn.d = a;
        euclidianReturn.x = 1;
        euclidianReturn.y = 0;
        return euclidianReturn;
    }
    else // 2.
    { 
        x2 = 1;
        x1 = 0;
        y2 = 0;
        y1 = 1;
        while (b > 0) // 3.
        {
            q = a / b; // 3.1.
            r = a - q * b;
            x = x2 - q * x1;
            y = y2 - q * y1;
            a = b; // 3.2.
            b = r;
            x2 = x1;
            x1 = x;
            y2 = y1;
            y1 = y;
        }
        euclidianReturn.d = a;
        euclidianReturn.x = x2;
        euclidianReturn.y = y2;
        return euclidianReturn;
    }
}


//генерация случайного большого числа
static BigInteger randomGenerator()
{
    RandomNumberGenerator random = RandomNumberGenerator.Create();
    byte[] randomNumber = new byte[128]; // 128 байт = 1024 бита
    random.GetBytes(randomNumber);
    BigInteger result = new BigInteger(randomNumber);
    result = BigInteger.Abs(result);
    return result;
}

// генерация большого простого числа
static BigInteger randomPrimeNumberGenerator()
{
    BigInteger n;
    do
    {
        n = randomGenerator();
    }
    while (MillerRabinTest(n, 100) == false);
    return n;
}

// тест Миллера-Рабина
static bool MillerRabinTest(BigInteger n, int k)
{
    if (n <= 1)
        return false;
    if (n == 2)
        return true;
    if (n % 2 == 0)
        return false;
    int s = 0;
    BigInteger d = n - 1;
    while (d % 2 == 0)
    {
        d /= 2;
        s++;
    }

    for (int i = 0; i < k; i++)
    {
        RandomNumberGenerator random = RandomNumberGenerator.Create();
        byte[] z0 = new byte[n.ToByteArray().LongLength];
        BigInteger a;

        do
        {
            random.GetBytes(z0);
            a = new BigInteger(z0);
        }
        while (a < 2 || a >= n - 2) ;

        BigInteger x = BigInteger.ModPow(a, d, n); // a-z1, d-t, n-number
        if (x == 1 || x == n - 1)
            continue;
        for (int j = 0; j < s - 1; j++)
        {
            x = (x * x) % n;
            if (x == 1)
                return false;
            if (x == n - 1)
                break;
        }
        if (x != n - 1)
            return false;
    }
    return true;
}

// функция Эйлера
static BigInteger Euler(BigInteger p, BigInteger q)
{
    BigInteger euler = (p - 1) * (q - 1);
    return euler;
}

//шифрование сообщения
static BigInteger Encrypt(BigInteger msg, BigInteger e, BigInteger n)
{
    BigInteger c = BigInteger.ModPow(msg, e, n);
    return c;
}

//дешифровка сообщения
static BigInteger Decrypt(BigInteger c, BigInteger d, BigInteger n)
{
    BigInteger m = BigInteger.ModPow(c, d, n);
    return m;
}

BigInteger p = randomPrimeNumberGenerator();
Console.WriteLine("p = " + p + "\n");
BigInteger q = randomPrimeNumberGenerator();
Console.WriteLine("q = " + q + "\n");
BigInteger n = p * q;
Console.WriteLine("n = " + n + "\n");
BigInteger euler = Euler(p, q);
Console.WriteLine("fEuler (n) = " + euler + "\n");
Console.WriteLine("Введите значение открытой экспоненты e такой, что 3 < e < fEuler. Рекомендуется выбрать одно из следующих чисел: 257, 2053, 4099, 8209");
BigInteger e = BigInteger.Parse(Console.ReadLine());
BigInteger temp_d = ExtendedEuclideanAlgorithm(e, euler).x;
BigInteger d = temp_d + euler;
Console.WriteLine("Открытый ключ:\ne = " + e +"\nn = " + n);
Console.WriteLine();
Console.WriteLine("Закрытый ключ:\nd = " + d + "\nn = " + n);

//

BigInteger encryptedMessage;
BigInteger decryptedMessage;

Console.Write("\n\nВведите собщение (число) для шифрования: ");
BigInteger message = BigInteger.Parse(Console.ReadLine());

encryptedMessage = Encrypt(message, e, n);
Console.Write("\nЗашифрованный текст: {0}", encryptedMessage);

Console.WriteLine("Введите ключ d для расшифровки:\n");
BigInteger key = BigInteger.Parse(Console.ReadLine());

decryptedMessage = Decrypt(encryptedMessage, key, n);
Console.Write("\n\nРасшифрованный текст: {0}", decryptedMessage);


Console.ReadLine();

struct Euclidian
{
    public BigInteger d;
    public BigInteger x;
    public BigInteger y;
}