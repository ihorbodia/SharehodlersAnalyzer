using HtmlAgilityPack;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

        public async Task ProcessFileAsync()
        {
            FileInfo excelFi = new FileInfo(excelFilePath);
            if (excelFi.Exists)
            {
                p = new ExcelPackage(excelFi);
                ExcelWorksheet workSheet = p.Workbook.Worksheets[1];
                var start = workSheet.Dimension.Start.Row + 1;
                var end = workSheet.Dimension.End.Row;

                await Enumerable.Range(start, end).ForEachAsync(async item =>
                {
                    string familyName = FilesHelper.GetFamilyName(workSheet.Cells[item, 3].Text);
                    string middleName = FilesHelper.GetMiddleName(workSheet.Cells[item, 3].Text);
                    string URL = workSheet.Cells[item, 4].Text;
                    if (string.IsNullOrEmpty(URL))
                    {
                        return;
                    }
                    await StartProcess(workSheet, item, familyName, middleName, URL);
                });
            }
        }

        private async Task StartProcess(ExcelWorksheet workSheet, object row, string familyName, string middleName, string URL)
        {
            int rowNum = Convert.ToInt32(row);
            var htmlDocument = GetPageData(URL);
            bool? result = IsNFF(htmlDocument, familyName, middleName);
            if (result == true)
            {
                workSheet.Cells[rowNum, 6].Value = "NFF";
            }
            if (result == false)
            {
                workSheet.Cells[rowNum, 6].Value = "FF";
            }
            Debug.WriteLine(rowNum);
        }

        public void SaveFile()
        {
            p.Save();
            p.Dispose();
        }

        private bool? IsNFF(HtmlDocument htmlDocument, string familyName, string middleName)
        {
            if (string.IsNullOrEmpty(familyName) && string.IsNullOrEmpty(middleName))
            {
                return null;
            }
            if (htmlDocument == null)
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

            if (managersTable == null)
            {
                return false;
            }

            bool isTableContainsFamilyName(string familyNameFromXlsx)
            {
                if (string.IsNullOrEmpty(familyNameFromXlsx))
                {
                    return false;
                }
                return managersTable.Any(x => FilesHelper.GetFamilyName(x.InnerText).ToUpper().Equals(familyNameFromXlsx.ToUpper()));
            }
            bool isTableContainsMiddleName(string middleNameFromXlsx)
            {
                if (string.IsNullOrEmpty(middleNameFromXlsx))
                {
                    return false;
                }
                return managersTable.Any(x => FilesHelper.GetMiddleName(x.InnerText).ToUpper().Equals(middleNameFromXlsx.ToUpper()));
            }

           return isTableContainsFamilyName(familyName) || isTableContainsMiddleName(middleName);
        }

        public HtmlDocument GetPageData(string URL)
        {
            string html = string.Empty;
            if (!URL.Contains("https://www."))
            {
                html = "https://www." + URL;
            }
            else
            {
                html = URL;
            }
            var htmlDoc = new HtmlDocument();
            try
            {
                using (var htppClient = new HttpClient())
                {
                    var result = htppClient.GetStringAsync(html);
                    htmlDoc.LoadHtml(result.Result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return htmlDoc;
        }
    }
}
