using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class Beneficiaire
    {
        public Beneficiaire() { }
        public Beneficiaire(string nom, string compte)
        {
            this.Nom = nom;
            this.Compte = compte;
        }
        public int Id { get; set; } = -1;
        public string Nom { get; set; }
        public string Compte { get; set; }
        public string Adresse { get; set; }
    }     
}
