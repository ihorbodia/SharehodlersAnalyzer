﻿using ShareholdersAnalyzer;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareholdersAnalyzerGUI
{
    public partial class MainGUI : Form
    {
        string chosenPath = string.Empty;
        string choosenTermsFilePath = string.Empty;
        ShareholderAnalyzerLogic ms;

        public MainGUI()
        {
            InitializeComponent();
            StatusLabelText.Text = "Choose file";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ChoosenPathLabel.AutoEllipsis = true;

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string displayableVersion = $"Shareholder analyzer {version.Major}.{version.Minor}.{version.Revision}";
            Text = displayableVersion;
        }

        private void ChooseFirstFolderButton_Click(object sender, EventArgs e)
        {
            chosenPath = FilesHelper.SelectFile();
            if (!string.IsNullOrEmpty(chosenPath.Trim()))
            {
                ChoosenPathLabel.Text = chosenPath;
            }
        }

        private void ProcessFilesButton_Click(object sender, EventArgs e)
        {
            if (!CanRunProcess())
            {
                MessageBox.Show("Need to specify both files", "Processing cannot be started!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ms = new ShareholderAnalyzerLogic(chosenPath, choosenTermsFilePath);
            if (string.IsNullOrEmpty(chosenPath.Trim()))
            {
                return;
            }
            StatusLabelText.Text = "Processing";
            ProcessFilesButton.BeginInvoke((MethodInvoker)delegate () { ProcessFilesButton.Enabled = false; });
            ChooseFirstFolderButton.BeginInvoke((MethodInvoker)delegate () { ChooseFirstFolderButton.Enabled = false; });
            try
            {
                new Task(() =>
                {
                    Thread t = new Thread(RunProgram);
                    t.Start();
                    t.Join();
                    StatusLabelText.BeginInvoke((MethodInvoker)delegate () { StatusLabelText.Text = "Finish"; });
                    ProcessFilesButton.BeginInvoke((MethodInvoker)delegate () { ProcessFilesButton.Enabled = true; });
                    ChooseFirstFolderButton.BeginInvoke((MethodInvoker)delegate () { ChooseFirstFolderButton.Enabled = true; });
                    Console.WriteLine("Finish");
                }).Start();
            }
            catch (Exception)
            {
                StatusLabelText.Text = "Something wrong";
            }
        }
        private async void RunProgram()
        {
            await ms.ProcessFileAsync();
            SaveFile();
        }
        private void SaveFile()
        {
            try
            {
                ms.SaveFile();
            }
            catch (InvalidOperationException ex)
            {
                DialogResult result = MessageBox.Show("Try to close excel file and click OK.", "Error while saving file", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == DialogResult.OK)
                {
                    SaveFile();
                }
                else
                {
                    return;
                }
            }
        }

        private bool CanRunProcess()
        {
            return !string.IsNullOrEmpty(ChoosenPathLabel.Text);
        }
    }
}
