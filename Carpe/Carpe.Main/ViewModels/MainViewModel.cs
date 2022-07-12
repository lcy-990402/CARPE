using Carpe.Common;
using Carpe.Modules.Models;
using Carpe.Modules.ViewModels.Analyze;
using Carpe.Modules.ViewModels.Analyze.Visualization;
using Carpe.Modules.Views.Analyze;
using Carpe.Modules.Views.Analyze.Visualization;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.POCO;
using System.Data;
using System.Data.SQLite;

namespace Carpe.Main.ViewModels
{
    public class MainViewModel
    {
        #region Member
        Case CarpeCase;

        private SQLiteConnection _sqlCon;

        protected IModuleManager Manager { get { return ModuleManager.DefaultManager; } }
        #endregion

        #region Constructor
        public static MainViewModel Create()
        {
            return ViewModelSource.Create(() => new MainViewModel());
        }

        protected MainViewModel()
        {
            Messenger.Default.Register<Case>(
               recipient: this,
               token: "MainViewModel",
               action: OnMessage);
        }
        #endregion

        #region Method     
        private void OnMessage(Case carpeCase)
        {
            CarpeCase = carpeCase;
            _sqlCon = new SQLiteConnection("Data Source=" + CarpeCase.DBPath);
            _sqlCon.Open();
            AddModules();
            _sqlCon.Close();
            Manager.InjectOrNavigate(Regions.Documents, "Overview");
        }
        private void AddModules()
        {
            Manager.Register(Regions.Overview, new Module("Overview", () => new NavigationItem("Overview")));
            Manager.Register(Regions.Documents, new Module("Overview", () => OverViewModel.Create("Overview", CarpeCase), typeof(OverView)));
            Manager.Inject(Regions.Overview, "Overview");

            foreach(DataRow row in _sqlCon.GetSchema("Tables").Rows)
            {
                string tableName = row[2].ToString();
                if (tableName == "sqlite_sequence") continue;
                if (tableName == "file_info")
                {
                    //File Tree View 생성
                    Manager.Register(Regions.Filesystem, new Module(tableName, () => new NavigationItem(tableName)));
                    Manager.Register(Regions.Documents, new Module(tableName, () => TreeViewModel.Create(tableName, CarpeCase), typeof(TreeView)));
                    Manager.Inject(Regions.Filesystem, tableName);
                }
                else
                {
                    string region = tableName.Substring(0, 2) == "lv" ? Regions.Modules : Regions.Filesystem;
                    if (tableName.Substring(0, 3) == "gui") continue;
                    Manager.Register(region, new Module(tableName, () => new NavigationItem(tableName)));
                    Manager.Register(Regions.Documents, new Module(tableName, () => CarpeModuleViewModel.Create(tableName, tableName, CarpeCase), typeof(CarpeModuleView)));
                    Manager.Inject(region, tableName);
                }
            }

            using (SQLiteCommand command = _sqlCon.CreateCommand())
            {
                try
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS gui_tag_names(tag_name_id INTEGER PRIMARY KEY, name TEXT UNIQUE)";
                    command.ExecuteNonQuery();
                    command.CommandText = "CREATE TABLE IF NOT EXISTS gui_tag_contents(tag_id INTEGER PRIMARY KEY, obj_id INTEGER NOT NULL, tag_name_id INTEGER NOT NULL)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO gui_tag_names (name) values('Bookmark'), ('Favorite')";
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {

                }

            }

            // Tag View 추가 부분            
            SQLiteDataReader _sqlReader = new SQLiteCommand("SELECT name FROM gui_tag_names", _sqlCon).ExecuteReader();
            while (_sqlReader.Read())
            {
                string tagName = _sqlReader.GetString(0);
                Manager.Register(Regions.Tags, new Module(tagName, () => new NavigationItem(tagName)));
                Manager.Register(Regions.Documents, new Module(tagName, () => TagViewModel.Create(tagName, tagName, CarpeCase), typeof(TagView)));
                Manager.Inject(Regions.Tags, tagName);
            }

            //Visualization 부분
            Manager.Register(Regions.Visualization, new Module("AppHistory", () => new NavigationItem("AppHistory")));
            Manager.Register(Regions.Documents, new Module("AppHistory", () => AppHistoryViewModel.Create("AppHistory", CarpeCase), typeof(AppHistoryView)));
            Manager.Inject(Regions.Visualization, "AppHistory");
           
        }
        #endregion
    }
}
