using System.Windows.Forms;

namespace AutomationProjectShareholderBaseNationalityCheck
{
    public delegate void UpdateProgressDelegate(string value);
    public delegate void WorkStarted();
    public delegate void WorkFinished();
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

            logic.MessageChanged += UpdateLabel;
            logic.WorkFinished += WorkFinished;
            logic.WorkStarted += WorkStarted;
		}

        private void WorkStarted()
        {
            if (this.InvokeRequired)
            {
                var progrDel = new WorkStarted(WorkStarted);
                this.BeginInvoke(progrDel);
            }
            else
            {
                this.SelectUSADocFileButton.Enabled = false;
                this.SelectSecurityFilesFolderButton.Enabled = false;
                this.SelectUSAListFileButton.Enabled = false;
                this.StartButton.Enabled = false;
            }
        }

        private void WorkFinished()
        {
            if (this.InvokeRequired)
            {
                var progrDel = new WorkFinished(WorkFinished);
                this.BeginInvoke(progrDel);
            }
            else
            {
                this.SelectUSADocFileButton.Enabled = true;
                this.SelectSecurityFilesFolderButton.Enabled = true;
                this.SelectUSAListFileButton.Enabled = true;
                this.StartButton.Enabled = true;
            }
        }

        private void UpdateLabel(string value)
        {
            if(this.StatusLabel.InvokeRequired)
            {
                var progrDel = new UpdateProgressDelegate(UpdateLabel);
                this.BeginInvoke(progrDel, value);
            }
            else
            {
                this.StatusLabel.Text = value;
            }
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
            logic.RunProcess();
        }
	}
}
