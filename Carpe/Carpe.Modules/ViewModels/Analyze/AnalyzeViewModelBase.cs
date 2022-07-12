using Carpe.Common;
using Carpe.Modules.Models;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpe.Modules.ViewModels.Analyze
{
    public class AnalyzeViewModelBase : ViewModelBase, IDocumentModule
    {
        public Case CarpeCase { get; set; }

        public string Caption { get; protected set; }
        public virtual bool IsActive { get; set; }

        protected SQLiteConnection _sqlCon;
        protected SQLiteDataReader _sqlReader;
    }
}
