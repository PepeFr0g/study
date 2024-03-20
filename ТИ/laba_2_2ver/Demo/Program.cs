using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Demo
{
    internal class Program
    {
        static char[] alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();

        public static void Main(string[] args)
        {

            Dictionary<char, double> dictionary1 = new Dictionary<char, double>();

            // Добавляем элементы в словарь
            dictionary1['a'] = 1.0;
            dictionary1['b'] = 2.0;
            dictionary1['c'] = 3.0;
            dictionary1['d'] = 4.0;

            Dictionary<char, double> dictionary2 = new Dictionary<char, double>();

            // Добавляем элементы в словарь
            dictionary2['c'] = 1.0;
            dictionary2['b'] = 2.0;
            dictionary2['a'] = 3.0;
            dictionary2['d'] = 4.0;

            Dictionary<char, char> replacements = DecryptionText(dictionary1, dictionary2);

            Console.ReadKey();

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
            foreach (var item in DeCryptedTextTable)
            {
                Console.WriteLine("Ключ: {0}, Значение: {1}", item.Key, item.Value);
            }
            Console.WriteLine("__________________");
            foreach (var item in reversedDictionary2)
            {
                Console.WriteLine("Ключ: {0}, Значение: {1}", item.Key, item.Value);
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
                        Console.WriteLine("Значение успешно добавлено");
                    }
                    Console.WriteLine("Программа не нашла совпадений");
                }
            }
            foreach (var item in replacements)
            {
                Console.WriteLine("Ключ: {0}, Значение: {1}", item.Key, item.Value);
            }
            return replacements;

        }
    }
}