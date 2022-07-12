using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Carpe.Modules.Models
{
    public partial class Case : INotifyPropertyChanged
    {     
        #region Members
        private string dBPath;
        public string DBPath
        {
            get { return dBPath; }
            set
            {
                dBPath = value;
                OnPropertyChanged();
            }
        }

        private string caseName;
        public string CaseName
        {
            get { return caseName; }
            set
            {
                caseName = value;
                OnPropertyChanged();
            }
        }

        private string evidencePath;
        public string EvidencePath
        {
            get { return evidencePath; }
            set
            {
                evidencePath = value;
                OnPropertyChanged();
            }
        }

        private string baseDirectory;
        public string BaseDirectory
        {
            get { return baseDirectory; }
            set
            {
                baseDirectory = value;
                OnPropertyChanged();
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        private string caseID;
        public string CaseID
        {
            get { return caseID; }
            set
            {
                caseID = value;
                OnPropertyChanged();
            }
        }

        private string evidenceID;
        public string EvidenceID
        {
            get { return evidenceID; }
            set
            {
                evidenceID = value;
                OnPropertyChanged();
            }
        }

        private string investigator;
        public string Investigator
        {
            get { return investigator; }
            set
            {
                investigator = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Constructor
        public Case()
        {

        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
