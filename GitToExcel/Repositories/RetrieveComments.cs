using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace GitToExcel.Repositories
{
    class RetrieveComments : GitToExcelClient
    {
        public IReadOnlyList<IssueComment> Comments { get; set; }

        public RetrieveComments(long RepoId,int number ,string password, string username) : base(password, username)
        {
            Task.Run(async () => await GetComments(RepoId, number)).Wait();
        }

        public async Task GetComments(long RepoId, int number)
        {
            var client = this.GetClient();
            Comments = await client.Issue.Comment.GetAllForIssue(RepoId,number);

        }

    }
}
