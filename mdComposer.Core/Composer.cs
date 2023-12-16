using System.Text;
using System.Text.RegularExpressions;

namespace MdComposer
{
    public class Composer
    {
        public string Compose(string template, Func<string, string> refContentProvider)
        {
            StringBuilder result = new StringBuilder();

            int pos = 0;
            foreach (Match match in Regex.Matches(template, "```[A-Za-z]*:(.*)\n"))
            {
                var group = match.Groups[1];

                string content = refContentProvider(group.Value);
            
                result.AppendLine(template.Substring(pos, group.Index - pos - 1));
                pos = group.Index + group.Length;
            
                result.Append(content);
            }
            
            result.Append(template.Substring(pos, template.Length - pos));
            
            return result.ToString();
        }
    }   
}
