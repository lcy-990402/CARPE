using Carpe.Modules.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpe.Modules.ViewModels.Process
{
    public class ProcessViewModelBase :  ViewModelBase, ISupportParameter, ISupportWizardCancelCommand
    {
        #region Member
        public virtual Case Case { get; protected set; }
        object ISupportParameter.Parameter
        {
            get { return Case; }
            set { Case = (Case)value; }
        }
        public bool CanCancel
        {
            get { return GetCanCancel(); }
        }
        public virtual bool ShowNext { get; set; }
        public virtual bool ShowBack { get; set; }
        public virtual bool ShowCancel { get; set; }
        public virtual bool ShowFinish { get; set; }
        public virtual bool AllowNext { get; set; }
        public virtual bool AllowBack { get; set; }
        public virtual bool AllowCancel { get; set; }
        public virtual bool AllowFinish { get; set; }
        #endregion

        #region Method
        protected virtual bool GetCanCancel()
        {
            return true;
        }

        public void OnCancel(CancelEventArgs e)
        {
            if(this.GetService<IMessageBoxService>().
                ShowMessage("Do you want to cancel?", "Cancel", MessageButton.YesNo, MessageIcon.Question) == MessageResult.No)
                e.Cancel = true;
        }

        #endregion       


    }
}
