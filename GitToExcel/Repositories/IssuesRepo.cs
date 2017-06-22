using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace GitToExcel.Repositories
{
    public class IssuesRepo : GitToExcelClient
    {
        public IReadOnlyList<Issue> Issues { get; set; }
        public Issue CreatedIssue { get; set; }

        public IssuesRepo(string password, string username) :base(password,username)
        {

        }

        public void GetIsssues(long RepoId)
        {
            Task.Run(async () => await SetIssues(RepoId)).Wait();
        }

        public async Task SetIssues(long RepoId)
        {
            var client = this.GetClient();
            var filter = new RepositoryIssueRequest()
            {
                State = ItemStateFilter.All
            };

            Issues = await client.Issue.GetAllForRepository(RepoId, filter);
        }

        public async Task CreateIssue(long RepoId, int MileStoneNumber, string title, string description = null)
        {
            var client = this.GetClient();
            NewIssue issue = new NewIssue(title);
            issue.Body = description == null ? "Automaticall Generated" : description;
            issue.Milestone = MileStoneNumber;
            CreatedIssue = await client.Issue.Create(RepoId, issue);

        }
    }
}