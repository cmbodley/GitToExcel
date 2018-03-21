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
            var repo_issue = new IssuesRepo(password, userName);

            repo_issue.GetIsssues(repo.SelectedRepository.Id);
            ReportGenerator report = new ReportGenerator(userName, password);

            report.GenerateIssueReport(repo_issue.Issues, repo.SelectedRepository, "Energy Management Issues");
            //report.TaskListGenerateReport(repo_issue.Issues, repo.SelectedRepository);


        }
    }
}
