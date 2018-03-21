using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;

namespace GitToExcel.Repositories
{
    public class IssuesRepo : GitToExcelClient
    {
        public IReadOnlyList<Issue> Issues { get; set; }
        public IReadOnlyList<Project> Projects { get; set; }
        public IReadOnlyList<ProjectColumn> ProjectColumns { get; set; }
        public IReadOnlyCollection<ProjectCard> ProjectCards { get; set; }
        
        public Issue CreatedIssue { get; set; }

        public IssuesRepo(string password, string username) :base(password,username)
        {

        }

        public void GetIsssues(long RepoId)
        {
            Task.Run(async () => await SetIssues(RepoId)).Wait();
        }

        public void GetProjects(long RepoId)
        {
            Task.Run(async () => await SetProjects(RepoId)).Wait();
        }

        public void GetProjectCards(int projectId)
        {
            Task.Run(async () => await SetProjecCards(projectId)).Wait();
        }
        
        public void GetProjectColumns(int projectId)
        {
            Task.Run(async () => await SetProjectCollumns(projectId)).Wait();
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

        public async Task SetProjects(long RepoId)
        {
            var client = this.GetClient();

            Projects = await client.Repository.Project.GetAllForRepository(RepoId);
        }
        
        

        public async Task SetProjecCards(int projectId)
        {
            var client = this.GetClient();
            ProjectCards = await client.Repository.Project.Card.GetAll(projectId);
        }

        public async Task SetProjectCollumns(int projectId)
        {
            var client = this.GetClient();
            ProjectColumns = await client.Repository.Project.Column.GetAll(projectId);
        }

        public async Task CreateIssue(long RepoId, int MileStoneNumber, string title, string description = null)
        {
            var client = this.GetClient();
            NewIssue issue = new NewIssue(title);
            issue.Body = description == null ? "Automaticall Generated" : description;
            issue.Milestone = MileStoneNumber;
            CreatedIssue = await client.Issue.Create(RepoId, issue);

        }
    }
}