using System.Windows.Forms;

namespace AutomationProjectShareholderBaseNationalityCheck
{
	public partial class MainForm : Form
	{
		Logic logic;
		public MainForm()
		{
			InitializeComponent();
			logic = new Logic();
			this.SecurityFolderPathLabel.Text = Properties.Settings.Default.USAFolderPath;
			this.USADocFilePathLabel.Text = Properties.Settings.Default.USADocFilePath;
			this.USAListFilePathLabel.Text = Properties.Settings.Default.USAListFilePath;
		}

		private void SelectSecurityFilesFolderButton_Click(object sender, System.EventArgs e)
		{
			logic.GetUSASecurityFilesFolderPath(null);
			this.SecurityFolderPathLabel.Text = logic.securityFolderPath.ToString();
            this.StartButton.Enabled = logic.CheckIsAppCanStartProcess();
        }

		private void SelectUSADocFileButton_Click(object sender, System.EventArgs e)
		{
			logic.GetUSADocFilePath(null);
			this.USADocFilePathLabel.Text = logic.USADocFilePath.ToString();
            this.StartButton.Enabled = logic.CheckIsAppCanStartProcess();
        }

		private void SelectUSAListFileButton_Click(object sender, System.EventArgs e)
		{
			logic.GetUSAListFilesPath(null);
			this.USAListFilePathLabel.Text = logic.USAListFilePath.ToString();
            this.StartButton.Enabled = logic.CheckIsAppCanStartProcess();
        }

		private void StartButton_Click(object sender, System.EventArgs e)
		{
			logic.GetUSASecurityFilesFolderPath(Properties.Settings.Default.USAFolderPath);
			logic.GetUSADocFilePath(Properties.Settings.Default.USADocFilePath);
			logic.GetUSAListFilesPath(Properties.Settings.Default.USAListFilePath);
			this.StatusLabel.Text = "Processing...";
            logic.StartProcessing();
            this.StatusLabel.Text = "Finished";
        }
	}
}
