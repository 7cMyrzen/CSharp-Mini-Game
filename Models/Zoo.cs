using System;
using System.Data.SQLite;
using System.Collections.Generic;

namespace CSharp.Models
{
    public class Zoo
    {
        public int Argent { get; private set; }
        public int Jours { get; private set; }
        public List<Animal> Animaux { get; private set; }
        public int Terrain { get; private set; } // Pourcentage utilisé pour les animaux
        public int NbVisiteurs { get; private set; } // Capacité des visiteurs
        public int Dette { get; private set; }
        public int TerrainMax { get; private set; } = 100;
        public int TerrainParExpansion { get; private set; } = 25;
        public int MaxJours { get; private set; } = 50; // Temps de jeu ajusté
        public DatabaseManager DatabaseManager { get; private set; }

        public List<Animal> AnimauxDisponibles { get; private set; } = Boutique();

        public Zoo(int argentInitial, int detteInitiale, string dbPath, DatabaseManager databaseManager)
        {
            DatabaseManager = databaseManager;
            int argentDebut = argentInitial;
            int  detteDebut = detteInitiale;
            Animal AnimalDebut = new Animal("Mouton", 10, 50);
            int terrainDebut = 50;
            int NbVisiteursDebut = (int)(terrainDebut * 0.25);

            DatabaseManager.InitializeDatabase(argentInitial, detteInitiale, 0, terrainDebut, NbVisiteursDebut, AnimalDebut);
            DatabaseInfo databaseInfo = DatabaseManager.GetDatabaseInfo();

            Argent = databaseInfo.Argent;
            Dette = databaseInfo.Dette;
            Jours = databaseInfo.Jours;
            Terrain = databaseInfo.Terrain;
            NbVisiteurs = databaseInfo.NbVisiteurs;
            Animaux = databaseInfo.Animaux;

        }

        static List<Animal> Boutique()
        {
            List<Animal> boutique = new List<Animal>
            {
                new Animal("Poulet", 5, 30),
                new Animal("Mouton", 10, 50),
                new Animal("Vache", 25, 100),
                new Animal("Cheval", 50, 200),
                new Animal("Girafe", 120, 900),
                new Animal("Éléphant", 150, 1000),
                new Animal("Lion", 200, 1500),
                new Animal("Tigre", 250, 1800),
                new Animal("Panda", 300, 2000),
                new Animal("Phénix", 500, 5000),
                new Animal("Licorne", 750, 6000),
                new Animal("Dragon", 1000, 7500)
            };
            return boutique;
        }

        public void CollecterRevenus()
        {
            int revenus = 0;
            foreach (var animal in Animaux)
            {
                revenus += animal.RevenuQuotidien;
            }
            Argent += revenus; // Ajoute les revenus quotidiens
            Argent += NbVisiteurs * 10; // Prix d'entrée
            Jours++; // Passe au jour suivant
            DatabaseManager.UpdateDatabase(Argent, Dette, Jours, Terrain, NbVisiteurs);
        }

        public void AcheterAnimal(Animal animal)
        {
            // Vérifie si assez de terrain est disponible
            if (Terrain - (Animaux.Count * 5) < 5)
            {
                return;
            }

            if (Argent >= animal.Prix)
            {
                Argent -= animal.Prix;
                Animaux.Add(animal);
                DatabaseManager.UpdateDatabase(Argent, Dette, Jours, Terrain, NbVisiteurs);
                DatabaseManager.InsertAnimal(animal);
            }
            else
            {
                return;
            }
        }

        public void AgrandirTerrain()
        {
            if (Terrain < TerrainMax)
            {
                int prixExpansion = TerrainParExpansion * 40;
                if (Argent >= prixExpansion)
                {
                    Argent -= prixExpansion;
                    Terrain += TerrainParExpansion;
                    if (Terrain > TerrainMax) Terrain = TerrainMax;
                    NbVisiteurs = (int)(Terrain * 0.25);
                    DatabaseManager.UpdateDatabase(Argent, Dette, Jours, Terrain, NbVisiteurs);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        public void PayerDette(Dette dette)
        {
            if (Argent >= dette.Montant)
            {
                Argent -= dette.Montant;
                Dette -= dette.Montant;
                if (Dette < 0) Dette = 0;
                DatabaseManager.UpdateDatabase(Argent, Dette, Jours, Terrain, NbVisiteurs);
            }
            else
            {
                return;
            }
        }

        public bool JeuTermine()
        {
            return Dette == 0 || Jours >= MaxJours;
        }

        public bool JoueurGagne()
        {
            return Dette == 0;
        }
    }
}