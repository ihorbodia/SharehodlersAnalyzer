using HtmlAgilityPack;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShareholdersAnalyzer
{
    public class ShareholderAnalyzerLogic
    {
        string excelFilePath;
        ExcelPackage p;

        List<string> summaryTerms;
        List<Task> tasks;

        public ShareholderAnalyzerLogic(string filePath, string termsFilePath)
        {
            if (filePath.Contains("xlsx#"))
            {
                Debug.WriteLine("File Error");
            }
            else
            {
                tasks = new List<Task>();
                summaryTerms = new List<string>();

                this.excelFilePath = new FileInfo(filePath).FullName;
            }
        }

        public void ProcessFile()
        {
            FileInfo excelFi = new FileInfo(excelFilePath);
            if (excelFi.Exists)
            {
                p = new ExcelPackage(excelFi);
                ExcelWorksheet workSheet = p.Workbook.Worksheets[1];
                DataTable dt = new DataTable();
                var start = workSheet.Dimension.Start.Row + 1;
                var end = workSheet.Dimension.End.Row;
                for (int row = start; row <= end; row++)
                {
                    string familyName = FilesHelper.GetFamilyName(workSheet.Cells[row, 3].Text);
                    string middleName = FilesHelper.GetMiddleName(workSheet.Cells[row, 3].Text);
                    string URL = workSheet.Cells[row, 4].Text;
                    object arg = row;
#if DEBUG
                    int rowNum = Convert.ToInt32(row);
                    var htmlDocument = WebHelper.GetPageData(URL);
                    bool? result = IsNFF(htmlDocument, familyName, middleName);
                    if (result == true)
                    {
                        workSheet.Cells[rowNum, 7].Value = "NFF";
                    }
                    if (result == false)
                    {
                        workSheet.Cells[rowNum, 7].Value = "FF";
                    }
                    Debug.WriteLine(rowNum);
#else
                    tasks.Add(Task.Factory.StartNew(new Action<object>((argValue) =>
                    {
						try
						{
							int rowNum = Convert.ToInt32(argValue);
							var htmlDocument = WebHelper.GetPageData(name, URL);
							bool? result = IsFF(htmlDocument, name, companyName);
							if (result == true)
							{
								workSheet.Cells[rowNum, 6].Value = "FF";
							}
							if (result == false)
							{
								workSheet.Cells[rowNum, 6].Value = "NFF";
							}
							if (result == null)
							{
								workSheet.Cells[rowNum, 6].Value = "To be checked";
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex);
						}
                       
                    }), arg));
#endif
                }
            }
        }

        public void SaveFile()
        {
            Task.WaitAll(tasks.ToArray());
            p.Save();
            p.Dispose();
        }

        private bool? IsNFF(HtmlDocument htmlDocument, string familyName, string middleName)
        {
            if (string.IsNullOrEmpty(familyName) && string.IsNullOrEmpty(middleName))
            {
                return null;
            }

            IEnumerable<HtmlNode> managersTable = null;
            try
            {
                managersTable = htmlDocument.DocumentNode.SelectNodes("//table[@class='nfvtTab linkTabBl']")
                    .FirstOrDefault(x => x.Attributes.Count < 6)?
                    .ChildNodes.Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                    .Select(x => x.FirstChild);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var result = managersTable.FirstOrDefault(x => 
                (familyName.ToUpper() == x.InnerText.ToUpper()) || 
                (middleName.ToUpper() == x.InnerText.ToUpper()));

            return result != null;
        }
    }
}
