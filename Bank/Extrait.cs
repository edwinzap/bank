using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Extrait
    {
        public Extrait()
        {
            Beneficiaire = new Beneficiaire();
        }

        public int Sequence { get; set; }
        public string NumeroSequence
        {
            get
            {
                if (this.Sequence > 0 && this.DateValeur.Year > 1900 && this.DateValeur.Year <= System.DateTime.Now.Year)
                {
                    return string.Format("{0}-{1}", this.DateExecution.Year, this.Sequence.ToString("0000"));
                }
                else
                {
                    return null;
                }
            }
        }

        public decimal Montant { get; set; }

        public enum DeviseType {EURO, DOLLAR}
        public DeviseType Devise { get; set; } = DeviseType.EURO;
        public DateTime DateExecution { get; set; }
        public DateTime DateValeur { get; set; }
        public string Communication { get; set; }
        public string Mandat { get; set; }
        public string Reference { get; set; }
        public string Details { get; set; }
        public Beneficiaire Beneficiaire { get; set; }
        public bool AvecCarte { get; set; } = false;


        public void Print()
        {
            Console.WriteLine(string.Format("Nom bénéficiaire: {0}\nCompte bénéficiaire: {1}\nMontant: {2}\nCommunication: {3}\nDate Valeur: {4}\nDate d'Exécution: {5}\nNuméro de Séquence: {6}", Beneficiaire.Nom, Beneficiaire.Compte, Montant, Communication, DateValeur.ToShortDateString(), DateExecution.ToShortDateString(), NumeroSequence));
            if (Reference != null)
            {
                Console.WriteLine("Reference: " + Reference);
            }
            if (Mandat != null)
            {
                Console.WriteLine("Numéro de mandat: " + Mandat);
            }
            Console.WriteLine();
        }

        public void PrintDetails()
        {
            foreach (string line in Details.Split('\n'))
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }
    }
}
