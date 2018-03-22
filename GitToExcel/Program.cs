using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using GitToExcel.Repositories;
using GitToExcel.Report;

namespace GitToExcel
{
    class Program
    {
        
        
        public static void Main(string[] args)
        {
            
            Console.WriteLine("Enter User Name: ");
            var userName = Console.ReadLine();
            
            Console.WriteLine("Enter Repo Owner:");
            var groupName = Console.ReadLine();
            
            
            Console.WriteLine("Enter Repo Name:");
            var repoName = Console.ReadLine();
            
            Console.WriteLine("Enter MileStone Name(optional):");
            var milestoneName = Console.ReadLine();
            
            
            Console.WriteLine("Enter Password:");
            var password = ReadPassword();


            if (string.IsNullOrWhiteSpace(groupName) == false && string.IsNullOrWhiteSpace(repoName) == false &&
                string.IsNullOrWhiteSpace(userName) == false && string.IsNullOrWhiteSpace(password) == false)
            {

                try
                {




                var repo = new RetrieveRepo(groupName, repoName,
                    password, userName);
                var repoIssue = new IssuesRepo(password, userName, repo.SelectedRepository.Id, groupName);

                repoIssue.GetIsssues();
       


                ReportGenerator report = new ReportGenerator(userName, password);



                report.GenerateIssueReport(repoIssue.Issues, repo.SelectedRepository,
                    string.IsNullOrWhiteSpace(milestoneName) ? null : milestoneName, repoIssue);
                Console.WriteLine("Report Generated");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                Console.WriteLine("Improper information entered");
            }


        }
        
        
        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }

            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }
        
    }
    
    
}
