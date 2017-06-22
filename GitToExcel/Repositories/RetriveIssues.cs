using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace GitToExcel.Repositories
{
    public class RetriveIssues : GitToExcelClient
    {
        public IReadOnlyList<Issue> Issues { get; set; }

        public RetriveIssues(long RepoId, string password, string username) :base(password,username)
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
    }
}