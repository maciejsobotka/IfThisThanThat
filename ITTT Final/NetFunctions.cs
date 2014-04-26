using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using HtmlAgilityPack;


namespace ITTT_Final
{
    class NetFunctions
    {
        public bool IsUrl(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);  
            WebResponse webResponse;
            try 
            {
                webResponse = webRequest.GetResponse();
            }
            catch       //If exception thrown then couldn't get response from address
            {
                return false;
            }
            return true;
        }
        public string GetPageHtml(string url)
        {
            using (WebClient wc = new WebClient())
            {
                // Pobieramy zawartość strony spod adresu url jako ciąg bajtów
                byte[] data = wc.DownloadData(url);
                // Przekształcamy ciąg bajtów na string przy użyciu kodowania UTF-8.
                // To oczywiście powinno zależeć od właściwego kodowania
                string html = Encoding.UTF8.GetString(data);

                // Wersja uproszczona bez uwzględniania kodowania
                // string html = wc.DownloadString(url);

                return html;
            }
        }
    }
}
