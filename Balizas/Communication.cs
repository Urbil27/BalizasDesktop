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
    class Communication
    {
        public void GetBalizas()
        {
            var url = $"http://localhost:8080/items";
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
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            Console.WriteLine(responseBody);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle error
            }
        }

    }
}
