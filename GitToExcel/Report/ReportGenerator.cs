using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using GitToExcel.Repositories;
using Octokit;

namespace GitToExcel.Report
{
    public class ReportGenerator
    {
        private string UserName { get; set; }
        private string Password { get; set; }

        public ReportGenerator(string user, string pass)
        {
            this.UserName = user;
            this.Password = pass;
        }

        public void GenerateIssueReport(IReadOnlyList<Issue> issues, Repository repo)
        {
            XLWorkbook workbook = new XLWorkbook(".\\ReportTemplates\\BlankIssues.xlsx");
            var WorkSheet = workbook.Worksheets.First();

            int row = 2;
            int col = 1;



            foreach (var item in issues)
            {
                WorkSheet.Cell(row, col).Value = item.Number;
                col++;
                WorkSheet.Cell(row, col).Value = repo.Name;
                col++;
                WorkSheet.Cell(row, col).Value = item.Title;
                col++;
                WorkSheet.Cell(row, col).Value = item.Body;
                col++;
                WorkSheet.Cell(row, col).Value = string.Join(", ", item.Assignees.Select(o => o.Login));
                col++;
                WorkSheet.Cell(row, col).Value = item.State;
                col++;
                WorkSheet.Cell(row, col).Value = "Link To Git Issue";
                WorkSheet.Cell(row, col).Hyperlink = new XLHyperlink(item.HtmlUrl);
                col++;
                WorkSheet.Cell(row, col).Value = string.Join(",", item.Labels.Select(o => o.Name));
                col = 1;
                row++;
            }
            var Today = DateTime.Now.ToString("MMM-dd-yyyy");
            workbook.SaveAs(string.Format("Issue-Tracker-{0}.xlsx", Today));

        }

        public void TaskListGenerateReport(IReadOnlyList<Issue> issues, Repository repo)
        {
            XLWorkbook workbook = new XLWorkbook(".\\ReportTemplates\\TaskList.xlsx");
            var WorkSheet = workbook.Worksheets.First();

            int row = 2;
            int col = 1;

            //we need to group by mile stones
            var groupedIssues = issues.GroupBy(k => k.Milestone.Title, p => p, (key, o) => new {Milestone = key, Issues = o.ToList()})
                .ToList();

            //var newClient = new CustomCall(this.Password, this.UserName);

            foreach (var item in groupedIssues)
            {
                WorkSheet.Cell(row, col).Value = item.Milestone;
                WorkSheet.Cell(row, col).Hyperlink = new XLHyperlink(item.Issues.First().Milestone.HtmlUrl);
                WorkSheet.Cell(row, "G").Value = ((double)item.Issues.First().Milestone.ClosedIssues / (double) item.Issues.Count) * 100;
                col++;
                row++;
                foreach (var issue in item.Issues)
                {
                    WorkSheet.Cell(row, col).Value = issue.Title;
                    WorkSheet.Cell(row, col).Hyperlink = new XLHyperlink(issue.HtmlUrl);
                    col++;
                    col++;
                    col++;
                    WorkSheet.Cell(row, col).Value = issue.CreatedAt.DateTime;
                    col++;

                    if(issue.ClosedAt.HasValue)
                        WorkSheet.Cell(row, col).Value = issue.ClosedAt.Value.DateTime;

                    col++;
                    col++;
                    WorkSheet.Cell(row, col).Value = issue.State;
                    col++;
                    col++;
                    WorkSheet.Cell(row, col).Value = string.Join(", ", issue.Assignees.Select(o => o.Login));
                    col = 2;
                    row++;
                }

                col = 1;
            }


            var Today = DateTime.Now.ToString("MMM-dd-yyyy");
            workbook.SaveAs(string.Format("{0}-TaskList-{1}.xlsx", repo.Name,Today));

        }
    }
}
