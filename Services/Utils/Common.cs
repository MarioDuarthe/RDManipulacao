using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;


namespace Services.Utils
{
    public class Common
    {
        public static string LimpaCaracteresEspeciais(string texto)
        {
            try
            {
                if (string.IsNullOrEmpty(texto)) return string.Empty;
                texto = RemoveAcentos(texto);
                string expressaoPermitida = "[^0-9A-Za-z ]";
                return Regex.Replace(texto, expressaoPermitida, string.Empty, RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)  
            {
                return texto;
            }
        }
        private static string RemoveAcentos(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
    }
}
