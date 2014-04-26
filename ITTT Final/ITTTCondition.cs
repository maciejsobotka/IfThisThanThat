using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ITTT_Final
{
    [XmlInclude(typeof(ITTTConditionPicture)), XmlInclude(typeof(ITTTConditionWeather))]
    [Serializable]
    public abstract class ITTTCondition : System.Data.Entity.DropCreateDatabaseIfModelChanges<TaskDbContext>
    {
        [Key]
        public string Url { get; set; }
        public string Text { get; set; }

        public ITTTCondition()
        {
            Conditions = new List<ITTTCondition>();
        }

        public List<ITTTCondition> Conditions { get; set; }

        public abstract bool CheckCondition(string a, ref string msg, Form1 form);

        public override string ToString()
        {
            return string.Format("strona: {0}, słowo klucz: {1}, ", Url, Text);
        }
    }
    [Serializable]
    public class ITTTConditionPicture: ITTTCondition
    {
        public override bool CheckCondition(string fileName, ref string msg, Form1 form)
        {
            NetFunctions net = new NetFunctions();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            if (net.IsUrl(Url))
            {
                string html = net.GetPageHtml(Url);
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
                                msg = "Obrazek na temat: " + Text + ".\nPobrano ze strony: " + Url + '.';
                                form.UpdateInfoBox("Pobrano obrazek");
                                Logs.Info("Pobrano obrazek");
                                return true;
                            }
                        }
                    }
                }
                form.UpdateInfoBox("Nie znaleziono obrazka z podanym tekstem");
                Logs.Error("Nie znaleziono obrazka z podanym tekstem");
            }
            else
            {
                form.UpdateInfoBox("Podany Url nie istnieje");
                Logs.Error("Podany Url nie istnieje");
            }
            return false;                       // jak nie znaleziono obrazka
        }
        /*
        public override string ToString()
        {
            return string.Format("strona: {0}, słowo klucz: {1}, ", Url, Text);
        }
        */
    }
    [Serializable]
    public class ITTTConditionWeather : ITTTCondition
    {
        private WeatherObject weather;
        public override bool CheckCondition(string fileName, ref string msg, Form1 form)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    string json = wc.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=" + Url + ",pl");
                    weather = JsonConvert.DeserializeObject<WeatherObject>(json);
                    form.UpdateInfoBox("Pobrano dane pogodowe");
                    Logs.Info("Pobrano dane pogodowe");
                    using (WebClient wc2 = new WebClient())
                    {
                        wc2.DownloadFile("http://openweathermap.org/img/w/" + weather.Weather[0].icon + ".png", fileName);
                    }
                }
                catch
                {
                    form.UpdateInfoBox("Serwer pogodowy nie odpowiada. Spróbuj ponownie");
                    Logs.Error("Serwer pogodowy nie odpowiada. Spróbuj ponownie");
                    return false;
                }
            }
            if (weather.Main.temp > Convert.ToInt32(Text))
            {
                var time = UnixTimeStampToDateTime(weather.dt);
                msg = String.Format("Miasto: {0},\nTemperatura: {1:0.0} °C,\nCiśnienie: {2} hPa,\nNiebo: "
                                + weather.Weather[0].description + ",\nOdczyt: " + time.ToLongTimeString() + '.', Url, weather.Main.temp - 273.15, weather.Main.pressure);
                return true;
            }
            else return false;
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        /*
        public override string ToString()
        {
            return string.Format("miasto: {0}, temperatura: {1}, ", Url, Text);
        }
         */
    }
}
