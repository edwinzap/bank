using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Bank
{
    public class ExportCSV
    {
        public enum ExportOption { Basic, Advanced }
        public static void Export(string path, List<Extrait> extraits, ExportOption option)
        {
            try
            {
                Console.WriteLine("Mode d'exportation: {0}", option.ToString());
                using (TextWriter tw = new StreamWriter(path, false, Encoding.Default))
                {
                    if (option == ExportOption.Basic)
                    {
                        tw.WriteLine("Numéro de séquence;Date d'exécution;Date valeur;Montant;Devise du compte;Contrepartie;Communication;Numéro de compte");
                        foreach (Extrait extrait in extraits)
                        {
                            tw.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}",
                                extrait.NumeroSequence,
                                extrait.DateExecution.ToShortDateString(),
                                extrait.DateValeur.ToShortDateString(),
                                extrait.Montant,
                                extrait.Devise==0?"EUR":"NULL",
                                extrait.Beneficiaire.Nom,
                                extrait.Communication,
                                extrait.Beneficiaire.Compte
                                );
                        }
                    }
                    else
                    {
                        tw.WriteLine("Numéro de séquence;Date d'exécution;Date valeur;Montant;Devise du compte;Contrepartie;Communication;Numéro de compte;Avec la carte;Numéro de mandat; Référence");
                        foreach (Extrait extrait in extraits)
                        {
                            tw.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}",
                                extrait.NumeroSequence,
                                extrait.DateExecution.ToShortDateString(),
                                extrait.DateValeur.ToShortDateString(),
                                extrait.Montant,
                                extrait.Devise,
                                extrait.Beneficiaire.Nom,
                                extrait.Communication,
                                extrait.Beneficiaire.Compte,
                                extrait.AvecCarte,
                                extrait.Mandat,
                                extrait.Reference);
                        }
                    }
                    tw.Close();
                    Console.WriteLine("Exportation du CSV terminée !");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur !");
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
