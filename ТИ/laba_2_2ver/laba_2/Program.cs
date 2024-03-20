using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace laba_2
{

    internal class Program
    {
        static char[] alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();

        static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\leone\\Desktop\\Учеба\\ТИ и тервер\\ТИ\\laba_2\\laba_2\\Database1.mdf\";Integrated Security=True";
        public static void Main(string[] args)
        {

            Dictionary<char, double> CryptedTextTable = new Dictionary<char, double>();
            Dictionary<char, double> DeCryptedTextTable = new Dictionary<char, double>();


            string text = Text();
            string DycrypText = text;
            //Console.WriteLine(text);
            //Console.WriteLine("End");
            CryptedTextTable = AppendTable(text);
            text = ReplaceLetters(text);
            DeCryptedTextTable = AppendTable(text);
            //Console.WriteLine("___________________________");
            //Console.WriteLine(text);
            foreach (var pair in CryptedTextTable)
            {
                Console.WriteLine("Ключ: " + pair.Key + ", Значение: " + pair.Value);
            }
            Console.WriteLine("Таблица расшифровки");
            Console.WriteLine();
            foreach (var pair in DeCryptedTextTable)
            {
                Console.WriteLine("Ключ: " + pair.Key + ", Значение: " + pair.Value);
            }
            Console.WriteLine("Соотнесенные буквы");
            Console.WriteLine();
            text = DecryptionTextReplacer(text, CryptedTextTable, DeCryptedTextTable);

            //Console.WriteLine("___________________________");
            //Console.WriteLine(text);
            int matches = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == DycrypText[i])
                {
                    matches++;
                }
            }
            Console.WriteLine($"Процент совпадения: {((float)matches / text.Length) * 100}%");
            Console.ReadKey();
        }
        static string DecryptionTextReplacer(string text, Dictionary<char, double> CryptedTextTable, Dictionary<char, double> DeCryptedTextTable)
        {
            Dictionary<char, char> replacements = DecryptionText(CryptedTextTable, DeCryptedTextTable);

            char[] charArray = text.ToCharArray();
                for (int i = 0; i < charArray.Length; i++)
            {
                if (replacements.ContainsKey(char.ToLower(charArray[i])))
                {
                    charArray[i] = replacements[char.ToLower(charArray[i])];
                }
            }
            foreach (var pair in replacements)
            {
                Console.WriteLine("Ключ: " + pair.Key + ", Значение: " + pair.Value);
            }
            return new string(charArray);
        }
        static string ReplaceLetters(string input)
        {
            // Создаем словарь для хранения замен
            Dictionary<char, char> replacements = GenerateRandomReplacements();

            // Преобразуем строку в массив символов
            char[] charArray = input.ToCharArray();

            // Проходим по каждому символу и заменяем его, если есть соответствующая замена
            for (int i = 0; i < charArray.Length; i++)
            {
                if (replacements.ContainsKey(char.ToLower(charArray[i])))
                {
                    charArray[i] = replacements[char.ToLower(charArray[i])];
                }
            }
            // Преобразуем массив символов обратно в строку
            return new string(charArray);
        }
        static Dictionary<char, char> DecryptionText(Dictionary<char, double> CryptedTextTable, Dictionary<char, double> DeCryptedTextTable)
        {
            Dictionary<char, char> replacements = new Dictionary<char, char>();

            // Создание реверсивных словарей
            Dictionary<double, char> reversedDictionary1 = new Dictionary<double, char>();
            foreach (var kvp in CryptedTextTable)
            {
                if (!reversedDictionary1.ContainsKey(kvp.Value))
                {
                    reversedDictionary1.Add(kvp.Value, kvp.Key);
                }
            }
            Dictionary<double, char> reversedDictionary2 = new Dictionary<double, char>();
            foreach (var kvp in DeCryptedTextTable)
            {
                if (!reversedDictionary2.ContainsKey(kvp.Value))
                {
                    reversedDictionary2.Add(kvp.Value, kvp.Key);
                }
            }


            foreach (var key in reversedDictionary1.Keys)
            {
                if (reversedDictionary2.ContainsKey(key))
                {
                    char decryptedKey = reversedDictionary1[key];
                    char decryptedValue = reversedDictionary2[key];

                    // Проверяем, существует ли ключ уже в replacements
                    if (!replacements.ContainsKey(decryptedKey))
                    {
                        // Добавляем элемент в replacements
                        replacements.Add(decryptedKey, decryptedValue);
                    }
                }
            }
            return replacements;

        }
        static Dictionary<char, char> GenerateRandomReplacements()
        {
            Dictionary<char, char> replacements = new Dictionary<char, char>();
            Random random = new Random();

            // Создаем массив букв русского алфавита для замены
            char[] alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();

            // Перемешиваем буквы русского алфавита
            for (int i = 0; i < alphabet.Length; i++)
            {
                int randomIndex = random.Next(i, alphabet.Length);
                char temp = alphabet[randomIndex];
                alphabet[randomIndex] = alphabet[i];
                alphabet[i] = temp;
            }

            // Заполняем словарь замен случайными соответствиями
            for (int i = 0; i < alphabet.Length; i++)
            {
                replacements.Add((char)('а' + i), alphabet[i]);
            }

            return replacements;
        }
        public static string Text()
        {
            string path = "C:\\Users\\leone\\Desktop\\Учеба\\ТИ и тервер\\ТИ\\laba_2\\text.txt";

            string text = File.ReadAllText(path);
            text = text.ToLower();
            text = Regex.Replace(text, "[-?,.!]", "");
            return text;
        }
        static Dictionary<char, double> AppendTable(string text)
        {
            Dictionary<char, double> Probib = new Dictionary<char, double>();
            int lengt = text.Length;
                for (int i = 0; i < 30; i++)
                {
                    int finded = 0;
                    foreach (var item in text)
                    {
                        if (alphabet[i] == item)
                        {
                            finded++;
                        }
                    }
                    Probib.Add(alphabet[i], Math.Round((double)finded / lengt, 4));
                    //Console.WriteLine($"{alphabet[i]}  {Math.Round((double)finded / lengt, 3)}");
                }
            return Probib;
        }
    }
}
