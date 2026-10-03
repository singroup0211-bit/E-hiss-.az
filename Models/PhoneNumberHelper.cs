using System.Linq;
using System.Text.RegularExpressions;

namespace EHisseler.Web.Models
{
    public static class PhoneNumberHelper
    {
        // Operator prefiksləri asan yenilənə bilən array forması
        public static readonly string[] ValidPrefixes = { "10", "50", "51", "55", "60", "70", "77", "99" };

        public static bool TryNormalizeAzNumber(string? input, out string? normalizedWa, out string? normalizedTel)
        {
            normalizedWa = null;
            normalizedTel = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Yalnız rəqəmləri alırıq (boşluq, defis, mötərizə və '+' işarəsi silinir)
            string digits = new string(input.Where(char.IsDigit).ToArray());

            string numberWithoutCode = null;

            if (digits.StartsWith("994") && digits.Length == 12)
            {
                numberWithoutCode = digits.Substring(3);
            }
            else if (digits.StartsWith("0") && digits.Length == 10)
            {
                numberWithoutCode = digits.Substring(1);
            }
            else if (digits.Length == 9)
            {
                numberWithoutCode = digits;
            }

            if (numberWithoutCode == null || numberWithoutCode.Length != 9)
                return false;

            string prefix = numberWithoutCode.Substring(0, 2);
            if (!ValidPrefixes.Contains(prefix))
                return false;

            normalizedWa = "994" + numberWithoutCode;
            normalizedTel = "+994" + numberWithoutCode;
            return true;
        }
    }
}
