using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Carpe.Modules.Models
{
    public class CarpeModule : INotifyPropertyChanged
    {
        #region Member
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        #endregion

        #region Constructor
        public CarpeModule(int id, string name, int level, string description, bool isChecked = true)
        {
            IsChecked = isChecked;
            Id = id;
            Name = name;
            Level = level;
            Description = description;
        }

        #endregion

        #region Method
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
