using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System.Text;

Console.WriteLine("Лабораторная работа #4: шифрование сообщение с помощью кода Хаффмана");
Main();

int Main()
{
    for (; ; )
    {
        Console.WriteLine();
        Console.WriteLine("Выберите режим работы программы:");
        Console.WriteLine("1 - реализовать алгоритм Хаффмана");
        Console.WriteLine("2 - выйти из программы");

        int choice = 0;
        choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1: HuffmanCode(); break;
            case 2: return 0;
        }
    }
}

    static void HuffmanCode()
        {
            Console.WriteLine("Введите сообщение для шифрования:");
            string input = Console.ReadLine(); // входящая строка
            Tree tree = new(); // создание дерева (экземпляр класса Tree, описанного ниже)
            tree.Build(input); // построение дерева

            SortArray(input); // сортировка по числу повторений

            BitArray encodedMessage = tree.Encode(input); // шифрование строки в битовый массив
            Console.WriteLine("Зашифрованная строка: ");
            foreach (bool bit in encodedMessage)
            {
                Console.Write((bit ? 1 : 0) + ""); // если true - выводим 1, если false - 0
            }
            Console.WriteLine();

            //string decodedMessage = ; // расшифрованное сообщение
            Console.WriteLine("Расшифрованная строка: " + tree.Decode(encodedMessage)); // расшифровка зашифрованного сообщение
            Console.WriteLine();
            
            for (; input != "exit";) // расшифровка пользовательского сообщения
            {
            Console.WriteLine("Используя таблицу с кодами символов, введите Ваше сообщение из числа этих символов"); // сообщение пользователя
            Console.WriteLine("Для ввода используйте символы '0' и '1', для выхода введите 'exit':");
            input = Console.ReadLine(); // входящая строка

            BitArray inputBitArray = new BitArray(input.Select(c => c == '1').ToArray()); // конвертация в битовый массив
            Console.WriteLine("Расшифрованная строка: " + tree.Decode(inputBitArray)); // расшифровка битового массива
            }


        static void SortArray(string input) // функция сортировки символов в строке в зависимости от количества повторений
        {
            string shortMessage = new string(input.Distinct().ToArray()); // ToArray переводит строку в массив,
                                                                          // Distinct удаляет повторяющиеся значения из массива,
                                                                          // shortMessage - введённая строка без повторений символов
            int[] frequency = new int[input.Length]; // массив с указанием количества повторений

            for (int i = 0; i < shortMessage.Length; i++) //
            {
                frequency[i] = 0; // начальное количество повторений
                for (int j = 0; j < input.Length; j++) // подсчёт количества повторений символов
                {
                    if (shortMessage[i] == input[j]) // если символ строки без повторений совпадает с символом введённой строки
                    {
                        frequency[i] += 1; // увеличиваем счётчик с таким индексом
                    }
                }
            }

            // сортировка
            int swap;
            char[] sortedShortMessage = shortMessage.ToCharArray(); ; // массив для хранения отсортированных элементов (символов)
            for (int i = 0; i < shortMessage.Length - 1; i++) // сортировка
            {
                for (int j = 0; j < shortMessage.Length - 1; j++) // пузырьком
                {
                    if (frequency[j] < frequency[j + 1])
                    {
                        swap = frequency[j]; // перестановка элементов в массиве frequency (частота использования символа)
                        frequency[j] = frequency[j + 1];
                        frequency[j + 1] = swap;

                        swap = sortedShortMessage[j]; // перестановка используемых символов в порядке частоты использования
                        sortedShortMessage[j] = sortedShortMessage[j + 1];
                        sortedShortMessage[j + 1] = Convert.ToChar(swap);
                    }
                }
            }

            Console.WriteLine("Отсортированый по частоте повторений символов массив: ");
            for (int i = 0; i < input.Length; i++)
            {
                if (frequency[i] != 0)
                    Console.Write(shortMessage[i]);
            }
            Console.WriteLine();
            for (int i = 0; i < input.Length; i++)
            {
                if (frequency[i] != 0)
                    Console.Write(frequency[i]);
            }
            Console.WriteLine();
        }
    } // основной алгоритм

    public class Tree // дерево
    {
        private readonly List<Node> _nodes = new(); // вершины
        public Node Root { get; set; }
        public Dictionary<char, int> Frequency = new(); // словарь, где каждому символу сопоставлена частота его использования в сообщении

        public void Build(string message)
        {
            for (int i = 0; i < message.Length; i++) // добавление элементов в словарь по частоте использования
            {
                if (!Frequency.ContainsKey(message[i]))
                {
                    Frequency.Add(message[i], 0);
                }
                Frequency[message[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in Frequency) // добавление элементов в узел из словаря
            {
                _nodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            while (_nodes.Count > 1)
            {
                List<Node> orderedNodes = _nodes.OrderBy(node => node.Frequency).ToList();
                if (orderedNodes.Count >= 2)
                {
                    List<Node> taken = orderedNodes.Take(2).ToList();
                    Node parent = new()
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };
                    _nodes.Remove(taken[0]);
                    _nodes.Remove(taken[1]);
                    _nodes.Add(parent);
                }
                this.Root = _nodes.FirstOrDefault();
            }

        }

        public BitArray Encode(string source) // кодирование сообщения в биты
        {
            List<bool> encodedSource = new(); // список булевых значений

            Console.WriteLine("Таблица получившихся кодов символов:");
            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.TreeSearch(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
                Console.WriteLine("Символ " + source[i] + " будет закодирован значением " + new string(encodedSymbol.Select(c => c ? '1' : '0').ToArray()));
            }

            BitArray bits = new(encodedSource.ToArray()); // возврат закодированного сообщения в виде массива битов
            return bits;
        }

        public string Decode(BitArray bits) // декодирования сообщения из битов
        {
            Node current = this.Root;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded += current.Symbol;
                    current = this.Root;
                }
            }
            return decoded;
        }

        public static bool IsLeaf(Node node) // метод, позволяющий определить, что узел конечный (нет потомков)
        {
            return (node.Left == null && node.Right == null);
        }
    }

    public class Node // вершина, имеет следующие поля:
    {
        public Node Right { get; set; } // левый потомок
        public Node Left { get; set; } // правый потомок
        public char Symbol { get; set; } // символ
        public int Frequency { get; set; } // частота использования символа

    public List<bool> TreeSearch(char symbol, List<bool> data) // обход дерева в глубину
        {
            if (Right == null && Left == null)
            {
                if (symbol.Equals(this.Symbol))
                {
                    return data;
                }
                return null;
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new();
                    leftPath.AddRange(data);
                    leftPath.Add(false);
                    left = Left.TreeSearch(symbol, leftPath);
                }

                if (Right != null)
                {
                    List<bool> rightPath = new();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.TreeSearch(symbol, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                return right;
            }
        }
    }
