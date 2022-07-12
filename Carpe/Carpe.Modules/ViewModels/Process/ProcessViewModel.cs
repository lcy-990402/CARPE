using Carpe.Common;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
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
    public class ProcessViewModel : ProcessViewModelBase, ISupportWizardFinishCommand
    {
        #region Member
        public string LogText
        {
            get { return GetValue<string>(nameof(LogText)); }
            set { SetValue(value, nameof(LogText)); }
        }
        public BackgroundWorker BW;
        public System.Diagnostics.Process CarpeProcess;
        private string payload;        
        #endregion

        #region Command
        #endregion

        #region Constructor
        public ProcessViewModel()
        {
            Messenger.Default.Register<string>(
                recipient :this,
                token : "ProcessViewModel",
                action :OnMessage);
            BW = new BackgroundWorker();
            BW.DoWork += BW_Dowork;

            ShowFinish = true;
            AllowFinish = false;

        }
        #endregion

        #region Method
        private void BW_Dowork(object sender, DoWorkEventArgs e)
        {
            LogText += payload;
            CarpeProcess = new System.Diagnostics.Process();
            CarpeProcess.StartInfo.FileName = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Data/carpe.exe";
            CarpeProcess.StartInfo.Arguments = payload;
            CarpeProcess.StartInfo.CreateNoWindow = true;
            CarpeProcess.StartInfo.UseShellExecute = false;
            CarpeProcess.StartInfo.RedirectStandardOutput = true;
            CarpeProcess.StartInfo.RedirectStandardError = true;
            CarpeProcess.EnableRaisingEvents = true;
            CarpeProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler((s, e2) =>
            {
                LogText += e2.Data;
                LogText += "\n";
            });
            CarpeProcess.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler((s, e2) =>
            {
                LogText += e2.Data;
                LogText += "\n";
            });
            CarpeProcess.Exited += new EventHandler((s, e2) =>
            {
                AllowFinish = true;
            });
            CarpeProcess.Start();
            CarpeProcess.BeginOutputReadLine();
        }
        private void OnMessage(string message)
        {
            payload = message;
            BW.RunWorkerAsync();
        }

        private void CancelProcess()
        {
            CarpeProcess.Kill();
        }
        #endregion

        #region IsupportWizardFinisiCommand
        public bool CanFinish
        {
            get { return AllowFinish; }
        }

        public void OnFinish(CancelEventArgs e)
        {
            this.GetService<IMessageBoxService>().ShowMessage("Process Finished", "CARPE", MessageButton.OK, MessageIcon.Exclamation);
            ModuleManager.DefaultManager.InjectOrNavigate(Regions.MainWindow, "Main");
            Messenger.Default.Send(Case, "MainViewModel");
        }
        #endregion
    }
}
