using System;
using System.Data.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CSharp.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajouter le service CORS
builder.Services.AddCors();

var app = builder.Build();

// Configurer CORS pour permettre toutes les origines, méthodes, et en-têtes
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins("file://")
);

// Servir les fichiers statiques depuis le dossier "frontend"
app.UseDefaultFiles(); // Définit "index.html" comme page par défaut
app.UseStaticFiles();  // Permet de servir les fichiers CSS, JS, etc.

// Définition de la base de données et initialisation du zoo
const string dbPath = "Database/MyDatabase.sqlite";
Zoo zoo = InitializeZoo(dbPath);

// Endpoints pour l'API
app.MapGet("/zoo", () => zoo);

app.MapPost("/collecter", () =>
{
    zoo.CollecterRevenus();
    return zoo;
});

app.MapPost("/acheter", (Animal animal) =>
{
    zoo.AcheterAnimal(animal);
    return zoo;
});

app.MapPost("/agrandir", () =>
{
    zoo.AgrandirTerrain();
    return zoo;
});

app.MapPost("/rembourser", (Dette dette) =>
{
    zoo.PayerDette(dette);
    return zoo;
});

// Démarrer l'application
app.Run("http://localhost:5105");

/// <summary>
/// Initialise le zoo et la base de données.
/// </summary>
/// <param name="dbPath">Chemin vers la base de données SQLite.</param>
/// <returns>Une instance de Zoo initialisée.</returns>
Zoo InitializeZoo(string dbPath)
{
    if (!System.IO.File.Exists(dbPath))
    {
        SQLiteConnection.CreateFile(dbPath);
    }

    DatabaseManager databaseManager = new DatabaseManager(dbPath);
    return new Zoo(1000, 4000, dbPath, databaseManager);
}
