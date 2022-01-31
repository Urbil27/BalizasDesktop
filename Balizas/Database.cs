﻿using System;
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
            tabla.InsertOne(reading);

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
            tabla.InsertMany(readings);

        }
        public void InsertAll(List<Baliza> balizas)
        {
            var database = dbClient.GetDatabase("Balizas");
            var tabla = database.GetCollection<Baliza>("balizas");
            tabla.InsertMany(balizas);

        }
        public List<Reading> GetReadings()
        {
            var database = dbClient.GetDatabase("Balizas");
            var table = database.GetCollection<Reading>("readings");
            List<Reading> readings = new List<Reading>();  
            readings = table.Find(d => true).ToList();
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
    }
}