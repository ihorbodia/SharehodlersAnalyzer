using HtmlAgilityPack;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShareholdersAnalyzer
{
	public class ShareholderAnalyzerLogic
	{

		String excelFileName;
		String excelFilePath;
		ExcelPackage p;

		List<Task> tasks;

		public ShareholderAnalyzerLogic(string filePath)
		{
			if (filePath.Contains("xlsx#"))
			{
				Debug.WriteLine("File Error");
			}
			else
			{
				tasks = new List<Task>();
				excelFileName = new FileInfo(filePath).Name;
				excelFilePath = new FileInfo(filePath).FullName;
			}
		}

		public void ProcessFile()
		{
			FileInfo fi = new FileInfo(excelFilePath);
			if (fi.Exists)
			{
				p = new ExcelPackage(fi);
				ExcelWorksheet workSheet = p.Workbook.Worksheets[1];
				DataTable dt = new DataTable();
				var start = workSheet.Dimension.Start.Row + 1;
				var end = workSheet.Dimension.End.Row;
                for (int row = start; row <= end; row++)
                {
                    string name = FilesHelper.CleanName(workSheet.Cells[row, 3].Text);
                    string URL = workSheet.Cells[row, 4].Text;
                    string companyName = FilesHelper.CleanCompanyName(workSheet.Cells[row, 1].Text);
                    if (string.IsNullOrEmpty(name))
                    {
                        break;
                    }
                    object arg = row;
#if DEBUG
                    int rowNum = Convert.ToInt32(row);
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
                    Debug.WriteLine(rowNum);
#else
                    tasks.Add(Task.Factory.StartNew(new Action<object>((argValue) =>
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

		private bool? IsFF(HtmlDocument htmlDocument, string name, string companyName)
		{
            Debug.WriteLine(name);
			var shareholdersTable = htmlDocument.DocumentNode.SelectNodes("//table[@class='nfvtTab linkTabBl']")
				.FirstOrDefault(x => x.Attributes.Count > 5);

            var managersTable = htmlDocument.DocumentNode.SelectNodes("//table[@class='nfvtTab linkTabBl']")
                .FirstOrDefault(x => x.Attributes.Count < 6)
                .ChildNodes.Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "td"));

            IEnumerable<string> data = null;
            if (shareholdersTable != null)
			{
				data = shareholdersTable.ChildNodes.Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
				.Select(x => x.ChildNodes.Where(y => y.Name == "td").FirstOrDefault().InnerText.Trim());
			}
            if (!isSiteContainsName(data, name))
            {
                return false;
            }
            var linkToSummary = shareholdersTable.ChildNodes
                .Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .Where(x => x.FirstChild.Attributes["class"].Value == "nfvtL")
                .Where(x => x.FirstChild.FirstChild.Name == "a")
                .Where(x => x.InnerText.Trim().Contains(name))
                .Select(n => n.FirstChild.FirstChild.Attributes["href"].Value).FirstOrDefault();
            if (linkToSummary == null)
            {
                return true;
            }
            var doc = WebHelper.GetPageSummaryData(name, linkToSummary).GetAwaiter().GetResult().DocumentNode;
            var htmlPositionsTable = doc.SelectNodes("//table[@class='tabElemNoBor overfH']")
                .FirstOrDefault(x => x.InnerText.Contains("Current positions"));

            if (htmlPositionsTable == null)
            {
                return false;
            }
            var currentPositionCompanies = htmlPositionsTable
                .ChildNodes
                .FirstOrDefault(x => x.Name == "tr" && x.LastChild.Name == "td")
                .FirstChild
                .FirstChild
                .FirstChild
                .ChildNodes
                .Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .Select(x => x.ChildNodes.Where(y => y.Name == "td").ToList());

            string jobTitle = string.Empty;
            foreach (var positionRow in currentPositionCompanies)
            {
                if (FilesHelper.CleanCompanyName(positionRow[0].InnerText.Trim()).Equals(companyName))
                {
                    jobTitle = positionRow[1].InnerText.Trim();
                }
            }

            if (managersTable.Any(x => x.InnerText.Equals(jobTitle)))
            {
                return null;
            }
            else
            {
                return false;
            }
        }

		private bool isSiteContainsName(IEnumerable<string> data, string name)
		{
			bool result = false;
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			if (data != null)
			{
				foreach (var item in data)
				{
					if (item.ToUpper().Contains(name.ToUpper()))
					{
						return true;
					}
				}
			}
			return result;
		}
	}
}
