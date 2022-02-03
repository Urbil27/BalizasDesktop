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
    using System.Threading.Tasks;

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
                            List<Baliza> definitiveList = new List<Baliza>();
                            foreach (Baliza item in list)
                            {
                                if (item.stationType.Equals("METEOROLOGICAL"))
                                {
                                    definitiveList.Add(item);
                                }
                            }
                            return definitiveList;
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
        public void getAllReadings(List<Baliza> balizas)
        {
            Database database = new Database();
            DateTime today = DateTime.Now;
            database.deleteAllReadings();
            foreach (Baliza baliza in balizas)
            {
                this.GetReadings(today, baliza.id);
            }
        }
        public async void GetReadings(DateTime day, String balizaID)
        {

            {
                List<Reading> readings = new List<Reading>();
                String url = "https://www.euskalmet.euskadi.eus/vamet/stations/readings/" + balizaID + "/" + day.ToString("yyyy/MM/dd") + "/readingsData.json";
                var client = new HttpClient { BaseAddress = new Uri(url) };
                var responseMessage = await client.GetAsync("", HttpCompletionOption.ResponseHeadersRead);
                var resultData = await responseMessage.Content.ReadAsStringAsync();
                try
                {
                    dynamic readingdJson = JsonConvert.DeserializeObject(resultData);
                    Database database = new Database();

                    JObject fullJson = JObject.Parse(resultData);

                    IList<string> mainKeys = fullJson.Properties().Select(p => p.Name).ToList();


                    Reading reading = new Reading();
                    foreach (string mainkey in mainKeys)
                    {
                        JObject mainData = JObject.Parse(fullJson[mainkey].ToString());


                        String name = mainData["name"].ToString();
                        String balizaId = mainData["station"].ToString();
                        String type = mainData["type"].ToString();
                        JObject data = JObject.Parse(mainData["data"].ToString());
                        IList<string> keys = data.Properties().Select(p => p.Name).ToList();

                        JObject dataDef = JObject.Parse(data[keys[0]].ToString());
                        IList<string> numKey = data.Properties().Select(p => p.Name).ToList();

                        IList<string> timeKeys = dataDef.Properties().Select(p => p.Name).ToList();
                        IEnumerable<string> sortedEnum = timeKeys.OrderBy(f => f);
                        List<string> sortedList = sortedEnum.ToList();
                        string lastTime = sortedList[sortedList.Count() - 1];
                        Debug.WriteLine("the last time is " + lastTime);


                        Debug.WriteLine("timekey " + lastTime);
                        if (mainkey == "21")
                        {
                            Debug.WriteLine("Entro en 21");
                            TimeParser timeParser = new TimeParser(lastTime);
                            DateTime dateTime = day;
                            TimeSpan time = new TimeSpan(timeParser.hours, timeParser.minutes, 0);
                            dateTime = dateTime.Date + time;
                            reading.id = balizaId + "-" + dateTime;

                            reading.BalizaID = balizaId;

                            reading.Datetime = dateTime + "";
                            reading.temperature = Double.Parse(dataDef[lastTime].ToString());


                        }
                        else if (mainkey == "31")
                        {
                            Debug.WriteLine("Entro en 31");
                            reading.humidity = Double.Parse(dataDef[lastTime].ToString());


                        }
                        else if (mainkey == "40")
                        {
                            Debug.WriteLine("Entro en 40");
                            reading.precipitation = Double.Parse(dataDef[lastTime].ToString());

                        }
                        else if (mainkey == "70")
                        {
                            Debug.WriteLine("Entro en 70");
                            reading.irradiance = Double.Parse(dataDef[lastTime].ToString());

                        }



                    }
                    Debug.WriteLine("ctenperature: " + reading.temperature);
                    Debug.WriteLine("chumidity: " + reading.humidity);
                    Debug.WriteLine("cirradiance: " + reading.irradiance);
                    Debug.WriteLine("cprecipitation: " + reading.precipitation);
                    database.Insert(reading);
                }
                catch
                {
                    Debug.WriteLine("Error baliza");
                }
               

            }
        }
    }
}


