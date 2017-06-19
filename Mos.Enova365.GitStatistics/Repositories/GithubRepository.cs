using Mos.Enova365.GitStatistics.Models;
using Mos.Enova365.GitStatistics.Models.Github;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace Mos.Enova365.GitStatistics.Repositories
{
    public class GithubRepository : IGitRepository
    {
        private const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 OPR/45.0.2552.898";
        private const string githubApi = "https://api.github.com/";
        private const string githubUser = "bocium";
        private const string repositoryName = "EnovaTestRepo";

        private IEnumerable<CommitResponse> commitResponses;

        public GithubRepository()
        {
            commitResponses = GetGithubCommits();
        }

        public IEnumerable<DailyCommitStatistic> GetDailyCommitStatistics()
        {
            List<DailyCommitStatistic> dailyCommitStatistics = new List<DailyCommitStatistic>();

            foreach (CommitResponse commit in commitResponses)
            {
                string commiter = commit.Commit.Committer.Name;
                DateTime commitDay = commit.Commit.Committer.Date.Date;

                DailyCommitStatistic dailyCommitStat = dailyCommitStatistics.FirstOrDefault(c => c.Committer == commiter && c.CommitDate == commitDay);

                if (dailyCommitStat == null)
                {
                    dailyCommitStat = new DailyCommitStatistic
                    {
                        Committer = commiter,
                        CommitDate = commitDay,
                        CommitsCount = 1
                    };

                    dailyCommitStatistics.Add(dailyCommitStat);
                }
                else
                {
                    ++dailyCommitStat.CommitsCount;
                }
            }

            return dailyCommitStatistics;
        }

        public IEnumerable<AverageCommitStatistic> GetAverageCommitStatistics()
        {
            List<AverageCommitStatistic> averageCommitStatistics = new List<AverageCommitStatistic>();
            IEnumerable<DailyCommitStatistic> dailyCommitsPerUser = GetDailyCommitStatistics();

            IEnumerable<IGrouping<string, int>> groupedCommitsCountsPerUser = dailyCommitsPerUser.GroupBy(d => d.Committer, d => d.CommitsCount);

            foreach (IGrouping<string, int> userCommits in groupedCommitsCountsPerUser)
            {
                AverageCommitStatistic averageUserCommits = new AverageCommitStatistic
                {
                    Committer = userCommits.Key
                };

                foreach (int count in userCommits)
                {
                    averageUserCommits.CommitsCount += count;
                }

                averageCommitStatistics.Add(averageUserCommits);
            }

            return averageCommitStatistics;
        }

        private IEnumerable<CommitResponse> GetGithubCommits()
        {
            Branch[] branches = GetBranches();
            List<CommitResponse> commits = GetCommitsFromBranches(branches);

            IEnumerable<CommitResponse> distinctCommits = commits.GroupBy(c => c.Sha).Select(group => group.First());
            IOrderedEnumerable<CommitResponse> orderedCommits = distinctCommits.OrderByDescending(c => c.Commit.Committer.Date);

            return orderedCommits;
        }

        private static List<CommitResponse> GetCommitsFromBranches(Branch[] branches)
        {
            List<CommitResponse> commits = new List<CommitResponse>();

            foreach (Branch branch in branches)
            {
                string commitsRequestUri = string.Format("{0}repos/{1}/{2}/commits?sha={3}", githubApi, githubUser, repositoryName, branch.Name);
                var commitsRequest = (HttpWebRequest)WebRequest.Create(commitsRequestUri);
                commitsRequest.UserAgent = userAgent;

                using (var webResponse = (HttpWebResponse)commitsRequest.GetResponse())
                {
                    using (Stream responseStream = webResponse.GetResponseStream())
                    {
                        using (var sr = new StreamReader(responseStream))
                        {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            var commitsResponse = sr.ReadToEnd();
                            CommitResponse[] currentBranchCommits = (CommitResponse[])js.Deserialize(commitsResponse, typeof(CommitResponse[]));

                            commits.AddRange(currentBranchCommits);
                        }
                    }
                }
            }

            return commits;
        }

        private Branch[] GetBranches()
        {
            Branch[] branches;
            string branchesRequestUri = string.Format("{0}repos/{1}/{2}/branches", githubApi, githubUser, repositoryName);
            var branchesRequest = (HttpWebRequest)WebRequest.Create(branchesRequestUri);
            branchesRequest.UserAgent = userAgent;

            using (var webResponse = (HttpWebResponse)branchesRequest.GetResponse())
            {
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var branchesResponse = sr.ReadToEnd();
                        branches = (Branch[])js.Deserialize(branchesResponse, typeof(Branch[]));
                    }
                }
            }

            return branches;
        }
    }
}
