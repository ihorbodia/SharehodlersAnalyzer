namespace ShareholdersAnalyzerGUI
{
	partial class MainGUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FolderChosenPath = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ChoosenPathLabel = new System.Windows.Forms.Label();
            this.StatusLabelText = new System.Windows.Forms.Label();
            this.ProcessFilesButton = new System.Windows.Forms.Button();
            this.ChooseFirstFolderButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FolderChosenPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ChoosenPathLabel);
            this.groupBox1.Controls.Add(this.StatusLabelText);
            this.groupBox1.Location = new System.Drawing.Point(7, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 176);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // FolderChosenPath
            // 
            this.FolderChosenPath.AutoSize = true;
            this.FolderChosenPath.Location = new System.Drawing.Point(9, 16);
            this.FolderChosenPath.Name = "FolderChosenPath";
            this.FolderChosenPath.Size = new System.Drawing.Size(50, 13);
            this.FolderChosenPath.TabIndex = 11;
            this.FolderChosenPath.Text = "File path:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Status:";
            // 
            // ChoosenPathLabel
            // 
            this.ChoosenPathLabel.Location = new System.Drawing.Point(9, 40);
            this.ChoosenPathLabel.Name = "ChoosenPathLabel";
            this.ChoosenPathLabel.Size = new System.Drawing.Size(408, 13);
            this.ChoosenPathLabel.TabIndex = 12;
            this.ChoosenPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusLabelText
            // 
            this.StatusLabelText.Location = new System.Drawing.Point(47, 150);
            this.StatusLabelText.Name = "StatusLabelText";
            this.StatusLabelText.Size = new System.Drawing.Size(160, 13);
            this.StatusLabelText.TabIndex = 10;
            // 
            // ProcessFilesButton
            // 
            this.ProcessFilesButton.Location = new System.Drawing.Point(330, 179);
            this.ProcessFilesButton.Name = "ProcessFilesButton";
            this.ProcessFilesButton.Size = new System.Drawing.Size(105, 25);
            this.ProcessFilesButton.TabIndex = 16;
            this.ProcessFilesButton.Text = "Process files";
            this.ProcessFilesButton.UseVisualStyleBackColor = true;
            this.ProcessFilesButton.Click += new System.EventHandler(this.ProcessFilesButton_Click);
            // 
            // ChooseFirstFolderButton
            // 
            this.ChooseFirstFolderButton.Location = new System.Drawing.Point(7, 180);
            this.ChooseFirstFolderButton.Name = "ChooseFirstFolderButton";
            this.ChooseFirstFolderButton.Size = new System.Drawing.Size(105, 25);
            this.ChooseFirstFolderButton.TabIndex = 15;
            this.ChooseFirstFolderButton.Text = "Choose file";
            this.ChooseFirstFolderButton.UseVisualStyleBackColor = true;
            this.ChooseFirstFolderButton.Click += new System.EventHandler(this.ChooseFirstFolderButton_Click);
            // 
            // MainGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 211);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ProcessFilesButton);
            this.Controls.Add(this.ChooseFirstFolderButton);
            this.Name = "MainGUI";
            this.ShowIcon = false;
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label FolderChosenPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label ChoosenPathLabel;
		private System.Windows.Forms.Label StatusLabelText;
		private System.Windows.Forms.Button ProcessFilesButton;
		private System.Windows.Forms.Button ChooseFirstFolderButton;
	}
}

