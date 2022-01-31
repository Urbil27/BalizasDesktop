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
        public int id { get; set; }
        [BsonElement("BalizaID")]
        public string BalizaID { get; set; }
        [BsonElement("Year")]
        public int Year { get; set; }
        [BsonElement("Month")]
        public int Month { get; set; }
        [BsonElement("Day")]
        public public int Day { get; set; }
        [BsonElement("Hour")]
        public int Hour { get; set; }
        [BsonElement("Minute")]
        public int Minute { get; set; }
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
            this.Year = dateTime.Year; 
            this.Month = dateTime.Month;
            this.Day = dateTime.Day;
            this.Hour = dateTime.Hour;
            this.Minute = dateTime.Minute;
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
