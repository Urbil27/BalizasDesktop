using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Balizas.Models
{
    internal class Reading
    {
        [BsonId]
        [BsonElement("id")]
        public String id { get; set; }
        [BsonElement("BalizaID")]
        public string BalizaID { get; set; }
        [BsonElement("Datetime")]
        public string Datetime { get; set; }
        
        [BsonElement("Temperature")]
        public double temperature { get; set; }
        [BsonElement("Humidity")]
        public double humidity { get; set; }
        [BsonElement("Precicitation")]
        public double precipitation { get; set; }
        [BsonElement("Irradiance")]
        public double irradiance { get; set; }
        public Reading()
        {

        }
        public Reading(string balizaID,DateTime dateTime, double temperature, double humidity, double precipitation, double irradiance)
        {
            this.BalizaID = balizaID;
            this.Datetime = dateTime+"";
            this.temperature = temperature;
            this.humidity = humidity;
            this.precipitation = precipitation;
            this.irradiance = irradiance;
        }
        public void save()
        {
            Database database = new Database();
            database.Insert(this);

        }
    }
}
