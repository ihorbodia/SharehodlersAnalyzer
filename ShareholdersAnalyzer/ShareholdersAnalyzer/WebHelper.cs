﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareholdersAnalyzer
{
	public static class WebHelper
	{
        public static Task ForEachAsync<T>(this IEnumerable<T> sequence, Func<T, Task> action)
        {
            return Task.WhenAll(sequence.Select(action));
        }
        public async static Task<HtmlDocument> GetPageData(string URL)
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
			Encoding iso = Encoding.GetEncoding("iso-8859-1");
			HtmlWeb web = new HtmlWeb()
			{
				AutoDetectEncoding = false,
				OverrideEncoding = iso,
			};
			HtmlDocument htmlDoc = null;
			try
			{
                htmlDoc = await web.LoadFromWebAsync(html, iso);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return htmlDoc;
		}

        public static HtmlDocument GetPageSummaryData(string name, string URL)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            string html = string.Empty;
            if (!URL.Contains("https://www.marketscreener.com"))
            {
                html = "https://www.marketscreener.com" + URL;
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
            HtmlDocument htmlDoc = null;
            try
            {
                htmlDoc = web.Load(html);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return htmlDoc;
        }
    }
}
