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
                            // Do something with responseBody
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
            String url = "https://www.euskalmet.euskadi.eus/vamet/stations/readings/"+baliza.id+"/"+day.ToString("yyyy/MM/dd")+"/readingsData.json";
            Debug.WriteLine(url);
            var client = new HttpClient { BaseAddress = new Uri(url) };
            var responseMessage = await client.GetAsync("", HttpCompletionOption.ResponseHeadersRead);
            var resultData = await responseMessage.Content.ReadAsStringAsync();
            dynamic readingdJson = JsonConvert.DeserializeObject(resultData);
            foreach(var obj in readingdJson)
            {
                foreach (JObject stationTypeJson in obj)
                {
                    String name = stationTypeJson["name"].ToString();
                    String balizaId = stationTypeJson["station"].ToString();
                    String type = stationTypeJson["type"].ToString();
                    JObject data = JObject.Parse(stationTypeJson["data"].ToString());
                    IList<string> keys = data.Properties().Select(p => p.Name).ToList();
                    Debug.WriteLine(keys[0]);
                    JObject dataDef = JObject.Parse(data[keys[0]].ToString());
                    IList<string> numKey = data.Properties().Select(p => p.Name).ToList();
                    IList<string> timeKey = dataDef.Properties().Select(p => p.Name).ToList();
                    foreach (string key in timeKey)
                    {
                        Debug.WriteLine(key);
                        Double value = Double.Parse(dataDef[key].ToString());
                        Debug.WriteLine("key " + key + " value " + value);
                        TimeParser timeParser = new TimeParser(key);
                        DateTime dateTime = day;
                        TimeSpan time = new TimeSpan(timeParser.hours, timeParser.minutes,0);
                        dateTime = dateTime.Date + time;
                        Debug.WriteLine(dateTime.ToString());
                        Reading reading = new Reading(balizaId, name, type,dateTime, value);
                        reading.save();
                    }


                }

            }
        }
    }
}
