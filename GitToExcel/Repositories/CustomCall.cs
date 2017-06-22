using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GitToExcel.ViewModels;
using Newtonsoft.Json;

namespace GitToExcel.Repositories
{
    public class CustomCall : GitToExcelClient
    {
        HttpClient client = new HttpClient();
        public HttpResponseMessage Response { get; set; }
        public CustomCall(string password, string username) : base(password, username)
        {
            //client.BaseAddress = new Uri("https://api.github.com");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }


        public string MakeGetReqest(string url)
        {
            string baseUrl = "https://api.github.com";
            var combinedUrl = baseUrl + url;
            HttpWebRequest req = WebRequest.Create(new Uri(combinedUrl)) as HttpWebRequest;
            req.UserAgent = "GitToExcel";
            req.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(this.UserName + ":" + this.Password)));
            string result = null;
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader =
                    new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }
            var test = JsonConvert.DeserializeObject<Class1[]>(result);
            return result;
        }


        public async Task MakeGetCall(string url)
        {
            Response = await client.GetAsync(url);
        }

    }
}
