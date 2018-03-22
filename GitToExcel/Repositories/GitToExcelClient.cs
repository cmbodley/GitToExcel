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
        protected string HttpResponse { get; set; }
        protected string Orgs { get; set; }
        

        public GitToExcelClient(string Password, string Username, string orgs = null)
        {
            this.Password = Password;
            this.UserName = Username;
            this.Orgs = orgs;
        }

        public GitHubClient GetClient()
        {
            var client = new GitHubClient(new  Octokit.ProductHeaderValue("GitToExcel"));
            var basicAuth = new Credentials(this.UserName, this.Password); // NOTE: not real credentials
            client.Credentials = basicAuth;
            return client;
        }
        
    

        public  async Task NewCustomCal(string url)
        {
            string baseUrl = $"https://api.github.com/orgs/{Orgs}{url}";
            var auth = Convert.ToBase64String(new ASCIIEncoding().GetBytes(this.UserName + ":" + this.Password));
            
            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "cmbodley-GitToExcel");
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            HttpResponse = await client.GetStringAsync(baseUrl);
            
            

            Console.WriteLine(HttpResponse);

        }

        public async Task ProcessRepositories()
        {      HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");

            var msg = await stringTask;
            Console.Write(msg);
        }
   

    }
}