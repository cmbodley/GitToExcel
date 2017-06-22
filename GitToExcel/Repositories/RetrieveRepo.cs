using System.Threading.Tasks;
using Octokit;

namespace GitToExcel.Repositories
{
    public class RetrieveRepo : GitToExcelClient
    {
        public Repository SelectedRepository { get; set; }

        public RetrieveRepo(string owner, string name, string password, string username) : base(password, username)
        {
            Task.Run(async () => await SetRepo(name, owner)).Wait();
        }

        public async Task SetRepo(string name, string owner)
        {
            var client = this.GetClient();
            SelectedRepository = await client.Repository.Get(owner, name);
        }
    }
}