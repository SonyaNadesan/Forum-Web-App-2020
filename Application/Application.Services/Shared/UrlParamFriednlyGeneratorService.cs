using System.Text.RegularExpressions;

namespace Application.Services.Shared
{
    public class UrlParamFriednlyGeneratorService
    {
        public static string GetTextForParamUse(string text)
        {
            text = text.ToLower();
            text = text.Replace("&", "and");
            text = text.Replace("_", " ").Trim();
            text = Regex.Replace(text, @"\W+", "-");
            return text;
        }
    }
}
