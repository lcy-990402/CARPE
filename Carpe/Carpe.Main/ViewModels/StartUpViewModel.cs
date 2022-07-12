using Carpe.Common;
using Carpe.Modules.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carpe.Main.ViewModels
{
    public class StartUpViewModel
    {
        #region Member
        private SQLiteConnection _sqlCon;       
        protected IModuleManager Manager { get { return ModuleManager.DefaultManager; } }
        #endregion

        #region Command
        public void OpenCase()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DB Files|*.db|CARPE Case Files|*.carpe";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                switch (Path.GetExtension(openFileDialog.FileName).ToLower())
                {
                    case ".db":                       
                        Case carpeCase = new Case();
                        carpeCase.DBPath = openFileDialog.FileName;
                        Messenger.Default.Send(carpeCase, "MainViewModel");
                        Manager.InjectOrNavigate(Regions.MainWindow, "Main");
                        //Manager.InjectOrNavigate(Regions.Documents, "OverView");
                        break;
                    case ".carpe":
                        break;
                    default:
                        break;
                }
            }
        }

        public void NewCase()
        {
            var wizardResult = this.GetRequiredService<IDialogService>().ShowDialog(MessageButton.OKCancel, "New Case", this).ToString();
        }

        public void ViewLoaded()
        {
            this.GetRequiredService<IWizardService>().Navigate("SetCaseView", null, new Case(), this);
        }

        #endregion

        #region Method
        #endregion

        #region Constructor
        public static StartUpViewModel Create()
        {
            return ViewModelSource.Create(() => new StartUpViewModel());
        }

        #endregion

    }
}
