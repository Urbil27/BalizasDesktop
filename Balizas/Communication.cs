using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Web;
using System.Net;
using System.IO;
namespace Balizas
{

    using Newtonsoft.Json;
    using System.Text.Json.Serialization;
    using Models;
    using System.Diagnostics;
    using Newtonsoft.Json.Linq;
    using Balizas.etc;

    class Communication
    {
        public List<Baliza> GetBalizas()
        {
            var url = $"https://www.euskalmet.euskadi.eus/vamet/stations/stationList/stationList.json";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return new List<Baliza>();
                        using (StreamReader objReader = new StreamReader(strReader, System.Text.Encoding.UTF7))
                        {
                            string responseBody = objReader.ReadToEnd();
                            Debug.WriteLine(responseBody);

                            var list = JsonConvert.DeserializeObject<List<Baliza>>(responseBody);
                            return list;
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                // Handle error
            }
            return new List<Baliza>();
        }
        public async void GetReadings(DateTime day, Baliza baliza)
        {
            List<Reading> readings = new List<Reading>();
            String url = "https://www.euskalmet.euskadi.eus/vamet/stations/readings/" + baliza.id + "/" + day.ToString("yyyy/MM/dd") + "/readingsData.json";
            var client = new HttpClient { BaseAddress = new Uri(url) };
            var responseMessage = await client.GetAsync("", HttpCompletionOption.ResponseHeadersRead);
            var resultData = await responseMessage.Content.ReadAsStringAsync();
            dynamic readingdJson = JsonConvert.DeserializeObject(resultData);
            Database database = new Database();
            JObject fullJson = JObject.Parse(resultData);

            IList<string> mainKeys = fullJson.Properties().Select(p => p.Name).ToList();
            IDictionary<String, Reading> Readings = new Dictionary<String, Reading>();
            foreach (string key in mainKeys)
            {
                JObject mainData = JObject.Parse(fullJson[key].ToString());


                String name = mainData["name"].ToString();
                String balizaId = mainData["station"].ToString();
                String type = mainData["type"].ToString();
                JObject data = JObject.Parse(mainData["data"].ToString());
                IList<string> keys = data.Properties().Select(p => p.Name).ToList();
                
                JObject dataDef = JObject.Parse(data[keys[0]].ToString());
                IList<string> numKey = data.Properties().Select(p => p.Name).ToList();
                //No coje todos los timeKeys
                IList<string> timeKeys = dataDef.Properties().Select(p => p.Name).ToList();

                foreach (string timeKey in timeKeys)
                {
                    Reading reading ;
                    Debug.WriteLine("timekey "+timeKey);
                    if (key == "11")
                    {
                        Debug.WriteLine("Entro en 11");
                       
                        reading = new Reading();


                        TimeParser timeParser = new TimeParser(timeKey);
                        DateTime dateTime = day;
                        TimeSpan time = new TimeSpan(timeParser.hours, timeParser.minutes, 0);
                        dateTime = dateTime.Date + time;
                        reading.id = balizaId + dateTime;
                        
                        reading.BalizaID = balizaId;

                        reading.Datetime = dateTime+"";
                        reading.mean_speed = Double.Parse(dataDef[timeKey].ToString());
                        Readings.Add(timeKey, reading);

                    }
                    else if (key == "12")
                    {
                        Debug.WriteLine("Entro en 12");
                        reading = Readings[timeKey];
                        reading.mean_direction = Double.Parse(dataDef[timeKey].ToString());
                        Readings.Remove(timeKey);
                        Readings.Add(timeKey, reading);
                    }
                    else if (key == "14")
                    {
                        Debug.WriteLine("Entro en 14");
                        reading = Readings[timeKey];
                        reading.max_speed = Double.Parse(dataDef[timeKey].ToString());
                        Readings.Remove(timeKey);
                        Readings.Add(timeKey, reading);
                    }
                    else if (key == "16")
                    {
                        Debug.WriteLine("Entro en 16");
                        reading = Readings[timeKey];
                        reading.speed_sigma = Double.Parse(dataDef[timeKey].ToString());
                        Readings.Remove(timeKey);
                        Readings.Add(timeKey, reading);
                    }
                    else if (key == "17")
                    {
                        Debug.WriteLine("Entro en 17");
                        reading = Readings[timeKey];
                        reading.direction_sigma = Double.Parse(dataDef[timeKey].ToString());
                        Readings.Remove(timeKey);
                        Readings.Add(timeKey, reading);
                    }
                    else if (key == "21")
                    {
                        Debug.WriteLine("Entro en 21");
                        reading = Readings[timeKey];
                        reading.temperature = Double.Parse(dataDef[timeKey].ToString());
                        Readings.Remove(timeKey);
                        Readings.Add(timeKey, reading);
                    }
                    else if (key == "31")
                    {
                        Debug.WriteLine("Entro en 31");
                        reading = Readings[timeKey];
                        reading.humidity = Double.Parse(dataDef[timeKey].ToString());
                        Readings.Remove(timeKey);
                        Readings.Add(timeKey, reading);
                    }
                    else if (key == "40")
                    {
                        Debug.WriteLine("Entro en 40");
                        reading = Readings[timeKey];
                        reading.precipitation = Double.Parse(dataDef[timeKey].ToString());
                        Readings.Remove(timeKey);
                        Readings.Add(timeKey, reading);
                    }
                    else if (key == "70")
                    {
                        Debug.WriteLine("Entro en 70");
                        reading = Readings[timeKey];
                        reading.irradiance = Double.Parse(dataDef[timeKey].ToString());
                        Readings.Remove(timeKey);
                        Readings.Add(timeKey, reading);

                        

                    }
                    
                }
                
            }
            foreach (KeyValuePair<string, Reading> entry in Readings)
            {
                // do something with entry.Value or entry.Key
                //Debug.WriteLine(entry.Value.id);
                database.Insert(entry.Value);
            }
        }
    }
}

