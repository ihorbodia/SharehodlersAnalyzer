using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutomationProjectShareholderBaseNationalityCheck
{
    public delegate void MessageChangedHandler(string value);
    public delegate void WorkFinishedHandler();
    public delegate void WorkStartedHandler();

    public class Logic
    {
        public event MessageChangedHandler MessageChanged;
        public event WorkFinishedHandler WorkFinished;
        public event WorkStartedHandler WorkStarted;
        public DirectoryInfo securityFolderPath { get; private set; }
        public FileInfo USADocFilePath { get; private set; }
        public FileInfo USAListFilePath { get; private set; }
        public string InfoMessage { get; set; }

        private void OnMessageChanged(string value)
        {
            var del = MessageChanged as MessageChangedHandler;
            if (del != null)
            {
                InfoMessage = value;
                del(value);
            }
        }

        private void OnWorkStarted()
        {
            (WorkStarted as WorkStartedHandler)?.Invoke();
        }

        private void OnWorkFinished()
        {
            (WorkFinished as WorkFinishedHandler)?.Invoke();
        }

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

		public bool CheckIsAppCanStartProcess() =>
        Directory.Exists(securityFolderPath?.FullName) &&
        File.Exists(USADocFilePath?.FullName) &&
        File.Exists(USAListFilePath?.FullName);


        public void RunProcess()
        {
            Task.Factory.StartNew(new Action(StartProcessing));
        }

        private void StartProcessing()
        {
            OnWorkStarted();
            OnMessageChanged("Start processing...");

            FileInfo[] Files = securityFolderPath.GetFiles("*.xlsx");
            int filesCount = Files.Count();
            for (int f = 0; f < filesCount; f++)
            {
                FileInfo file = Files[f];
         
                string companyName = string.Empty;
                string fileName = file.Name.Replace(".xlsx", "");
                string docFileValue = string.Empty;
                
                try
                {
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
                }
                catch (Exception ex)
                {
                    OnMessageChanged(ex.Message);
                }
               

                try
                {
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
                }
                catch (Exception ex)
                {
                    OnMessageChanged(ex.Message);
                }


                try
                {
                    using (ExcelPackage pckg = new ExcelPackage(USAListFilePath))
                    {
                        var sheet = pckg.Workbook.Worksheets[1];
                        for (int i = 2; i < sheet.Dimension.Rows + 1; i++)
                        {
                            string data = sheet.Cells[$"B{i}"].Text;
                            if (data.Equals(fileName))
                            {
                                sheet.Cells[$"Z{i}"].Value = docFileValue;
                                break;
                            }
                        }
                        pckg.Save();
                    }
                }
                catch (Exception ex)
                {
                    OnMessageChanged(ex.Message);
                }

                OnMessageChanged($"Processed: {f + 1}/{filesCount + 1}");
            }
            OnMessageChanged("Finished");
            OnWorkFinished();
        }
    }
}
