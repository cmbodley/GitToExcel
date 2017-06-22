using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
            var repo = new RetrieveRepo("EssarSteelAlgoma", "VendorManagementSystem", password, userName);
            var repoMilestones = new RetriveMileStones(repo.SelectedRepository.Id, password, userName);
            var repoIssues = new RetriveIssues(repo.SelectedRepository.Id, password, userName);
            var mileStoneIssues = repoIssues.Issues.Where(o => o.Milestone != null).ToList();
            var ReportGenerator = new ReportGenerator(userName, password);
            ReportGenerator.TaskListGenerateReport(mileStoneIssues,repo.SelectedRepository);
        }
    }
}
