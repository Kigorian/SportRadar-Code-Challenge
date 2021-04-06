using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace NHL_API
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://statsapi.web.nhl.com/api/v1/teams";
            //string url = $"{ConfigurationManager.AppSettings["NHL-API"]}/teams";
            string json = GetJsonResponse(url);

            var jObject = JObject.Parse(json);
            var teams = jObject["teams"];

            Console.WriteLine("ID: " + teams[0]["id"]);
            Console.WriteLine("Name: " + teams[0]["name"]);
            Console.WriteLine("Link: " + teams[0]["link"]);
            Console.WriteLine("Venue Name: " + teams[0]["venue"]["name"]);
        }

        /// <summary>
        /// Calls the API for the given url, and returns a string containing the JSON response.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetJsonResponse(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = responseStream.ReadToEnd();
            webresponse.Close();

            return result;
        }
    }
}
