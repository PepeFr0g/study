using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace laba_2
{

    internal class Program
    {
        static char[] alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя".ToCharArray();

        static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\leone\\Desktop\\Учеба\\ТИ и тервер\\ТИ\\laba_2\\laba_2\\Database1.mdf\";Integrated Security=True";
        public static void Main(string[] args)
        {
            bool infinity = true;
            while (infinity)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                string text = Text();
                string DycrypText = text;
                //Console.WriteLine(text);
                //Console.WriteLine("End");
                AppendTable("Probib", text);
                text = ReplaceLetters(text);
                AppendTable("encryptedTable", text);
                //Console.WriteLine("___________________________");
                //Console.WriteLine(text);
                text = DecryptionTextReplacer(text);
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
                if ((float)matches / text.Length * 100 > 96) 
                {
                    infinity = false;

                }

            }

            Console.ReadKey();
        }
        static string DecryptionTextReplacer(string text) 
        {
            Dictionary<char, char> replacements = DecryptionText();

            char[] charArray = text.ToCharArray();

            for (int i = 0; i < charArray.Length; i++)
            {
                if (replacements.ContainsKey(char.ToLower(charArray[i])))
                {
                    charArray[i] = replacements[char.ToLower(charArray[i])];
                }
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
        static Dictionary<char, char> DecryptionText()
        {
            Dictionary<char, char> replacements = new Dictionary<char, char>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT Probib.freq AS ColumnName1, encryptedTable.freq AS ColumnName2, Probib.letter AS letters1, encryptedTable.letter AS letters2
                FROM Probib
                INNER JOIN encryptedTable ON ABS(Probib.freq - encryptedTable.freq) < 0.000000001";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Получаем значения из столбцов
                    string valueFromColumn1 = reader["letters1"].ToString();
                    string valueFromColumn2 = reader["letters2"].ToString();
                    
                    // Проверяем, что строка не пустая
                    if (!string.IsNullOrEmpty(valueFromColumn1) && !string.IsNullOrEmpty(valueFromColumn2))
                    {
                        char charFromColumn1 = valueFromColumn1[0];
                        char charFromColumn2 = valueFromColumn2[0];

                        // Проверяем, что ключа еще нет в словаре
                        if (!replacements.ContainsKey(charFromColumn2))
                        {
                            // Добавляем символы в словарь замен
                            replacements.Add(charFromColumn2, charFromColumn1);
                        }
                    }
                }
                return replacements;
            }

            //// Перемешиваем буквы русского алфавита
            //for (int i = 0; i < alphabet.Length; i++)
            //{
            //    //int randomIndex = random.Next(i, alphabet.Length);
            //    //char temp = alphabet[randomIndex];
            //    //alphabet[randomIndex] = alphabet[i];
            //    //alphabet[i] = temp;
            //}

            //// Заполняем словарь замен случайными соответствиями
            //for (int i = 0; i < alphabet.Length; i++)
            //{
            //    replacements.Add((char)('а' + i), alphabet[i]);
            //}

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
            string path = "C:\\Users\\leone\\Desktop\\Учеба\\ТИ и тервер\\ТИ\\laba_2\\text2.txt";

            string text = File.ReadAllText(path);
            text = text.ToLower();
            text = Regex.Replace(text, "[-?,.!]", "");
            return text;
        }
        public static void AppendTable(string nametable,string text) 
        {
            int lengt = text.Length;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string quer = $"TRUNCATE TABLE {nametable}";
                using (SqlCommand command = new SqlCommand(quer, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
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
                    connection.Open();
                    string queries = $"INSERT INTO {nametable} (letter, freq) VALUES (N'{alphabet[i]}', {Math.Round((double)finded / lengt, 6)});";
                    using (SqlCommand command = new SqlCommand(queries, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    //Console.WriteLine($"{alphabet[i]}  {Math.Round((double)finded / lengt, 3)}");
                }
            }
        }
    }
}
