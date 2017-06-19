#define MAIN_WINDOW
#define GIT_STATISTICS

using Soneta.Business.Licence;
using Soneta.Business.UI;
using Mos.Enova365.GitStatistics.Extender;

#if MAIN_WINDOW

[assembly: FolderView("Git Statistics",
    Priority = 1,
    Description = "Git Statistics",
    BrickColor = FolderViewAttribute.BlueBrick,
    Icon = "Mos.Enova365.GitStatistics.Utils.git_logo.ico;Mos.Enova365.GitStatistics",
    Contexts = new object[] { LicencjeModułu.All }
)]

#endif

#if GIT_STATISTICS

[assembly: FolderView("Git Statistics/Commit Statistics",
    Priority = 2,
    Description = "Commit Statistics",
    ObjectType = typeof(CommitStatistics),
    ObjectPage = "CommitStatistics.Ogolne.pageform.xml",
    ReadOnlySession = false,
    ConfigSession = false
)]

#endif