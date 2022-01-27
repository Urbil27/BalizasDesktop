using System;
using System.Collections.Generic;
using System.Text;

namespace Balizas.Models
{
    internal class Reading
    {
        string BalizaID { get; set; }
        int Year { get; set; }
        int Month { get; set; }
        public int Day { get; set; }
        int Hour { get; set; }
        int Minute { get; set; }
        
        public string Type { get; set; }
        public string Name { get; set; }
        public double  Value { get; set; }
        
        public Reading(string balizaID,string name,string type, DateTime dateTime, Double value)
        {
            this.BalizaID = balizaID;
            this.Name = name;
            this.Type = type;
            this.Year = dateTime.Year; 
            this.Month = dateTime.Month;
            this.Day = dateTime.Day;
            this.Hour = dateTime.Hour;
            this.Minute = dateTime.Minute;
            this.Value = value;
        }
        public void save()
        {

        }
    }
}
