using HtmlAgilityPack;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
                    string name = workSheet.Cells[row, 3].Text;
                    string URL = workSheet.Cells[row, 4].Text;
                    string companyName = FilesHelper.CleanCompanyName(workSheet.Cells[row, 1].Text);
                    if (string.IsNullOrEmpty(name))
                    {
                        break;
                    }
                    object arg = row;
#if DEBUG
                    int rowNum = Convert.ToInt32(row);
                    var htmlDocument = WebHelper.GetPageData(name, URL).GetAwaiter().GetResult();
                    if (IsFF(htmlDocument, name, companyName))
                    {
                        workSheet.Cells[rowNum, 6].Value = "FF";
                    }
                    else
                    {
                        workSheet.Cells[rowNum, 6].Value = "NFF";
                    }
                    Debug.WriteLine(rowNum);
#else
                    tasks.Add(Task.Factory.StartNew(new Action<object>(async (argValue) =>
                    {
                        int rowNum = Convert.ToInt32(argValue);
                        var htmlDocument = await WebHelper.GetPageData(name, URL);
                        if (IsFF(htmlDocument, name))
                        {
                            workSheet.Cells[rowNum, 6].Value = "FF";
                        }
                        else
                        {
                            workSheet.Cells[rowNum, 6].Value = "NFF";
                        }
                        Debug.WriteLine(rowNum);
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

		private bool IsFF(HtmlDocument htmlDocument, string name, string companyName)
		{
			var table = htmlDocument.DocumentNode.SelectNodes("//table[@class='nfvtTab linkTabBl']")
				.FirstOrDefault(x => x.Attributes.Count > 5);
			IEnumerable<string> data = null;
            if (table != null)
			{
				data = table.ChildNodes.Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
				.Select(x => x.ChildNodes.Where(y => y.Name == "td").FirstOrDefault().InnerText.Trim());
			}
            if (!isSiteContainsName(data, name))
            {
                return false;
            }
            var linkToSummary = table.ChildNodes
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

            var currentPositionCompanies =
                doc.SelectNodes("//table[@class='tabElemNoBor overfH']")
                .Single(x => x.InnerText.Contains("Current positions"))
                .ChildNodes
                .Single(x => x.Name == "tr" && x.LastChild.Name == "td")
                .FirstChild
                .FirstChild
                .FirstChild
                .ChildNodes
                .Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .Select(x => x.ChildNodes.Where(y => y.Name == "td").FirstOrDefault().InnerText.Trim());
            string test;
            foreach (var item in currentPositionCompanies)
            {
                if (FilesHelper.CleanCompanyName(item).Contains(companyName))
                {
                    test = item;
                }
            }

            return false;
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
