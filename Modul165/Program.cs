using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Modul165
{
    class Program
    {
        static void Main(string[] args)
        {
            // Verbindungszeichenfolge zur MongoDB
            var connectionString = "mongodb://localhost:27017";
            var databaseName = "LB_165_GabrielBischof";
            var collectionName = "weekly_deaths";

            // MongoDB-Client erstellen
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            // Aggregationspipeline definieren
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument
                {
                    { "Year", 2010 },
                    { "Week", new BsonDocument("$gt", 0) },
                    { "Expected", new BsonDocument("$gt", 100) }
                })
            };

            // Aggregation ausführen und Ergebnisse anzeigen
            var result = collection.Aggregate<BsonDocument>(pipeline).ToList();
            foreach (var document in result)
            {
                var week = document.GetValue("Week", 0);
                var year = document.GetValue("Year", 0);
                var expectedDeaths = document.GetValue("Expected", 0);

                Console.WriteLine($"Week:{week}\n{year}:\nExpected deaths: {expectedDeaths}");
            }
        }
    }
}

