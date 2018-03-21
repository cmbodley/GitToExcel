using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
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

        public void GenerateIssueReport(IReadOnlyList<Issue> issues, Repository repo, string mileStoneName = null, IReadOnlyList<Project> projects = null)
        {
            XLWorkbook workbook = new XLWorkbook("./ReportTemplates/BlankIssues.xlsx");
            var openSheet = workbook.Worksheets.First(o => o.Name == "Open");

            int row = 2;
            int col = 1;
            

            var targeted = issues.Where(o => (mileStoneName != null &&  o.Milestone.Title == mileStoneName) || mileStoneName == null)
                .GroupBy(x => x.State , (s,i) => new {state = s, issue = i})
                .ToList();


            
            foreach (var item in targeted.Where(o => o.state == ItemState.Open).SelectMany(o => o.issue))
            {
                openSheet.Cell(row, col).Value = item.Number;
                col++;
                openSheet.Cell(row, col).Value = repo.Name;
                col++;
                openSheet.Cell(row, col).Value = item.Title;
                col++;
                openSheet.Cell(row, col).Value = item.Body;
                col++;
                openSheet.Cell(row, col).Value = string.Join(", ", item.Assignees.Select(o => o.Login));
                col++;
                openSheet.Cell(row, col).Value = item.State;
                col++;
                openSheet.Cell(row, col).Value = "Link To Git Issue";
                openSheet.Cell(row, col).Hyperlink = new XLHyperlink(item.HtmlUrl);
                col++;
                openSheet.Cell(row, col).Value = string.Join(",", item.Labels.Select(o => o.Name));
                col = 1;
                row++;
            }

            var closedSheet = workbook.Worksheets.First(o => o.Name == "Closed");
            row = 2;
            foreach (var item in targeted.Where(o => o.state == ItemState.Closed).SelectMany(o => o.issue))
            {
                closedSheet.Cell(row, col).Value = item.Number;
                col++;
                closedSheet.Cell(row, col).Value = repo.Name;
                col++;
                closedSheet.Cell(row, col).Value = item.Title;
                col++;
                closedSheet.Cell(row, col).Value = item.Body;
                col++;
                closedSheet.Cell(row, col).Value = string.Join(", ", item.Assignees.Select(o => o.Login));
                col++;
                closedSheet.Cell(row, col).Value = item.State;
                col++;
                closedSheet.Cell(row, col).Value = "Link To Git Issue";
                closedSheet.Cell(row, col).Hyperlink = new XLHyperlink(item.HtmlUrl);
                col++;
                closedSheet.Cell(row, col).Value = string.Join(",", item.Labels.Select(o => o.Name));
                col = 1;
                row++;
            }
            
            
           
                  
            var Today = DateTime.Now.ToString("MMM-dd-yyyy");
            var fileName = $"Issue-Tracker-{Today.ToString()}.xlsx";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }


            
            workbook.SaveAs(fileName);

        }

    }
}
