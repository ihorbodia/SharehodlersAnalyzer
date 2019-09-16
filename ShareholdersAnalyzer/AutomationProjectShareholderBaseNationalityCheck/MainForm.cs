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
		}

		private void SelectSecurityFilesFolderButton_Click(object sender, System.EventArgs e)
		{
			logic.GetUSASecurityFilesFolderPath();
			this.SecurityFolderPathLabel.Text = logic.securityFolderPath.ToString();
		}

		private void SelectUSADocFileButton_Click(object sender, System.EventArgs e)
		{
			logic.GetUSADocFilePath();
			this.USADocFilePathLabel.Text = logic.USADocFilePath.ToString();
		}

		private void SelectUSAListFileButton_Click(object sender, System.EventArgs e)
		{
			logic.GetUSAListFilesPath();
			this.USAListFilePathLabel.Text = logic.USAListFilePath.ToString();
		}

		private void StartButton_Click(object sender, System.EventArgs e)
		{
			logic.StartProcessing();	
		}
	}
}
