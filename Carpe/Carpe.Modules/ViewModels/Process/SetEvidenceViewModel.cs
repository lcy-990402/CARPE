using Carpe.Modules.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Carpe.Modules.ViewModels.Process
{
    public class SetEvidenceViewModel : ProcessViewModelBase, ISupportWizardNextCommand
    {
        #region Member
        public List<CarpeModule> CarpeModules
        {
            get { return GetValue<List<CarpeModule>>(nameof(CarpeModules)); }
            set { SetValue(value, nameof(CarpeModules)); }
        }
        public int SelectedIndex
        {
            get { return GetValue<int>(nameof(SelectedIndex)); }
            set { SetValue(value, changedCallback: SelectedIndexChanged); }
        }
        public string Description
        {
            get { return GetValue<string>(nameof(Description)); }
            set { SetValue(value, nameof(Description)); }
        }
        #endregion
        public void SetEvidencePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Supported types|*.E01; *.dd";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Case.EvidencePath = openFileDialog.FileName;
            }
        }
        #region Command

        #endregion

        #region Constructor
        public SetEvidenceViewModel()
        {
            ShowCancel = true;
            ShowBack = true;
            ShowNext = true;
            AllowFinish = false;

            CarpeModules = new List<CarpeModule>();
            GetModules();
        }
        #endregion

        #region Method
        private void GetModules()
        {
            string directory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            StreamReader reader = new StreamReader(directory + "/Data/CarpeModules.csv");
            int cnt = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] data = line.Split(',');
                CarpeModules.Add(new CarpeModule(cnt++, data[0], Int32.Parse(data[1]), ""));
            }
            reader.Close();
        }

        private void SelectedIndexChanged(int value)
        {
            string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName +
                              "/Data/ModulesDescriptions/" +
                              System.Globalization.CultureInfo.CurrentCulture.Name +
                              "/" + CarpeModules[value].Name + ".txt";
            if (new FileInfo(filePath).Exists) Description = File.ReadAllText(filePath);
            else Description = "No Description";
            RaisePropertyChanged(nameof(SelectedIndex));
        }

        private string SetPayload()
        {
            string payload = "";
            int count = 0;

            payload += "\"" + Case.EvidencePath + "\" ";
            payload += "\"" + Case.BaseDirectory + "\\" + Case.CaseName + "\" ";
            if (Case.CaseID != null) payload += "--cid " + Case.CaseID + " ";
            if (Case.CaseName != null) payload += "--case-name " + Case.CaseName + " ";
            if (Case.EvidenceID != null) payload += "--eid " + Case.EvidenceID + " ";
            if (Case.Investigator != null) payload += "--investigator " + Case.Investigator + " ";
            if (Case.Description != null) payload += "--case_desc " + Case.Description + " ";

            List<string> modChecked = new List<string>();
            foreach (CarpeModule item in CarpeModules)
            {
                if (item.IsChecked) modChecked.Add(item.Name);
                else count++;
            }

            if (modChecked.Count > 0 && count > 0)
            {
                payload += "--modules ";
                foreach (var item in modChecked)
                {
                    payload += item.Replace(" ", "_") + (item == modChecked.Last() ? "" : ",");
                }
            }
            payload += " --sqlite ";

            return payload;
        }

        #endregion

        #region ISupportWizardNextCommand
        public bool CanGoForward
        {
            get { return true; }
        }
        public void OnGoForward(CancelEventArgs e)
        {         
            this.GetRequiredService<IWizardService>().Navigate("ProcessView", Case, this);
            Messenger.Default.Send(SetPayload(), "ProcessViewModel");
        }
        #endregion
    }
}
