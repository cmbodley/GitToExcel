using Octokit;
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
    public class GitToExcelClient
    {
        protected string Password { get; set; }
        protected string UserName { get; set; }
        
        HttpClient httpclient = new HttpClient();
        public HttpResponseMessage Response { get; set; }

        public GitToExcelClient(string Password, string Username)
        {
            this.Password = Password;
            this.UserName = Username;
        }

        public GitHubClient GetClient()
        {
            var client = new GitHubClient(new  Octokit.ProductHeaderValue("GitToExcel"));
            var basicAuth = new Credentials(this.UserName, this.Password); // NOTE: not real credentials
            client.Credentials = basicAuth;
            return client;
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
            //var test = JsonConvert.DeserializeObject<Class1[]>(result);
            return result;
        }


        public async Task MakeGetCall(string url)
        {
            Response = await httpclient.GetAsync(url);
        }

    }
}