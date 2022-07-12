using Carpe.Modules.Models;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpe.Modules.ViewModels.Analyze
{
    public class CarpeModuleViewModel : AnalyzeViewModelBase
    {
        #region Member
        public DataTable DT { get; protected set; }
        #endregion

        #region Constructor
        public static CarpeModuleViewModel Create(string caption, string tableName, Case carpeCase)
        {
            return ViewModelSource.Create(() => new CarpeModuleViewModel(tableName, carpeCase)
            {
                Caption = caption,
            });
        }

        protected CarpeModuleViewModel(string tableName, Case carpeCase)
        {
            CarpeCase = carpeCase;
            _sqlCon = new SQLiteConnection("Data Source=" + CarpeCase.DBPath);
            _sqlCon.Open();
            _sqlReader = new SQLiteCommand("SELECT * FROM " + tableName, _sqlCon).ExecuteReader();
            DT = new DataTable();
            DT.Load(_sqlReader);
        }
        #endregion

        #region Command
        #endregion

        #region Method
        #endregion
    }
}
