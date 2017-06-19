using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mos.Enova365.GitStatistics.Models;
using Mos.Enova365.GitStatistics.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Mos.Enova365.GitStatistics.Tests
{
    [TestClass]
    public class GithubCommitStatisticsTests
    {
        [TestMethod]
        public void GetGithubCommitsTest()
        {
            int expected = 1;
            IGitRepository gitRepository = new GithubRepository();

            IEnumerable<DailyCommitStatistic> commits = gitRepository.GetDailyCommitStatistics();

            int actual = commits.Count();
            Assert.AreEqual(expected, actual);
        }
    }
}
