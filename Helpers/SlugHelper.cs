using System.Text;
using System.Text.RegularExpressions;
namespace NetNews.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrEmpty(phrase)) return "";

            string str = phrase.ToLower();

            str = Regex.Replace(str, @"[áàạảãâấầậẩẫăắằặẳẵ]", "a");
            str = Regex.Replace(str, @"[éèẹẻẽêếềệểễ]", "e");
            str = Regex.Replace(str, @"[oóòọỏõôốồộổỗơớờợởỡ]", "o");
            str = Regex.Replace(str, @"[uúùụủũưứừựửữ]", "u");
            str = Regex.Replace(str, @"[iíìịỉĩ]", "i");
            str = Regex.Replace(str, @"[yýỳỵỷỹ]", "y");
            str = Regex.Replace(str, @"[đ]", "d");

            str = str.Normalize(NormalizationForm.FormD);
            var chars = str.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark).ToArray();
            str = new string(chars).Normalize(NormalizationForm.FormC);

            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            str = Regex.Replace(str, @"\s+", " ").Trim();

            str = str.Replace(" ", "-");

            str = Regex.Replace(str, @"-+", "-");

            return str;
        }
    }
}