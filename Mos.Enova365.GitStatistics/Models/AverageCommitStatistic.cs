namespace Mos.Enova365.GitStatistics.Models
{
    public class AverageCommitStatistic
    {
        public AverageCommitStatistic()
        {
            CommitsCount = 0;
        }

        public string Committer { get; set; }

        public int CommitsCount { get; set; }
    }
}
