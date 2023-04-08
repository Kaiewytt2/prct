using System;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
{
    Console.WriteLine("Выберите метод шифрования: ");
    Console.WriteLine("1 - Зашифровать сообщение первым способом(частотный анализ)");
    Console.WriteLine("2 - Дешифровать сообщение первым способом(частотный анализ)");
    Console.WriteLine("3 - Зашифровать сообщение вторым способом(ключ)");
    Console.WriteLine("4 - Дешифровать сообщение вторым способом(ключ)");
    Console.WriteLine("5 - Зашифровать сообщение третьим способом(ASCII)");
    Console.WriteLine("6 - Дешифровать сообщение третьим способом(ASCII)");
    int choice = int.Parse(Console.ReadLine());

    switch (choice)
    {
        case 1:
        case 2:
            Console.WriteLine("Введите ключ шифра (достаточное кол-во букв для шифрования):");
            string key1 = Console.ReadLine().ToLower();
            Console.WriteLine("Введите сообщение для " + (choice == 1 ? "шифрования" : "дешифрования") + ":");
            string message1 = Console.ReadLine().ToLower();
            string result1 = choice == 1 ? EncryptMessage1(message1, key1) : DecryptMessage1(message1, key1);
            Console.WriteLine("Результат: " + result1);
            break;
        case 3:
        case 4:
            Console.WriteLine("Введите ключ шифра (достаточное кол-во букв для шифрования):");
            string key2 = Console.ReadLine().ToLower();
            Console.WriteLine("Введите сообщение для " + (choice == 3 ? "шифрования" : "дешифрования") + ":");
            string message2 = Console.ReadLine().ToLower();
            string result2 = choice == 3 ? EncryptMessage2(message2, key2) : DecryptMessage2(message2, key2);
            Console.WriteLine("Результат: " + result2);
            break;
        case 5:
        case 6:
            Console.WriteLine("Введите сообщение для " + (choice == 5 ? "шифрования" : "дешифрования") + ":");
            string message3 = Console.ReadLine().ToLower();
            Console.WriteLine("Введите сдвиг (число):");
            int shift = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите дополнительный параметр (число для добавления):");
            int extra = int.Parse(Console.ReadLine());
            string result3 = choice == 5 ? EncryptMessage3(message3, shift, extra) : DecryptMessage3(message3, shift, extra);
            Console.WriteLine("Результат: " + result3);
            break;
        default:
            Console.WriteLine("Ошибка ввода.");
            break;
    }

    Console.ReadLine();
}

        static string EncryptMessage1(string message, string key)
        {
            // определение частоты появления каждого символа в ключе
            int[] freq = new int[26];
            foreach (char c in key)
            {
                if (Char.IsLetter(c))
                {
                    freq[c - 'a']++;
                }
            }

            // создание таблицы замен
            char[] table = new char[26];
            int code = 0;
            for (int i = 0; i < 26; i++)
            {
                int maxIndex = Array.IndexOf(freq, freq.Max());
                freq[maxIndex] = -1;
                table[maxIndex] = (char)('a' + code);
                code++;
            }

            // шифрование сообщения
            string result = "";
            foreach (char c in message)
            {
                if (Char.IsLetter(c))
                {
                    result += table[c - 'a'];
                }
                else
                {
                    result += c;
                }
            }

            return result;
        }

        static string DecryptMessage1(string message, string key)
        {
            // определение частоты появления каждого символа в ключе
            int[] freq = new int[26];
            foreach (char c in key)
            {
                if (Char.IsLetter(c))
                {
                    freq[c - 'a']++;
                }
            }

            // создание таблицы замен
            char[] table = new char[26];
            int code = 0;
            for (int i = 0; i < 26; i++)
            {
                int maxIndex = freq.ToList().IndexOf(freq.Max());
                freq[maxIndex] = -1;
                table[maxIndex] = (char)(code + 'a');
                code++;
            }

            // расшифрование сообщения
            string result = "";
            foreach (char c in message)
            {
                if (Char.IsLetter(c))
                {
                    result += (char)('a' + Array.IndexOf(table, c));
                }
                else
                {
                    result += c;
                }
            }

            return result;
        }

        // Функция шифрования
        static string EncryptMessage2(string message, string key)
        {
            // Убираем все символы кроме букв из ключа
            key = new string(key.Where(Char.IsLetter).ToArray());

            // Создаем словарь для замены букв цифрами
            Dictionary<char, int> table = new Dictionary<char, int>();
            int code = 0;
            foreach (char c in key.ToLower())
            {
                if (!table.ContainsKey(c))
                {
                    table.Add(c, code);
                    code++;
                }
            }

            // Шифрование сообщения
            StringBuilder result = new StringBuilder();
            foreach (char c in message.ToLower())
            {
                if (Char.IsLetter(c))
                {
                    result.Append((table[c] + 1).ToString("D2") + " ");
                }
            }

            return result.ToString().TrimEnd();
        }

        static string DecryptMessage2(string message, string key)
        {
            // Убираем все символы кроме букв из ключа
            key = new string(key.Where(Char.IsLetter).ToArray());

            // Создаем словарь для замены цифр буквами
            Dictionary<int, char> table = new Dictionary<int, char>();
            int code = 0;
            foreach (char c in key.ToLower())
            {
                if (!table.ContainsValue(c))
                {
                    table.Add(code, c);
                    code++;
                }
            }

            // Дешифрование сообщения
            StringBuilder result = new StringBuilder();
            foreach (string s in message.Split())
            {
                int index = Int32.Parse(s) - 1;
                result.Append(table[index]);
            }

            return result.ToString();
        }
        
        static string EncryptMessage3(string message, int shift, int extraCharsCount)
        {
            Random rnd = new Random();
            StringBuilder result = new StringBuilder();

            foreach (char c in message)
            {
                // Сдвигаем код символа на заданную величину
                int newCharValue = c + shift;

                // Если новое значение выходит за пределы кодов таблицы ASCII,
                // то возвращаемся к началу таблицы, чтобы избежать ошибок
                if (newCharValue > 127) newCharValue -= 127;

                // Добавляем случайные символы между символами сообщения
                for (int i = 0; i < extraCharsCount; i++)
                {
                    result.Append((char)rnd.Next(33, 127)); 
                }

                // Добавляем зашифрованный символ
                result.Append((char)newCharValue);
            }

            return result.ToString();
        }

        static string DecryptMessage3(string message, int shift, int extraCharsCount)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;

            while (i < message.Length)
            {
                // Добавляем случайные символы, которые были вставлены между символами сообщения
                for (int j = 0; j < extraCharsCount; j++)
                {
                    i++; // Пропускаем текущий символ, так как он является случайным символом
                }

                // Дешифруем символ
                int newCharValue = message[i] - shift;

                // Если новое значение выходит за пределы кодов таблицы ASCII, то возвращаемся к концу таблицы, чтобы избежать ошибок
                if (newCharValue < 0) newCharValue += 127;

                // Добавляем дешифрованный символ
                result.Append((char)newCharValue);

                i++; // Переходим к следующему символу сообщения
            }

            return result.ToString();
        }
    }
}
