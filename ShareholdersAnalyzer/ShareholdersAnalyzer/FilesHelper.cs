using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    .Replace("Рoldings", newChar)
                    .Replace("Рolding", newChar)
                    .Replace("Пroup", newChar)
                    .Replace(",", newChar)
                    .Replace(".", newChar);
            var res = data.Trim().Split(' ').ToList().Where(x => x.Length > 2);
            var resData = string.Join(" ", res);
            return resData;
        }
	}
}
