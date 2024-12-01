using System;

namespace CSharp.Models
{
    public class Dette
    {
        public int Montant { get; set; }

        public Dette(int montant)
        {
            Montant = montant;
        }
    }
}