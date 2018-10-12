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
                        Console.WriteLine(companyName);
                        Console.WriteLine(name);
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
            if (string.IsNullOrEmpty(companyName) || string.IsNullOrEmpty(name))
            {
                return null;
            }
            name = name.ToUpper();
            companyName = companyName.ToUpper();
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
                return true;
            }
            var linkToSummary = shareholdersTable.ChildNodes
                .Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .Where(x => x.FirstChild.Attributes["class"].Value == "nfvtL")
                .Where(x => x.FirstChild.FirstChild.Name == "a")
                .Where(x => x.InnerText.Trim().ToUpper().Contains(name))
                .Select(n => n.FirstChild.FirstChild.Attributes["href"].Value).FirstOrDefault();
            if (linkToSummary == null)
            {
                return true;
            }
            var doc = WebHelper.GetPageSummaryData(name, linkToSummary).DocumentNode;
            var personPageTables = doc.SelectNodes("//table[@class='tabElemNoBor overfH']");

            var htmlPositionsTable = personPageTables.FirstOrDefault(x => x.InnerText.Contains("Current positions"));

            IEnumerable<IList<HtmlNode>> currentPositionCompanies = null;
            string jobTitle = string.Empty;
            if (htmlPositionsTable != null)
            {
                currentPositionCompanies = htmlPositionsTable
                .ChildNodes
                .FirstOrDefault(x => x.Name == "tr" && x.LastChild.Name == "td")
                .FirstChild
                .FirstChild
                .FirstChild
                .ChildNodes
                .Where(x => x.Name == "tr" && x.PreviousSibling.Name == "tr")
                .Select(x => x.ChildNodes.Where(y => y.Name == "td").ToList());
            }
            
            if (currentPositionCompanies == null)
            {
                var summaryText = personPageTables.FirstOrDefault(x => x.InnerText.Contains("Summary")).InnerText.ToUpper();
                if (summaryText.IndexOf(companyName) != summaryText.LastIndexOf(companyName) && summaryText.IndexOf(companyName) != -1)
                {
                    return null;
                }
                else if (summaryText.Contains(companyName))
                {
                    if (isPresentedInSummary(summaryText, companyName))
                    {
                        return false;
                    }
                    else
                    {
                        jobTitle = FoundJobTitle(summaryText, companyName);
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                foreach (var positionRow in currentPositionCompanies)
                {
                    if (FilesHelper.CleanCompanyName(positionRow[0].InnerText.Trim()).Equals(companyName))
                    {
                        jobTitle = positionRow[1].InnerText.Trim();
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(jobTitle))
            {
                return true;
            }
            if (managersTable == null)
            {
                return false;
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
        
        private string FoundJobTitle(string summaryText, string companyName)
        {
            Regex regex = new Regex("IS (.*) AT " + companyName);
            var v = regex.Match(summaryText);
            if (v.Groups[1] != null)
            {
                return v.Groups[1].ToString();
            }
            return string.Empty;
        }

        private bool isPresentedInSummary(string summaryText, string companyName)
        {
            return summaryText.Contains("is a Director at".ToUpper() + companyName) 
                || summaryText.Contains("is an independent Director at".ToUpper() + companyName) 
                || summaryText.Contains("is on the board of Directors at".ToUpper() + companyName) 
                || summaryText.Contains("is on the board at".ToUpper() + companyName) 
                || summaryText.Contains("founded".ToUpper() + companyName) 
                || summaryText.Contains("founder at".ToUpper() + companyName);
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
