using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace GitToExcel.Repositories
{
    public class RetriveMileStones : GitToExcelClient
    {
        public IReadOnlyList<Milestone> MileStones { get; set; }

        public RetriveMileStones(long RepoId, string password, string username) : base(password, username)
        {
            Task.Run(async () => await SetMileStones(RepoId)).Wait();
        }

        public async Task SetMileStones(long repoId)
        {
            var client = this.GetClient();
            MileStones = await client.Issue.Milestone.GetAllForRepository(repoId);
        }

    }
}