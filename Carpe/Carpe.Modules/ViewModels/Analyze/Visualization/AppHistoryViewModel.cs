using Carpe.Modules.Models;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Carpe.Modules.ViewModels.Analyze.Visualization
{
    public class AppHistoryViewModel : AnalyzeViewModelBase
    {
        #region Member
        public DataTable AppHistoryDT { get; private set; }
        public DataTable ShownDT { get; private set; }
        public ObservableCollection<AppHistoryDataPoint> FilesData { get; private set; }

        public int TimeUnit = 0;

        #endregion

        #region Constructor
        public static AppHistoryViewModel Create(string caption, Case carpeCase)
        {
            return ViewModelSource.Create(() => new AppHistoryViewModel(carpeCase)
            {
                Caption = caption,
            });
        }

        protected AppHistoryViewModel(Case carpeCase)
        {
            CarpeCase = carpeCase;
            _sqlCon = new SQLiteConnection("Data Source=" + CarpeCase.DBPath);
            _sqlCon.Open();
            _sqlReader = new SQLiteCommand("SELECT program_name as \"Process Name\", last_run_time as \"Execution Time\", program_path as \"Process Path\" FROM lv1_os_win_prefetch", _sqlCon).ExecuteReader();
            AppHistoryDT = new DataTable();
            ShownDT = new DataTable();
            AppHistoryDT.Load(_sqlReader);
            ShownDT = AppHistoryDT;
            SetAppHistoryInfo();

            _sqlCon.Close();
        }
        #endregion

        #region Command
        public void ChartDoubleClick(SeriesPoint point)
        {
            if (point == null) return;
            _sqlCon.Open();
            DateTime tmpDate = DateTime.Parse(point.Argument);

            switch (TimeUnit)
            {
                case 0:
                    DateTime destDate = tmpDate.AddYears(1);
                    string sqlStr = String.Format("SELECT COUNT(*) cnt, strftime(\'%Y-%m\', last_run_time) temp1 FROM lv1_os_win_prefetch WHERE temp1 >= \'{0}\' and temp1 <= \'{1}\' GROUP BY temp1 ORDER BY temp1 ASC", tmpDate.ToShortDateString(), destDate.ToShortDateString());
                    _sqlReader = new SQLiteCommand(sqlStr, _sqlCon).ExecuteReader();
                    FilesData.Clear();
                    while (_sqlReader.Read())
                    {
                        FilesData.Add(new AppHistoryDataPoint(_sqlReader["temp1"].ToString(), _sqlReader["cnt"].ToString()));
                    }
                    _sqlReader = new SQLiteCommand("SELECT program_name as \"Process Name\", last_run_time as \"Execution Time\", program_path as \"Process Path\" FROM lv1_os_win_prefetch WHERE strftime(\'%Y-%m\', last_run_time) >= \'{0}\' and strftime(\'%Y-%m\', last_run_time) <= \'{1}\'", _sqlCon).ExecuteReader();
                    ShownDT = new DataTable();
                    ShownDT.Load(_sqlReader);
                    TimeUnit += 1;
                    _sqlCon.Close();
                    break;
                case 1:
                    destDate = tmpDate.AddMonths(1);
                    sqlStr = String.Format("SELECT COUNT(*) cnt, strftime(\'%Y-%m-%d\', last_run_time) temp1 FROM lv1_os_win_prefetch WHERE temp1 >= \'{0}\' and temp1 <= \'{1}\' GROUP BY temp1 ORDER BY temp1 ASC", tmpDate.ToShortDateString(), destDate.ToShortDateString());
                    _sqlReader = new SQLiteCommand(sqlStr, _sqlCon).ExecuteReader();
                    FilesData.Clear();
                    while (_sqlReader.Read())
                    {
                        FilesData.Add(new AppHistoryDataPoint(_sqlReader["temp1"].ToString(), _sqlReader["cnt"].ToString()));
                    }
                    _sqlReader = new SQLiteCommand("SELECT program_name as \"Process Name\", last_run_time as \"Execution Time\", program_path as \"Process Path\" FROM lv1_os_win_prefetch WHERE strftime(\'%Y-%m-%d\', last_run_time) >= \'{0}\' and strftime(\'%Y-%m-%d\', last_run_time) <= \'{1}\'", _sqlCon).ExecuteReader();
                    ShownDT = new DataTable();
                    ShownDT.Load(_sqlReader);
                    TimeUnit += 1;

                    _sqlCon.Close();
                    break;
                case 2:
                    destDate = tmpDate.AddDays(1);
                    sqlStr = String.Format("SELECT COUNT(*) cnt, strftime(\'%Y-%m-%d %H:00:00\', last_run_time) temp1 FROM lv1_os_win_prefetch WHERE temp1 >= \'{0}\' and temp1 <= \'{1}\' GROUP BY temp1 ORDER BY temp1 ASC", tmpDate.ToShortDateString(), destDate.ToShortDateString());
                    _sqlReader = new SQLiteCommand(sqlStr, _sqlCon).ExecuteReader();
                    FilesData.Clear();
                    while (_sqlReader.Read())
                    {
                        FilesData.Add(new AppHistoryDataPoint(_sqlReader["temp1"].ToString(), _sqlReader["cnt"].ToString()));
                    }
                    _sqlReader = new SQLiteCommand("SELECT program_name as \"Process Name\", last_run_time as \"Execution Time\", program_path as \"Process Path\" FROM lv1_os_win_prefetch WHERE strftime(\'%Y-%m-%d %H:00:00\', last_run_time) >= \'{0}\' and strftime(\'%Y-%m-%d %H:00:00\', last_run_time) <= \'{1}\'", _sqlCon).ExecuteReader();
                    ShownDT = new DataTable();
                    ShownDT.Load(_sqlReader);
                    TimeUnit += 1;
                    _sqlCon.Close();
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Method

        private void SetAppHistoryInfo()
        {
            _sqlReader = new SQLiteCommand("SELECT COUNT(*) cnt, strftime(\'%Y\', last_run_time) year_month_day FROM lv1_os_win_prefetch GROUP BY year_month_day ORDER BY year_month_day ASC", _sqlCon).ExecuteReader();
            FilesData = new ObservableCollection<AppHistoryDataPoint>();
            while (_sqlReader.Read())
            {
                FilesData.Add(new AppHistoryDataPoint(_sqlReader["year_month_day"].ToString() + "-1-1", _sqlReader["cnt"].ToString()));
            }
        }
        #endregion
    }

    public class AppHistoryDataPoint
    {
        public DateTime Argument { get; set; }
        public int Value { get; set; }

        public AppHistoryDataPoint(string argument, string count)
        {
            Argument = DateTime.Parse(argument);
            Value = Int32.Parse(count);
        }
    }
}
