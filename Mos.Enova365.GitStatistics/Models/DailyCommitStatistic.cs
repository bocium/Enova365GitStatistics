using System;

namespace Mos.Enova365.GitStatistics.Models
{
    public class DailyCommitStatistic
    {
        public DailyCommitStatistic()
        {
            CommitsCount = 0;
        }

        public string Committer { get; set; }

        public DateTime CommitDate { get; set; }

        public int CommitsCount { get; set; }
    }
}
