using OfficeOpenXml;
using System.Data;
using System.IO;

namespace AutomationProjectShareholderBaseNationalityCheck
{
    public class Logic
    {
        public DirectoryInfo securityFolderPath { get; private set; }
        public FileInfo USADocFilePath { get; private set; }
        public FileInfo USAListFilePath { get; private set; }

        public void GetUSASecurityFilesFolderPath()
        {
            var str = FilesHelper.SelectFolder();
            if (string.IsNullOrWhiteSpace(str)) return;
            securityFolderPath = new DirectoryInfo(str);
        }
        public void GetUSADocFilePath()
        {
            var str = FilesHelper.SelectFile();
            if (string.IsNullOrWhiteSpace(str)) return;
            USADocFilePath = new FileInfo(str);
        }
        public void GetUSAListFilesPath()
        {
            var str = FilesHelper.SelectFile();
            if (string.IsNullOrWhiteSpace(str)) return;
            USAListFilePath = new FileInfo(str);
        }

        public bool CheckIsAppCanStartProcess() => 
                Directory.Exists(securityFolderPath?.FullName) &&
                 File.Exists(USADocFilePath?.FullName) &&
                 File.Exists(USAListFilePath?.FullName);

        public void StartProcessing()
        {
            ExcelPackage usaDocFile = new ExcelPackage(USADocFilePath);
            ExcelWorksheet workSheet = usaDocFile.Workbook.Worksheets[1];
            DataTable dt = new DataTable();
            var start = workSheet.Dimension.Start.Row + 1;
            var end = workSheet.Dimension.End.Row;
            for (int row = start; row <= end; row++)
            {
                string name = FilesHelper.CleanName(workSheet.Cells[row, 3].Text);
                string URL = workSheet.Cells[row, 4].Text;
                string companyName = FilesHelper.CleanCompanyName(workSheet.Cells[row, 1].Text);
                object arg = row;

            }
        }
    }
}
