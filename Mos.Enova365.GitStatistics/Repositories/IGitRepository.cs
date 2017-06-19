using Mos.Enova365.GitStatistics.Models;
using System.Collections.Generic;

namespace Mos.Enova365.GitStatistics.Repositories
{
    public interface IGitRepository
    {
        IEnumerable<DailyCommitStatistic> GetDailyCommitStatistics();

        IEnumerable<AverageCommitStatistic> GetAverageCommitStatistics();
    }
}
