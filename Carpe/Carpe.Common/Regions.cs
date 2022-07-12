namespace Carpe.Common
{
    public static class Regions
    {
        #region MainMenu
        public static string MainWindow { get { return nameof(MainWindow); } }
        public static string Documents { get { return nameof(Documents); } }
        public static string Navigation { get { return nameof(Navigation); } }
        public static string Overview { get { return nameof(Overview); } }
        public static string Evidence { get { return nameof(Evidence); } }
        public static string Analysis { get { return nameof(Analysis); } }
        public static string Recovery { get { return nameof(Recovery); } }
        public static string Visualization { get { return nameof(Visualization); } }
        public static string Report { get { return nameof(Report); } }
        #endregion

        #region AnalysisMenu
        public static string Filesystem { get { return nameof(Filesystem); } }
        public static string Modules { get { return nameof(Modules); } }
        #endregion

        #region ReportMenu
        public static string Tags { get { return nameof(Tags); } }
        #endregion
    }
}
