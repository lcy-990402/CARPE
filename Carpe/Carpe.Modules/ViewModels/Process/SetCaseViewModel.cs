using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carpe.Modules.ViewModels.Process
{
    public class SetCaseViewModel : ProcessViewModelBase, ISupportWizardNextCommand
    {
        #region Member
        #endregion

        #region Command
        public void SetCaseDirectory()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath)) Case.BaseDirectory = folderBrowserDialog.SelectedPath;
        }

        #endregion

        #region Constructor
        protected SetCaseViewModel()
        {
            ShowCancel = true;
            ShowBack = false;
            ShowNext = true;
            ShowFinish = false;
            AllowFinish = false;
        }

        public static SetCaseViewModel Create()
        {
            return ViewModelSource.Create(() => new SetCaseViewModel());
        }
        #endregion

        #region Method


        #endregion

        #region ISupportWizardNextCommand
        public bool CanGoForward
        {
            get
            {
                return
                    Case.CaseID != String.Empty &&
                    Case.CaseName != String.Empty &&
                    Directory.Exists(Case.BaseDirectory);
            }
        }
        public void OnGoForward(CancelEventArgs e)
        {
            this.GetRequiredService<IWizardService>().Navigate("SetEvidenceView", Case, this);
        }
        #endregion

    }
}
