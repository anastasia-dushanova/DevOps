using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PR1
{
    
    class Program
    {
     
        static void Main(string[] args)
        {
            string task = File.ReadAllText("orig.txt");
            Cesar(task);  //шифруем с помощью алгоритма Цезаря
            DeleteSimbolsTask("orig.txt"); //убираем лишнии символы в исходнике
            DeleteSimbolsAnswer("shifr.txt"); //убираем лишнии символы в зашифрованном тексте
            Frequency("orig_DeleteSim.txt", "shifr_DeleteSim.txt");
            Decrypt_Mono("orig.txt", "shifr.txt", "shifr.txt"); //дешифровка с монограммами             
            Decrypt_Bi("orig.txt", "shifr.txt", "shifr.txt"); //дешифровка с биграммами
        }
        /// <summary>
        /// Частотный анализ
        /// </summary>
        /// <param name="orig_DeleteSim"></param>
        /// <param name="shifr_DeleteSim"></param>
        /// <returns></returns>
        public static string Frequency(string orig_DeleteSim, string shifr_DeleteSim)
        {
            //orig - словарь из исходного текста
            //shifr - словарь из зашифрованного текста
            Console.WriteLine("Исходный текст\n");
            string text = File.ReadAllText(orig_DeleteSim); 
            var groups = SwimParts(text, 2).GroupBy(str => str); //группируем монограммы
            //и делаем словарь, где ключ - символ, а значение - частота символов
            Dictionary<string, double> orig = groups.ToDictionary(g => g.Key, g => (g.Count() * 100.0) / (double)text.Length);
            orig = orig.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value); //упорядочиваем это поуменьшению
            foreach (var item in orig)
                Console.WriteLine(item.Key + " " + item.Value); //вывод в консоль

            Console.WriteLine("\nЗашифрованый текст методом Цезаря\n");

            text = File.ReadAllText(shifr_DeleteSim); //считаем частоту в зашифрованом

            groups = SwimParts(text, 2).GroupBy(str => str);
            Dictionary<string, double> shifr = groups.ToDictionary(g => g.Key, g => (g.Count() * 100.0) / (double)text.Length);
            shifr = shifr.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (var item in shifr)
                Console.WriteLine(item.Key + " " + item.Value); //вывод в консоль
            //создаю массивы из ключей словарей
            var arr_orig = orig.Select(z => z.Key).ToArray();
            var arr_shifr = shifr.Select(z => z.Key).ToArray();
            for (int i = 0; i < arr_orig.Length; i++)
            {
                Console.WriteLine("номер: {0}\tarr_orig: {1}\tarr_shifr:  {2}; ", i, arr_orig[i], arr_shifr[i]); //вывод в консоль
            }

            return text;
        }
        /// <summary>
        /// Алгоритм Цезаря
        /// </summary>
        /// <param name="text">исходный текст, который необходимо зашифровать</param>
        /// <returns>текст зашифрованный</returns>
        public static string Cesar(string text)
        {
            string result_encryp; 
            int num, j, key;
            char[] message = text.ToLower().ToCharArray(); //делаем нижний регистр для всего текста
            char[] alphabetLow = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' };

            for (int i = 0; i < message.Length; i++) 
            {
                for (j = 0; j < alphabetLow.Length; j++)
                {
                    if (message[i] == alphabetLow[j])
                    {
                        break;
                    }
                }
                if (j != 33)
                {
                    num = j;
                    key = num + 3; //сдвиг на 3 буквы вправо
                    if (key > 32)
                    {
                        key = key - 33; //"петля", если заканчивается алфавит, возвращаемся в его начало
                    }
                    message[i] = alphabetLow[key]; //соответсвенно сама замена символов
                }
            }
            result_encryp = new string(message);
            File.WriteAllText("shifr.txt", result_encryp); //записываем в файл
            return result_encryp; 
        }
        /// <summary>
        /// Необходимо убрать ненужные символы: пробелы, запятые и тд
        /// </summary>
        /// <param name="encryptText">текст</param>
        /// <returns>текст без доп символов</returns>
        public static string DeleteSimbolsTask(string encryptText)
        {
            string text = File.ReadAllText(encryptText);
            var charsToRemove = new string[] { "@", ",", ".", ";", "'", " ", "(", ")", "!", "—", "?", "\n", "\r", "0", "1", "2", "7", "8", "5", "6", "<", ">" };
            foreach (var c in charsToRemove)
            {
                text = text.Replace(c, string.Empty);
            }
            File.WriteAllText("orig_DeleteSim.txt", text);
            return text;
        }
        /// <summary>
        /// Необходимо убрать ненужные символы: пробелы, запятые и тд
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns></returns>
        public static string DeleteSimbolsAnswer(string encryptText)
        {
            string text = File.ReadAllText(encryptText);
            var charsToRemove = new string[] { "@", ",", ".", ";", "'", " ", "(", ")", "!", "—", "?", "\n", "\r", "0", "1", "2", "7", "8", "5", "6", "<", ">" };
            foreach (var c in charsToRemove)
            {
                text = text.Replace(c, string.Empty);
            }
            File.WriteAllText("shifr_DeleteSim.txt", text);
            return text;
        }
        /// <summary>
        /// Дешиврование с помощью частотного анализа (монограммы)
        /// </summary>
        /// <param name="orig_DeleteSim"></param>
        /// <param name="shifr_DeleteSim"></param>
        /// <param name="shifruem"></param>
        /// <returns></returns>
        public static string Decrypt_Mono(string orig_DeleteSim, string shifr_DeleteSim, string shifruem)
        {
            //orig - словарь из исходного текста
            //shifr - словарь из зашифрованного текста
            string text =File.ReadAllText(orig_DeleteSim); //считаем частоту в исходнике
            var groups = SwimParts(text, 1).GroupBy(str => str);
            Dictionary<string, double> orig = groups.ToDictionary(g => g.Key, g => (g.Count() * 100.0) / (double)text.Length);
            orig = orig.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            text = File.ReadAllText(shifr_DeleteSim); //считаем частоту в зашифрованом
            
            groups = SwimParts(text, 1).GroupBy(str => str);
            Dictionary<string, double> shifr = groups.ToDictionary(g => g.Key, g => (g.Count() * 100.0) / (double)text.Length);
            shifr = shifr.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            //=====================================================================================================
            // к этому моменту все словари уже упорядоченны, наиболее встречающиеся буквы в начале

            var arr_orig = orig.Select(z => z.Key).ToArray();
            var arr_shifr = shifr.Select(z => z.Key).ToArray();
            text = File.ReadAllText(shifruem);
            var war = text.ToCharArray().Select(c => c.ToString()).ToArray(); //массив из символов текста
            string result = null;
            for (int i=0; i< war.Length; i++)
            {
                int a = Array.IndexOf(arr_shifr, war[i]); //ищу символ из текста в массиве част анализа зашифр текста
                result += arr_orig[a]; //и заменяем на ориг словарь
                
            }
            File.WriteAllText("finale_mono.txt", result);
            return result;
        }
        /// <summary>
        /// Дешиврование с помощью частотного анализа (монограммы)
        /// </summary>
        /// <param name="orig_DeleteSim"></param>
        /// <param name="shifr_DeleteSim"></param>
        /// <param name="shifruem"></param>
        /// <returns></returns>
        public static string Decrypt_Bi(string orig_DeleteSim, string shifr_DeleteSim, string shifruem)
        {
            string text = File.ReadAllText(orig_DeleteSim); //считаем частоту в исходнике

            var groups = SwimParts(text, 2).GroupBy(str => str);
            Dictionary<string, double> orig = groups.ToDictionary(g => g.Key, g => (g.Count() * 100.0) / (double)text.Length);
            orig = orig.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            text = File.ReadAllText(shifr_DeleteSim); //считаем частоту в зашифрованом

            groups = SwimParts(text, 2).GroupBy(str => str);
            Dictionary<string, double> shifr = groups.ToDictionary(g => g.Key, g => (g.Count() * 100.0) / (double)text.Length);
            shifr = shifr.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            //=====================================================================================================
            // к этому моменту все словари уже упорядоченны, наиболее встречающиеся буквы в начале

            var arr_orig = orig.Select(z => z.Key).ToArray();
            var arr_shifr = shifr.Select(z => z.Key).ToArray();
            text = File.ReadAllText(shifruem);
            string[] b = new string[text.Length/2];
            int k = 0;
            for(int i=0; i<text.Length; i=i+2)
            {
                b[k] = text.Substring(i, 2);
                k++;
            }
            string result = null;
            for (int i = 0; i < b.Length; i++)
            {
                int a = Array.IndexOf(arr_shifr, b[i]); //ищу символ из текста в массиве част анализа зашифр текста
                result += arr_orig[a];
            }
            File.WriteAllText("finale_bi.txt", result);
            return result;
        }
        /// <summary>
        /// Дробление строк на подстроки
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<string> SwimParts(string source, int length)
        {
             for (int i = length; i <= source.Length; i++)
                yield return source.Substring(i - length, length);
        }
    }
}

