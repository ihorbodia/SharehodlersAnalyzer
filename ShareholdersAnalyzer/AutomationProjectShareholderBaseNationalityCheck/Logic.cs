using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace AutomationProjectShareholderBaseNationalityCheck
{
    public class Logic
    {
        public DirectoryInfo securityFolderPath { get; private set; }
        public FileInfo USADocFilePath { get; private set; }
        public FileInfo USAListFilePath { get; private set; }

        public void GetUSASecurityFilesFolderPath(string str)
        {
			if (string.IsNullOrWhiteSpace(str)) { str = FilesHelper.SelectFolder(); }
            if (string.IsNullOrWhiteSpace(str)) return;
            securityFolderPath = new DirectoryInfo(str);

			Properties.Settings.Default.USAFolderPath = str;
			Properties.Settings.Default.Save();
		}
        public void GetUSADocFilePath(string str)
        {
			if (string.IsNullOrWhiteSpace(str)) { str = FilesHelper.SelectFile(); }
            if (string.IsNullOrWhiteSpace(str)) return;
            USADocFilePath = new FileInfo(str);

			Properties.Settings.Default.USADocFilePath = str;
			Properties.Settings.Default.Save();
		}
        public void GetUSAListFilesPath(string str)
        {
			if (string.IsNullOrWhiteSpace(str)) { str = FilesHelper.SelectFile(); }
			if (string.IsNullOrWhiteSpace(str)) { return; }
            USAListFilePath = new FileInfo(str);

			Properties.Settings.Default.USAListFilePath = str;
			Properties.Settings.Default.Save();
		}

		public bool CheckIsAppCanStartProcess() => true;
                //Directory.Exists(securityFolderPath?.FullName) &&
                //File.Exists(USADocFilePath?.FullName) &&
                //File.Exists(USAListFilePath?.FullName);

		public void StartProcessing()
		{
			FileInfo[] Files = securityFolderPath.GetFiles("*.xlsx");
			foreach (FileInfo file in Files)
			{
				string companyName = string.Empty;
				string fileName = file.Name.Replace(".xlsx", "");
				string docFileValue = string.Empty;

				using (var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read))
				{
					using (var reader = ExcelReaderFactory.CreateReader(stream))
					{
						var result = reader
							.AsDataSet()
							.Tables["Feuil1"]
							.AsEnumerable();
						if (result.Count() > 1)
						{
							companyName = result.Skip(1).First()?.ItemArray[0].ToString();
						}
					}
				}

				using (var stream = File.Open(USADocFilePath.FullName, FileMode.Open, FileAccess.ReadWrite))
				{
					using (var reader = ExcelReaderFactory.CreateReader(stream))
					{
						var result = reader
							.AsDataSet()
							.Tables["Feuil2"]
							.AsEnumerable();
						var item = result.FirstOrDefault(x => x.ItemArray[0].ToString().Equals(companyName));
						docFileValue = item?.ItemArray[5].ToString();
					}
				}

				using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(file.FullName)))
				{
					using (ExcelPackage pckg = new ExcelPackage(ms))
					{
						var sheet = pckg.Workbook.Worksheets[1];
						
						//TODO:

						pckg.Save();
					}
				}
			}
		}
    }
}
