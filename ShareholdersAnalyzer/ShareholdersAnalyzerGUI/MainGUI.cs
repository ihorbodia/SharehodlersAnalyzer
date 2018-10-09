using ShareholdersAnalyzer;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareholdersAnalyzerGUI
{
	public partial class MainGUI : Form
	{
		string chosenPath = string.Empty;
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
				StatusLabelText.Text = "Start process";
				ChoosenPathLabel.Text = chosenPath;
			}
		}

		private void ProcessFilesButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(chosenPath.Trim()))
			{
				return;
			}
			StatusLabelText.Text = "Processing";
			ProcessFilesButton.BeginInvoke((MethodInvoker)delegate () { ProcessFilesButton.Enabled = false; });
			try
			{
				new Task(() =>
				{
					Thread t = new Thread(RunProgram);
					t.Start();
					t.Join();
					StatusLabelText.BeginInvoke((MethodInvoker)delegate () { StatusLabelText.Text = "Finish"; });
					ProcessFilesButton.BeginInvoke((MethodInvoker)delegate () { ProcessFilesButton.Enabled = true; });
					Console.WriteLine("Finish");
				}).Start();
			}
			catch (Exception)
			{
				StatusLabelText.Text = "Something wrong";
			}
		}
		private void RunProgram()
		{
			ShareholderAnalyzerLogic ms = new ShareholderAnalyzerLogic(chosenPath);
			ms.ProcessFile();
			try
			{
				ms.SaveFile();
			}
			catch (InvalidOperationException ex)
			{
				DialogResult result = MessageBox.Show("Try to close excel file and click OK.", "Error while saving file", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
				if (result == DialogResult.OK)
				{
					ms.SaveFile();
				}
			}
		}
	}
}
