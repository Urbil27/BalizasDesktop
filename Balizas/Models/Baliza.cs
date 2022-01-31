using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Balizas.Models
{
    class Baliza
    {
        [BsonId]
        [BsonElement("id")]
        public String id { set; get; }
        [BsonElement("name")]
        public String name { set; get; }
        [BsonElement("nameEus")]
        public String nameEus { set; get; }
        [BsonElement("municipality")]
        public String municipality { set; get; }
        [BsonElement("province")]
        public String province { set; get; }
        [BsonElement("altitude")]
        public double altitude { set; get; }
        [BsonElement("x")]
        public double x { set; get; }
        [BsonElement("y")]
        public double y { set; get; }
        [BsonElement("stationType")]
        public String stationType { set; get; }
        public void save()
        {
            Database database = new Database();
            database.Insert(this);
        }
    }
    
}
