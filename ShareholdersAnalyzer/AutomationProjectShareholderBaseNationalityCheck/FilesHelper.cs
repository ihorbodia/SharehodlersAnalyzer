using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutomationProjectShareholderBaseNationalityCheck
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

		public static string SelectFolder()
		{
			string res = string.Empty;
			using (var fbd = new FolderBrowserDialog())
			{
				DialogResult result = fbd.ShowDialog();
				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					res = fbd.SelectedPath;
				}
			}
			return res;
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
                    .Replace("HOLDINGS", newChar)
                    .Replace("LIMITED", newChar)
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
