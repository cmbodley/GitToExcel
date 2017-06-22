using Octokit;

namespace GitToExcel.Repositories
{
    public class GitToExcelClient
    {
        protected string Password { get; set; }
        protected string UserName { get; set; }

        public GitToExcelClient(string Password, string Username)
        {
            this.Password = Password;
            this.UserName = Username;
        }

        public GitHubClient GetClient()
        {
            var client = new GitHubClient(new ProductHeaderValue("GitToExcel"));
            var basicAuth = new Credentials(this.UserName, this.Password); // NOTE: not real credentials
            client.Credentials = basicAuth;
            return client;
        }

    }
}