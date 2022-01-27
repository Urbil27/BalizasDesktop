using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;


namespace Balizas
{
    internal class Database
    {
        public String Connect()
        {
            Console.WriteLine("Entro a la funcion");
            MongoClient dbClient = new MongoClient("mongodb://localhost:27017");
            var dbList = dbClient.ListDatabases().ToList();
            foreach (var db in dbList)
            {
                Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                Console.WriteLine(db);
              //  return db;
            }
            return "hola";
        }
    }
}
