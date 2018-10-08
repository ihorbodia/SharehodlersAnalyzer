using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ShareholdersAnalyzer
{
	public static class WebHelper
	{
		public static async Task<HtmlDocument> GetPageData(string name, string URL)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			string html = string.Empty;
			if (!URL.Contains("https://www."))
			{
				html = "https://www." + URL;
			}
			else
			{
				html = URL;
			}

			Encoding iso = Encoding.GetEncoding("iso-8859-1");
			HtmlWeb web = new HtmlWeb()
			{
				AutoDetectEncoding = false,
				OverrideEncoding = iso,
			};
			HtmlAgilityPack.HtmlDocument htmlDoc = null;
			try
			{
				htmlDoc = await web.LoadFromWebAsync(html);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			if (htmlDoc == null)
			{
				return null;
			}
			return null;
		}
	}
}
