using Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bank
{
    class ExtraitsCSV
    {
        public static void Import(string path)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("fr-FR");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            try
            {
                using (StreamReader rdr = new StreamReader(path, Encoding.Default))
                {

                    foreach (var item in CsvReader.Read(rdr))
                    {
                        string s = item[item.ColumnCount - 2];
                        Console.WriteLine(SplitDetails(s)[0]); 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            

            Console.ReadLine();
        } 

        private static string[] SplitDetails(string text)
        {
            string[] separators = new string[] { "DATE VALEUR :" ,"EXECUTE LE " };
            string[] split = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            separators = new string[] { "COMMUNICATION :" };
            string[] split2 = split[0].Split(separators, StringSplitOptions.RemoveEmptyEntries);

            string[] final = new string[2];
            final[0] = split2[0];
            if (split2.Length >1 )
            {
                final[1] = split2[1];
            }
            else
            {
                final[1] = null;
            }
            return final;
        }

        private static void SplitDetailsRegex(string text)
        {
            string input = "123xx456yy789";
            string pattern = "(COMMUNICATION: |DATE VALEUR:)";
            string[] result = Regex.Split(input, pattern);

        }
    }
}
