using System;

namespace CSharp.Models
{
    public class Animal
    {
        public string Nom { get; set; }
        public int RevenuQuotidien { get; set; }
        public int Prix { get; set; }

        public Animal(string nom, int revenuQuotidien, int prix)
        {
            Nom = nom;
            RevenuQuotidien = revenuQuotidien;
            Prix = prix;
        }
    }
}