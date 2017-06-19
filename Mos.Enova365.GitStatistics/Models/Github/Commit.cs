namespace Mos.Enova365.GitStatistics.Models.Github
{
    public class Commit
    {
        public Committer Author { get; set; }

        public Committer Committer { get; set; }

        public string Message { get; set; }
    }
}
