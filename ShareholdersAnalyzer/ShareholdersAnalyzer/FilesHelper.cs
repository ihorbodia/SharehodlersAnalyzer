using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareholdersAnalyzer
{
	public static class FilesHelper
	{
		public static string SelectFile()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			string selectedFileName = string.Empty;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				selectedFileName = openFileDialog.FileName;
			}
			else
			{
				selectedFileName = string.Empty;
			}
			return selectedFileName;
		}

        public static string CleanCompanyName(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            string result = string.Empty;
            string newChar = string.Empty;
            data =
                data.Replace("Inc.", newChar)
                    .Replace("Ltd.", newChar)
                    .Replace("Holdings", newChar)
                    .Replace("Holding", newChar)
                    .Replace("Group", newChar)
                    .Replace("Plc", newChar)
                    .Replace(",", newChar)
                    .Replace(".", newChar);
            var res = data.Trim().Split(' ').ToList().Where(x => x.Length > 1);
            var resData = string.Join(" ", res);
            return resData;
        }

        public static string CleanName(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            string result = string.Empty;
            string newChar = string.Empty;
            data =
                data.Replace("MBA", newChar)
                    .Replace("PhD", newChar)
                    .Replace("CPA", newChar)
                    .Replace("Jr", newChar)
                    .Replace("Sr", newChar)
                    .Replace("MD", newChar)
                    .Replace("family", newChar)
                    .Replace("CFA", newChar)
                    .Replace(",", newChar)
                    .Replace(".", newChar);
            data = Regex.Replace(data, @"\s[I]{1,}\s", string.Empty);
            data = Regex.Replace(data, @"(\s)([IV]{1,})(\b|\s)", string.Empty);
            var res = data.Trim().Split(' ').ToList().Where(x => x.Length > 1);
            var resData = string.Join(" ", res);
            return resData;
        }
    }
}
