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
                            Console.WriteLine(responseBody);
                            
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
    }
}
