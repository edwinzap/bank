using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Bank
{
    class ExtraitsPDF
    {
        //Mettre en static ???
        public string Path { get; set; }
        public ExtraitsPDF(string path)
        {
            Path = path;
        }
        private string ReadPdfFile(string fileName)
        {
            try
            {
                PdfReader reader = new PdfReader(fileName);

                string text = string.Empty;

                for (int page = 1; page < reader.NumberOfPages; page++)
                {
                    text += PdfTextExtractor.GetTextFromPage(reader, page, new LocationTextExtractionStrategy()) + " ";
                }
                reader.Close();

                return text.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return "error";
        }
        public List<Extrait> Import()
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("fr-FR");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            string text = ReadPdfFile(this.Path);
            int cursorTop = Console.CursorTop;
            List<Extrait> extraits = new List<Extrait>();

            try
            {
                List<List<string>> stringExtraits = new List<List<string>>();

                string[] lines = text.Split('\n');
                int j = 0;

                //Boucle servant à trouver le début du PDF (là où les extraits commencent vraiment)
                while (Regex.IsMatch(lines[j], @"^-?\d{1,5},\d{2}") == false)
                {
                    j++;
                }
                List<string> extrait = new List<string>();
                

                for (int i = j - 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    //A revoir (un commentaire pourrait contenir "Aujourd'hui" !
                    if ((Regex.IsMatch(line, @"^\d{2}\.\d{2}\.\d{4}") || line.Contains("Disclaimer") || line.Contains("Aujourd'hui") || line.Contains("Hier")) == false)
                    {
                        extrait.Add(line);

                        if (line.Contains("Numéro de séquence"))
                        {
                            extraits.Add(GetData(extrait)); //Extrait les données et renvois un Extrait que l'on ajoute à la liste
                            extrait = new List<string>(); //Vide l'extrait pour le remplir avec un autre
                        }
                        Console.SetCursorPosition(0, cursorTop);
                        Console.WriteLine(string.Format("Lecture du fichier pdf: {0}%", Math.Ceiling((decimal)i/lines.Length * 100)));
                        //   System.Threading.Thread.Sleep(10);       
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return extraits;
            
        }
        private Extrait GetData(List<string> textExtrait)
        {
            Dictionary<string, string> patterns = new Dictionary<string, string>();

            patterns.Add("montant", @"-?\d{1,5},\d{2}");
            patterns.Add("compte", @"^[A-Z]{2}(\d *){14}");
            //patterns.Add("date", @"(\d{2}.\d{2}.\d{4})");
            patterns.Add("sequence", @"^Numéro de séquence (\d{4})?");
            patterns.Add("dateExecution", @"^Date d'exécution (\d{2}.\d{2}.\d{4})"); // Chercher @"^Date d'exécution"
            patterns.Add("dateValeur", @"^Date valeur (\d{2}.\d{2}.\d{4})");
            patterns.Add("communication", @"COMMUNICATION ?:");
            patterns.Add("reference", @"^REFERENCE");
            patterns.Add("mandat", @"^NUMERO DE MANDAT");
            /* Disposition d'un extrait:
             * -------------------------
             * 1ère ligne: bénificiaire OU Avec la carte
             * 2ème ligne: montant
             * 3ème ligne: numéro de compte OU bénificiaire (Avec la carte...) OU Adresse du bénéficiaire (sur plusieurs lignes !)
             * Si "COMMUNICATION :" : communication sur la/les lignes suivantes (!!parfois sur la même ligne)
             * Si "Date d'exécution" : date exécution en format 00.00.0000
             * Si "Date valeur" : date valeur en format 00.00.0000
             * Si "Numéro de séquence" : numéro séquence en format 0000 (à ensuite reformater en année-numéroDeSéquence (2016-0123)
             */

            Extrait extrait = new Extrait();

            
            extrait.Details = String.Join(System.Environment.NewLine,textExtrait.ToArray());
            try
            {
                for (int i = 0; i < textExtrait.Count; i++)
                {
                    string text = textExtrait[i];
                    if (i == 0)
                    {
                        if (text.Contains("AVEC LA CARTE"))
                        {
                            extrait.Beneficiaire.Nom = textExtrait[i + 2];
                            extrait.AvecCarte = true;
                        }
                        else
                        {
                            extrait.Beneficiaire.Nom = text;
                        }
                    }
                    foreach (var item in patterns)
                    {
                        Match m = Regex.Match(textExtrait[i], item.Value);
                        if (m.Success)
                        {
                            switch (item.Key)
                            {
                                case "montant":
                                    extrait.Montant = Convert.ToDecimal(m.Value);
                                    break;
                                case "compte":
                                    extrait.Beneficiaire.Compte = m.Value.Trim().Replace(" ", string.Empty);
                                    break;
                                case "dateExecution":
                                    extrait.DateExecution = Convert.ToDateTime(m.Groups[1].Value);
                                    break;
                                case "dateValeur":
                                    extrait.DateValeur = Convert.ToDateTime(m.Groups[1].Value);
                                    break;
                                case "sequence":
                                    extrait.Sequence = Convert.ToInt32(m.Groups[1].Value);
                                    break;
                                case "mandat":
                                    extrait.Mandat = textExtrait[++i];
                                    break;
                                case "reference":
                                    extrait.Reference = textExtrait[++i];
                                    break;
                                case "communication":
                                    i++;
                                    string comment = string.Empty;
                                    while (textExtrait[i].Contains("DATE VALEUR") == false && textExtrait[i].Contains("EXECUTE LE")==false )
                                    {
                                        comment += textExtrait[i++];
                                    }
                                    extrait.Communication = comment;
                                    break;
                                default:
                                    Console.WriteLine("Error : " + item.Key);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
            //extrait.PrintDetails();
            //extrait.Print();
            return extrait;
        }
    }
}
