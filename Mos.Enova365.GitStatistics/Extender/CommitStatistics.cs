using Mos.Enova365.GitStatistics.Models;
using Mos.Enova365.GitStatistics.Repositories;
using System.Collections.Generic;

namespace Mos.Enova365.GitStatistics.Extender
{
    public class CommitStatistics
    {
        private IGitRepository gitApi;

        public CommitStatistics()
        {
            gitApi = new GithubRepository();
        }

        public IEnumerable<DailyCommitStatistic> DailyCommitStatistics
        {
            get
            {
                return gitApi.GetDailyCommitStatistics();
            }
        }

        public IEnumerable<AverageCommitStatistic> AverageCommitStatistics
        {
            get
            {
                return gitApi.GetAverageCommitStatistics();
            }
        }
    }
}
