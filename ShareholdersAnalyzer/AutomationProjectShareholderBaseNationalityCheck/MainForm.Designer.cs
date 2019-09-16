namespace AutomationProjectShareholderBaseNationalityCheck
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SelectSecurityFilesFolderButton = new System.Windows.Forms.Button();
			this.SelectUSADocFileButton = new System.Windows.Forms.Button();
			this.SelectUSAListFileButton = new System.Windows.Forms.Button();
			this.StartButton = new System.Windows.Forms.Button();
			this.SecurityFolderPathLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.StatusLabel = new System.Windows.Forms.Label();
			this.USADocFilePathLabel = new System.Windows.Forms.Label();
			this.USAListFilePathLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// SelectSecurityFilesFolderButton
			// 
			this.SelectSecurityFilesFolderButton.Location = new System.Drawing.Point(457, 12);
			this.SelectSecurityFilesFolderButton.Name = "SelectSecurityFilesFolderButton";
			this.SelectSecurityFilesFolderButton.Size = new System.Drawing.Size(151, 23);
			this.SelectSecurityFilesFolderButton.TabIndex = 0;
			this.SelectSecurityFilesFolderButton.Text = "Select security files folder";
			this.SelectSecurityFilesFolderButton.UseVisualStyleBackColor = true;
			this.SelectSecurityFilesFolderButton.Click += new System.EventHandler(this.SelectSecurityFilesFolderButton_Click);
			// 
			// SelectUSADocFileButton
			// 
			this.SelectUSADocFileButton.Location = new System.Drawing.Point(457, 41);
			this.SelectUSADocFileButton.Name = "SelectUSADocFileButton";
			this.SelectUSADocFileButton.Size = new System.Drawing.Size(151, 23);
			this.SelectUSADocFileButton.TabIndex = 1;
			this.SelectUSADocFileButton.Text = "Select USA doc file";
			this.SelectUSADocFileButton.UseVisualStyleBackColor = true;
			this.SelectUSADocFileButton.Click += new System.EventHandler(this.SelectUSADocFileButton_Click);
			// 
			// SelectUSAListFileButton
			// 
			this.SelectUSAListFileButton.Location = new System.Drawing.Point(457, 70);
			this.SelectUSAListFileButton.Name = "SelectUSAListFileButton";
			this.SelectUSAListFileButton.Size = new System.Drawing.Size(151, 23);
			this.SelectUSAListFileButton.TabIndex = 2;
			this.SelectUSAListFileButton.Text = "Select USA list file";
			this.SelectUSAListFileButton.UseVisualStyleBackColor = true;
			this.SelectUSAListFileButton.Click += new System.EventHandler(this.SelectUSAListFileButton_Click);
			// 
			// StartButton
			// 
			this.StartButton.Location = new System.Drawing.Point(457, 111);
			this.StartButton.Name = "StartButton";
			this.StartButton.Size = new System.Drawing.Size(152, 23);
			this.StartButton.TabIndex = 5;
			this.StartButton.Text = "Start";
			this.StartButton.UseVisualStyleBackColor = true;
			this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
			// 
			// SecurityFolderPathLabel
			// 
			this.SecurityFolderPathLabel.AutoSize = true;
			this.SecurityFolderPathLabel.Location = new System.Drawing.Point(12, 17);
			this.SecurityFolderPathLabel.Name = "SecurityFolderPathLabel";
			this.SecurityFolderPathLabel.Size = new System.Drawing.Size(0, 13);
			this.SecurityFolderPathLabel.TabIndex = 6;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 120);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Status:";
			// 
			// StatusLabel
			// 
			this.StatusLabel.AutoSize = true;
			this.StatusLabel.Location = new System.Drawing.Point(60, 120);
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Size = new System.Drawing.Size(0, 13);
			this.StatusLabel.TabIndex = 8;
			// 
			// USADocFilePathLabel
			// 
			this.USADocFilePathLabel.AutoSize = true;
			this.USADocFilePathLabel.Location = new System.Drawing.Point(12, 46);
			this.USADocFilePathLabel.Name = "USADocFilePathLabel";
			this.USADocFilePathLabel.Size = new System.Drawing.Size(0, 13);
			this.USADocFilePathLabel.TabIndex = 9;
			// 
			// USAListFilePathLabel
			// 
			this.USAListFilePathLabel.AutoSize = true;
			this.USAListFilePathLabel.Location = new System.Drawing.Point(12, 75);
			this.USAListFilePathLabel.Name = "USAListFilePathLabel";
			this.USAListFilePathLabel.Size = new System.Drawing.Size(0, 13);
			this.USAListFilePathLabel.TabIndex = 10;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(618, 146);
			this.Controls.Add(this.USAListFilePathLabel);
			this.Controls.Add(this.USADocFilePathLabel);
			this.Controls.Add(this.StatusLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.SecurityFolderPathLabel);
			this.Controls.Add(this.StartButton);
			this.Controls.Add(this.SelectUSAListFileButton);
			this.Controls.Add(this.SelectUSADocFileButton);
			this.Controls.Add(this.SelectSecurityFilesFolderButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "MainForm";
			this.Text = "Automation project shareholder base nationality check";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button SelectSecurityFilesFolderButton;
		private System.Windows.Forms.Button SelectUSADocFileButton;
		private System.Windows.Forms.Button SelectUSAListFileButton;
		private System.Windows.Forms.Button StartButton;
		private System.Windows.Forms.Label SecurityFolderPathLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label StatusLabel;
		private System.Windows.Forms.Label USADocFilePathLabel;
		private System.Windows.Forms.Label USAListFilePathLabel;
	}
}

