using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Balizas.Models;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Balizas
{
    internal class Database
    {
        MongoClient dbClient = new MongoClient("mongodb://localhost:27017");
       
        public void Insert(Reading reading)
        {
            var database = dbClient.GetDatabase("Balizas");
            var tabla = database.GetCollection<Reading>("readings");
            var filter = Builders<Reading>.Filter.Eq( r=> r.id, reading.id);
           // try
           // {
                tabla.InsertOne(reading);
            Debug.WriteLine(reading.id);
            //}
           /* catch
            {
                Debug.WriteLine("LOOOOOL");
                tabla.ReplaceOne(filter,reading);
            }*/
           

        }
        public void Insert(Baliza baliza)
        {
            var database = dbClient.GetDatabase("Balizas");
            var tabla = database.GetCollection<Baliza>("balizas");
            tabla.InsertOne(baliza);

        }
        public void InsertAll(List<Reading> readings)
        {
            var database = dbClient.GetDatabase("Balizas");
            var tabla = database.GetCollection<Reading>("readings");
            foreach(Reading r in readings)
            {
                Debug.WriteLine("Guardo el registro " + r.id);
            }
            tabla.InsertMany(readings);

        }
        public void InsertAll(List<Baliza> balizas)
        {
            var database = dbClient.GetDatabase("Balizas");
            var tabla = database.GetCollection<Baliza>("balizas");
            try
            {
                tabla.InsertMany(balizas);
            }
            catch 
            {

            }
            

        }
        public List<Reading> GetReadings()
        {
            var database = dbClient.GetDatabase("Balizas");
            var table = database.GetCollection<Reading>("readings");
            List<Reading> readings = new List<Reading>();  
            readings = table.Find(d => true).ToList();
            foreach(Reading r in readings)
            {
                Debug.WriteLine("dtenperature " + r.temperature);
                Debug.WriteLine("dprecipitation " + r.precipitation);
                Debug.WriteLine("dhumidity " + r.humidity);
                Debug.WriteLine("dirradiance " + r.irradiance);

            }
            return readings;
        }
        public List<Baliza> GetBalizas()
        {
            var database = dbClient.GetDatabase("Balizas");
            var table = database.GetCollection<Baliza>("balizas");
            List<Baliza> balizas = new List<Baliza>();
            balizas = table.Find(d => true).ToList();
            return balizas;
        }
        public void deleteAllReadings()
        {
            var database = dbClient.GetDatabase("Balizas");
            
            try
            {
                database.DropCollection("readings");
            }
            catch
            {

            }
            
        }
       
    }
}
