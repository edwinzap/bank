using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Test
    {
        public static void PrintExtrait(List<string> extrait)
        {
            foreach (string item in extrait )
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }

        public static void PrintExtrait(string[] extrait)
        {
            foreach (string item in extrait)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }

        public static void InsertBeneficiaire()
        {
            Extrait extrait = new Extrait();
            extrait.Beneficiaire.Nom = "Test2";
            extrait.Beneficiaire.Compte = "BE123456879885";
            extrait.Beneficiaire.Adresse = null;

            extrait.DateExecution = System.DateTime.Now;
            extrait.DateValeur = System.DateTime.Now.Subtract(new TimeSpan(30, 0, 0));
            extrait.Montant = (decimal)30.50;

            Data.Insert(extrait);
        }
    }
}
