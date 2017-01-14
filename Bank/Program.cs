using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csv;
using System.IO;

namespace Bank
{
    class Program
    {
        //Indent = ctrl+K ctrl+D
        static void Main(string[] args)
        {
            //"D:\Downloads\OK_BANQUE_FORTIS.pdf"
            object[] parameters = GetParameters(args);
            string pdfPath = (string)parameters[0];
            string csvPath = (string)parameters[1];
            ExportCSV.ExportOption option = (ExportCSV.ExportOption)parameters[2];
            //Si le nombre d'argument est supérieur à 1, le premier doit être le nom du fichier pdf

            Console.WriteLine("\n**********************************\n");
           
            ExtraitsPDF pdf = new ExtraitsPDF(pdfPath);
            List<Extrait> extraits = pdf.Import();
            ExportCSV.Export(csvPath,extraits , option);

            foreach (Extrait item in extraits)
            {
                Data.Insert(item);
            }
            Console.Write("Press ENTER to exit");
            Console.ReadLine();

        }

        private static object[] GetParameters(string[] args)
        {
            string pdfPath;
            string csvPath;
            ExportCSV.ExportOption option = ExportCSV.ExportOption.Basic;
            try
            {
                //A Corriger: vérifier si le fichier existe, si oui, vérifier qu'il s'agisse bien d'un fichier pdf, 
                //et enfin, vérifier si le fichier est compatible avec le système d'importation
                if (args.Length >= 2)
                {
                    pdfPath = args[0].Replace("\"", string.Empty);
                    Console.WriteLine("Import path (PDF): " + pdfPath);
                    csvPath = args[1].Replace("\"", string.Empty);
                    Console.WriteLine("Export path (CSV): " + csvPath);
                }
                else
                {
                    do
                    {
                        Console.Write("Import path (PDF): ");
                        pdfPath = Console.ReadLine().Replace("\"", string.Empty);
                    } while (File.Exists(pdfPath) == false || Path.GetExtension(pdfPath) != ".pdf");

                    //Boucle tant que le path n'est pas un .csv
                    do
                    {
                        Console.Write("Export path (CSV): ");
                        csvPath = Console.ReadLine().Replace("\"", string.Empty);
                    } while (Path.GetExtension(csvPath) != ".csv");
                }

                if (args.Length == 3)
                {
                    switch (Convert.ToInt32(args[2]))
                    {
                        case 0:
                            option = ExportCSV.ExportOption.Basic;
                            break;
                        case 1:
                            option = ExportCSV.ExportOption.Advanced;
                            break;
                        case 2:
                            break;
                        default:
                            option = ExportCSV.ExportOption.Basic;
                            break;
                    }
                }
                return new object[] { pdfPath, csvPath, option };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
