using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GitToExcel.Repositories;
using GitToExcel.Report;

namespace GitToExcel
{
    class Program
    {
        public static void Main(string[] args)
        {
            var userName = args[0];
            var password = args[1];
            var Owner = args[2];
            var Repo = args[3];
            var repo = new RetrieveRepo(Owner, Repo, password, userName);
            var repo_milestone = new MileStonesRepo(password, userName);
            var repo_issue = new IssuesRepo(password, userName);
            Task.Run(async () => await repo_milestone.CreateMileStone(repo.SelectedRepository.Id, "Test Repo", "Yes we have done it")).Wait();

            Task.Run(async () => await repo_issue.CreateIssue(repo.SelectedRepository.Id,repo_milestone.CreatedMilestone.Number ,"Test issue", "it should have a milestone")).Wait();
        }
    }
}
