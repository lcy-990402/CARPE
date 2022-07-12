using Carpe.Modules.Models;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpe.Modules.ViewModels.Analyze
{
    public class TagViewModel : AnalyzeViewModelBase
    {
        #region Member
        public DataTable DT { get; protected set; }

        public int TagID { get; protected set; } 
        public List<int> TaggedIDs { get; protected set; }
        public ObservableCollection<TreeItem> TaggedItems { get; protected set; }

        #endregion

        #region Constructor
        public static TagViewModel Create(string caption, string tagName, Case carpeCase)
        {
            return ViewModelSource.Create(() => new TagViewModel(tagName, carpeCase)
            {
                Caption = caption,
            });
        }

        protected TagViewModel(string tagName, Case carpeCase)
        {
            TaggedIDs = new List<int>();
            TaggedItems = new ObservableCollection<TreeItem>();
            DT = new DataTable();
            CarpeCase = carpeCase;
            _sqlCon = new SQLiteConnection("Data Source=" + CarpeCase.DBPath);
            _sqlCon.Open();
            _sqlReader = new SQLiteCommand(String.Format("SELECT tag_name_id FROM gui_tag_names WHERE name = '{0}'", tagName), _sqlCon).ExecuteReader();
            while (_sqlReader.Read())
            {
                TagID = _sqlReader.GetInt32(0);
            }
            _sqlReader = new SQLiteCommand(String.Format("SELECT obj_id FROM gui_tag_contents WHERE tag_name_id = '{0}'", TagID), _sqlCon).ExecuteReader();
            while (_sqlReader.Read())
            {
                TaggedIDs.Add(_sqlReader.GetInt32(0));
            }
            _sqlReader = new SQLiteCommand(String.Format("SELECT * FROM file_info WHERE id in ({0})", string.Join(",", TaggedIDs)), _sqlCon).ExecuteReader();
            DT.Load(_sqlReader);
        }
        #endregion

        #region Command
        #endregion

        #region Method
        #endregion
    }
}
