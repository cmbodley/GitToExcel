using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace GitToExcel.Repositories
{
    public class MileStonesRepo : GitToExcelClient
    {
        public IReadOnlyList<Milestone> MileStones { get; set; }
        public Milestone CreatedMilestone { get; set; }

        public MileStonesRepo(string password, string username) : base(password, username)
        {

        }

        public void GetMilestones(long RepoId)
        {
            Task.Run(async () => await SetMileStones(RepoId)).Wait();
        }

        public async Task SetMileStones(long repoId)
        {
            var client = this.GetClient();
            MileStones = await client.Issue.Milestone.GetAllForRepository(repoId);
        }

        public async Task CreateMileStone(long repoId, string Title, string Description = null)
        {
            var client = this.GetClient();
            NewMilestone model = new NewMilestone(Title);
            model.Description = Description == null ? "Automatically Generated Please Fill" : Description;
            CreatedMilestone = await client.Issue.Milestone.Create(repoId, model);
        }



    }


}