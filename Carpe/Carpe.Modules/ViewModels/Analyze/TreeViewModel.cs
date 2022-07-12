using Carpe.Modules.Models;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Carpe.Modules.ViewModels.Analyze
{
    public class TreeViewModel : AnalyzeViewModelBase
    {
        #region Member
        public List<TreeItem> Items { get; set; }
        public ObservableCollection<TreeItem> ListItems { get; set; }
        public ObservableCollection<string> TagList { get; set; }
        private Dictionary <string, int> _tagDict { get; set; }
        public List<int> TaggedList { get; set; }
        private Dictionary<string, int> _partitionInfo;
        private string _evidencePath;
        #endregion

        #region Constructor
        public static TreeViewModel Create(string caption, Case carpeCase)
        {
            return ViewModelSource.Create(() => new TreeViewModel(carpeCase)
            {
                Caption = caption,
            });
        }

        protected TreeViewModel(Case carpeCase)
        {
            CarpeCase = carpeCase;
            Items = new List<TreeItem>();
            ListItems = new ObservableCollection<TreeItem>();
            _sqlCon = new SQLiteConnection("Data Source=" + CarpeCase.DBPath);
            _sqlCon.Open();
            GetEvidenceInfo();
            InitiateTreeView();
            SetTagList();
            SetTaggedList();
        }
        #endregion

        #region Command
        public void NodeDoubleClick(TreeItem treeItem)
        {
            ListItems.Clear();
            if(treeItem.SubDirectory.Count == 0 && treeItem.SubFile.Count == 0)
            {
                _sqlReader = new SQLiteCommand(String.Format("SELECT id, file_id, name, size, mtime, atime, ctime, extension, parent_path, dir_type FROM file_info WHERE par_id = '{0}' AND parent_id = '{1}'", treeItem.PartitionId, treeItem.FileId), _sqlCon).ExecuteReader();

                while (_sqlReader.Read())
                {
                    if(_sqlReader["dir_type"].ToString() == "3")
                    {
                        treeItem.SubDirectory.Add(new TreeItem(Int32.Parse(_sqlReader["id"].ToString()),
                                                               Int32.Parse(_sqlReader["file_id"].ToString()),
                                                               treeItem.FileId,
                                                               treeItem.PartitionId,
                                                               _sqlReader["name"].ToString(),
                                                               new DateTime(1601, 1, 1, 0, 0, 0, 0).AddSeconds(long.Parse(_sqlReader["ctime"].ToString())),
                                                               new DateTime(1601, 1, 1, 0, 0, 0, 0).AddSeconds(long.Parse(_sqlReader["mtime"].ToString())),
                                                               new DateTime(1601, 1, 1, 0, 0, 0, 0).AddSeconds(long.Parse(_sqlReader["atime"].ToString())),
                                                               long.Parse(_sqlReader["size"].ToString()),
                                                               _sqlReader["parent_path"].ToString(),
                                                               _sqlReader["extension"].ToString(),
                                                               treeItem,
                                                               TaggedList.Contains(Int32.Parse(_sqlReader["id"].ToString())),
                                                               true));
                    }
                    else
                    {
                        treeItem.SubFile.Add(new TreeItem(Int32.Parse(_sqlReader["id"].ToString()),
                                                          Int32.Parse(_sqlReader["file_id"].ToString()),
                                                          treeItem.FileId,
                                                          treeItem.PartitionId,
                                                          _sqlReader["name"].ToString(),
                                                          new DateTime(1601, 1, 1, 0, 0, 0, 0).AddSeconds(long.Parse(_sqlReader["ctime"].ToString())),
                                                          new DateTime(1601, 1, 1, 0, 0, 0, 0).AddSeconds(long.Parse(_sqlReader["mtime"].ToString())),
                                                          new DateTime(1601, 1, 1, 0, 0, 0, 0).AddSeconds(long.Parse(_sqlReader["atime"].ToString())),
                                                          long.Parse(_sqlReader["size"].ToString()),
                                                          _sqlReader["parent_path"].ToString(),
                                                          _sqlReader["extension"].ToString(),
                                                          treeItem,
                                                          TaggedList.Contains(Int32.Parse(_sqlReader["id"].ToString()))));
                    }                    
                }
                treeItem.IsExpanded = true;
            }
            foreach (TreeItem item in treeItem.SubDirectory) ListItems.Add(item);
            foreach (TreeItem item in treeItem.SubFile) ListItems.Add(item);
        }

        public void ListViewExtractFile(IList list)
        {
            if (list is null)
            {
                MessageBox.Show("No Items Selected");
                return;
            }

            var collection = list.Cast<TreeItem>();
            string di = "";

            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) di = folderBrowserDialog.SelectedPath;


        }

        public void AddTag(object parameter)
        {
            var values = (object[])parameter;
            var collection = ((IList)values[0]);

            int i = _tagDict[(string)values[1]];
            int cnt = 0;
            SQLiteCommand command = _sqlCon.CreateCommand();

            command.CommandText = "INSERT INTO gui_tag_contents (obj_id, tag_name_id) VALUES ";
            foreach (TreeItem item in collection) 
            {                              
                if (cnt != 0) command.CommandText += ",";
                command.CommandText += String.Format("('{0}', '{1}')", item.Id, i);
                item.IsTagged = true;
                cnt++;         
                
            }
            command.ExecuteNonQuery();
            SetTaggedList();
        }
        #endregion

        #region Method
        private void GetEvidenceInfo()
        {
            _partitionInfo = new Dictionary<string, int>();
            _sqlReader = new SQLiteCommand("SELECT par_id, start_sector FROM partition_info", _sqlCon).ExecuteReader();
            while (_sqlReader.Read()) _partitionInfo.Add(_sqlReader.GetString(0), _sqlReader.GetInt32(1));
            _sqlReader = new SQLiteCommand("SELECT * FROM evidence_info", _sqlCon).ExecuteReader();
            _sqlReader.Read();
            _evidencePath = _sqlReader.GetString(2);
        }
        private void InitiateTreeView()
        {
            _sqlReader = new SQLiteCommand("SELECT par_id, par_name, par_size FROM partition_info", _sqlCon).ExecuteReader();
            while (_sqlReader.Read())
            {
                Items.Add(new TreeItem(null, 5, null, _sqlReader["par_id"].ToString(), _sqlReader["par_name"].ToString(), default, default, default, long.Parse(_sqlReader["par_size"].ToString()), string.Empty, string.Empty, null));
            }
        }
        private void SetTagList()
        {
            TagList = new ObservableCollection<string>();
            _tagDict = new Dictionary<string, int>();
            _sqlReader = new SQLiteCommand("SELECT tag_name_id, name FROM gui_tag_names", _sqlCon).ExecuteReader();
            while (_sqlReader.Read())
            {
                _tagDict.Add(_sqlReader.GetString(1), Int32.Parse(_sqlReader["tag_name_id"].ToString()));
                TagList.Add(_sqlReader.GetString(1));
            }
        }
        private void SetTaggedList()
        {
            TaggedList = new List<int>();
            _sqlReader = new SQLiteCommand("SELECT DISTINCT obj_id FROM gui_tag_contents", _sqlCon).ExecuteReader();
            while (_sqlReader.Read())
            {
                TaggedList.Add(_sqlReader.GetInt32(0));
            }       
        }
        #endregion
    }
}
