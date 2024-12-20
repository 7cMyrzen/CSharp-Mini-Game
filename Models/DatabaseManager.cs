using System;
using System.Data.SQLite;
using System.Collections.Generic;

namespace CSharp.Models
{
    public class DatabaseInfo
    {
        public string Name { get; set; }
        public int Argent { get; set; }
        public int Jours { get; set; }
        public int Terrain { get; set; }
        public int NbVisiteurs { get; set; }
        public int Dette { get; set; }
        public List<Animal> Animaux { get; set; }

        public DatabaseInfo(string name, int argent, int jours, int terrain, int nbVisiteurs, int dette, List<Animal> animaux)
        {
            Name = name;
            Argent = argent;
            Jours = jours;
            Terrain = terrain;
            NbVisiteurs = nbVisiteurs;
            Dette = dette;
            Animaux = animaux;
        }
    }

    public class DatabaseManager
    {
        private string _connectionString;

        public DatabaseManager(string dbPath)
        {
            // Initialise la chaîne de connexion avec le chemin vers la BDD
            _connectionString = $"Data Source={dbPath};Version=3;";
        }

        public void InitializeDatabase(int argentInitial, int detteInitiale, int jours, int terrain, int NbVisiteurs, Animal animal)
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                // Création de table Zoo si elle n'existe pas
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Zoo (
                        Name TEXT NOT NULL,
                        Argent INT NOT NULL,
                        Jours INT NOT NULL,
                        Terrain INT NOT NULL,
                        NbVisiteurs INT NOT NULL,
                        Dette INT NOT NULL
                    );
                ";
                using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Création de table Animal si elle n'existe pas
                createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Animal (
                        Nom TEXT NOT NULL,
                        RevenuQuotidien INT NOT NULL,
                        Prix INT NOT NULL,
                        Nombre INT NOT NULL
                    );
                ";
                using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            InsertZooTuzZoo(argentInitial, detteInitiale, jours, terrain, NbVisiteurs, animal);
        }

        public void InsertZooTuzZoo(int argentInitial, int detteInitiale, int jours, int terrain, int NbVisiteurs, Animal animal)
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                // Insertion de L'Animal de base si la table Animal est vide
                string selectQuery = "SELECT COUNT(*) FROM Animal;";
                using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, conn))
                {
                    long count = (long)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        string insertQuery = $"INSERT INTO Animal (Nom, RevenuQuotidien, Prix, Nombre) VALUES ('{animal.Nom}', {animal.RevenuQuotidien}, {animal.Prix}, 1);";
                        using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn))
                        {
                            insertCmd.ExecuteNonQuery();
                        }
                        // Si la table Animal était vide, la table Zoo l'est aussi
                        string insertZooQuery = $"INSERT INTO Zoo (Name, Argent, Jours, Terrain, NbVisiteurs, Dette) VALUES ('TuzZoo', {argentInitial}, {jours}, {terrain}, {NbVisiteurs}, {detteInitiale});";
                        using (SQLiteCommand insertZooCmd = new SQLiteCommand(insertZooQuery, conn))
                        {
                            insertZooCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public void UpdateDatabase(int argent, int dette, int jours, int terrain, int NbVisiteurs)
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                string updateQuery = $"UPDATE Zoo SET Argent = {argent}, Jours = {jours}, Terrain = {terrain}, NbVisiteurs = {NbVisiteurs}, Dette = {dette} WHERE Name = 'TuzZoo';";
                using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertAnimal(Animal animal)
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                string selectQuery = $"SELECT COUNT(*) FROM Animal WHERE Nom = '{animal.Nom}';";
                using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, conn))
                {
                    long count = (long)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        string insertQuery = $"INSERT INTO Animal (Nom, RevenuQuotidien, Prix, Nombre) VALUES ('{animal.Nom}', {animal.RevenuQuotidien}, {animal.Prix}, 1);";
                        using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn))
                        {
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string updateQuery = $"UPDATE Animal SET Nombre = Nombre + 1 WHERE Nom = '{animal.Nom}';";
                        using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn))
                        {
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        // Renvoyer tout le contenu de la table Zoo et Animal
        public DatabaseInfo GetDatabaseInfo()
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                string selectQuery = "SELECT * FROM Zoo WHERE Name = 'TuzZoo';";
                using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader.GetString(0);
                            int argent = reader.GetInt32(1);
                            int jours = reader.GetInt32(2);
                            int terrain = reader.GetInt32(3);
                            int NbVisiteurs = reader.GetInt32(4);
                            int dette = reader.GetInt32(5);
                            List<Animal> animaux = new List<Animal>();

                            selectQuery = "SELECT * FROM Animal;";
                            using (SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, conn))
                            {
                                using (SQLiteDataReader animalReader = selectCmd.ExecuteReader())
                                {
                                    while (animalReader.Read())
                                    {
                                        string nom = animalReader.GetString(0);
                                        int revenuQuotidien = animalReader.GetInt32(1);
                                        int prix = animalReader.GetInt32(2);
                                        int nombre = animalReader.GetInt32(3);
                                        for (int i = 0; i < nombre; i++)
                                        {
                                            animaux.Add(new Animal(nom, revenuQuotidien, prix));
                                        }
                                    }
                                }
                            }
                            return new DatabaseInfo(name, argent, jours, terrain, NbVisiteurs, dette, animaux);
                        }
                    }
                }
            }
            return null;
        }
    }
}
