using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ITTT_Final
{
    [XmlInclude(typeof(ITTTConditionPicture)), XmlInclude(typeof(ITTTConditionWeather))]
    [Serializable]
    public abstract class ITTTCondition
    {
        public string Url { get; set; }
        public string Text { get; set; }

        public abstract bool CheckCondition(string a, string b);
    }
    [Serializable]
    public class ITTTConditionPicture: ITTTCondition
    {
        public override bool CheckCondition(string html, string fileName)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes((".//img/@alt")))
            {
                string alt = link.Attributes["alt"].Value;
                if (alt.Contains(Text))                  // jak alt zawiera tekst
                {
                    string src = link.Attributes["src"].Value;
                    if (src.Contains("http"))
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(src, fileName); // zapis obrazka do pliku
                            return true;
                        }
                    }
                }
            }
            return false;                       // jak nie znaleziono obrazka
        }
        public override string ToString()
        {
            return string.Format("strona: {0}, słowo klucz: {1}, ", Url, Text);
        }
    }
    [Serializable]
    public class ITTTConditionWeather : ITTTCondition
    {
        private WeatherObject weather;
        public override bool CheckCondition(string html, string fileName)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    string json = wc.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=" + Url + ",pl");
                    weather = JsonConvert.DeserializeObject<WeatherObject>(json);

                    using (WebClient wc2 = new WebClient())
                    {
                        wc2.DownloadFile("http://openweathermap.org/img/w/" + weather.Weather[0].icon + ".png", fileName);
                    }
                }
                catch
                {
                    Logs.Error("Weather Server not responding.");
                    return false;
                }
            }
            if (weather.Main.temp > Convert.ToInt32(Text)) return true;
            else return false;
        }
        public override string ToString()
        {
            return string.Format("miasto: {0}, temperatura: {1}, ", Url, Text);
        }
    }
}
