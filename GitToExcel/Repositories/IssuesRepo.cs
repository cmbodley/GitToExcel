using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2013.Excel;
using Octokit;

namespace GitToExcel.Repositories
{
    public class IssuesRepo : GitToExcelClient
    {
        public IReadOnlyList<Issue> Issues { get; set; }
        public IReadOnlyList<EventInfo> IssueEvents { get; set; }
        public IReadOnlyList<Project> Projects { get; set; }
        public IReadOnlyList<ProjectColumn> ProjectColumns { get; set; }
        public IReadOnlyList<ProjectCard> ProjectCards { get; set; }
        public string Cards { get; set; }
        
        public long RepoId { get; set; }
        
        
        
        public Issue CreatedIssue { get; set; }

        public IssuesRepo(string password, string username, long repoId, string orgs = null) :base(password,username, orgs)
        {
            this.RepoId = repoId;
        }

        public void GetIsssues()
        {
            Task.Run(async () => await SetIssues()).Wait();
        }

        public void GetIssueEvents(int id)
        {
            Task.Run(async () => await SetIssueEvents(id)).Wait();
        }

        public void GetProjects()
        {
            Task.Run(async () => await SetProjects()).Wait();
        }

        public void GetProjectColumns(int projectId)
        {
            Task.Run(async () => await SetProjectColumns(projectId)).Wait();
        }

        public void GetProjectColumnCards(int columnId)
        {
            Task.Run(async () => await SetProjectCards(columnId));
        }

        public void GetCardsManually(int columnId)
        {
            var url = $"/projects/columns/{columnId}/cards";

            Task.Run(async () => await this.NewCustomCal(url));
 
            //Task.Run(async () => await this.ProcessRepositories());
        }

        

        

        public async Task SetIssues()
        {
            var client = this.GetClient();
            var filter = new RepositoryIssueRequest()
            {
                State = ItemStateFilter.All
            };
            
            Issues = await client.Issue.GetAllForRepository(RepoId, filter);
        }



        public async Task SetIssueEvents(int issueID)
        {
            var client = this.GetClient();
            IssueEvents = await client.Issue.Events.GetAllForIssue(RepoId,issueID);
        }

        public async Task SetProjects()
        {
            var client = this.GetClient();
            Projects = await client.Repository.Project.GetAllForRepository(RepoId);
        }

        public async Task SetProjectColumns(int projectId)
        {
            var client = this.GetClient();
            ProjectColumns = await client.Repository.Project.Column.GetAll(projectId);
        }

        public async Task SetProjectCards(int columnId)
        {
            var client = this.GetClient();
            ProjectCards = await client.Repository.Project.Card.GetAll(columnId);
        }




        public async Task CreateIssue(int MileStoneNumber, string title, string description = null)
        {
            var client = this.GetClient();
            NewIssue issue = new NewIssue(title);
            issue.Body = description == null ? "Automaticall Generated" : description;
            issue.Milestone = MileStoneNumber;
            CreatedIssue = await client.Issue.Create(RepoId, issue);

        }
    }
}