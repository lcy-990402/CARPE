using Carpe.Common;
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
    public class OverViewModel : AnalyzeViewModelBase
    {
        #region Member
        public DataTable PartitionDT { get; private set; }
        public Collection<FileRatio> FilesData { get; private set; }
        public ObservableCollection<string> CurrentInfo { get; private set; }
        #endregion

        #region Constructor
        public static OverViewModel Create(string caption, Case carpeCase)
        {
            return ViewModelSource.Create(() => new OverViewModel(carpeCase)
            {
                Caption = caption,
                CarpeCase = carpeCase,
            });
        }

        protected OverViewModel(Case carpeCase)
        {
            CarpeCase = carpeCase;
            _sqlCon = new SQLiteConnection("Data Source=" + CarpeCase.DBPath);
            _sqlCon.Open();

            SetPartitionInfo();
            SetCurrentInfo();
            _sqlCon.Close();
        }
        #endregion

        #region Command
        #endregion

        #region Method
        private void SetPartitionInfo()
        {
            PartitionDT = new DataTable();
            FilesData = new ObservableCollection<FileRatio>();
            PartitionDT.Load(new SQLiteCommand("SELECT par_name, filesystem, start_sector, par_label, par_size FROM partition_info", _sqlCon).ExecuteReader());
            foreach(DataRow dataRow in PartitionDT.Rows) FilesData.Add(new FileRatio(dataRow["par_name"].ToString(), double.Parse(dataRow["par_size"].ToString())));
        }
        private void SetCurrentInfo()
        {
            CurrentInfo = new ObservableCollection<string>();
            _sqlReader = new SQLiteCommand("SELECT case_name, administrator, create_date, description FROM case_info", _sqlCon).ExecuteReader();
            while (_sqlReader.Read())
            {
                for (int i = 0; i < _sqlReader.FieldCount; i++) CurrentInfo.Add(_sqlReader.GetString(i));
            }
        }
        #endregion
    }

    public class FileRatio
    {
        public FileRatio(string argument, double value)
        {
            Argument = argument;
            Value = value;
        }

        public string Argument { get; private set; }
        public double Value { get; private set; }
    }
}
