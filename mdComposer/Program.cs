using System.Text;
using System.Text.RegularExpressions;

internal static class Program
{
    private static int Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("mdComposer template result [params]");
            return 1;
        }

        FileInfo template = new FileInfo(args[0]);
        if (!template.Exists)
        {
            Console.WriteLine($"File {args[0]} not found");
            return 1;
        }

        FileInfo result = new FileInfo(args[1]);
        
        Directory.SetCurrentDirectory(template.DirectoryName!);

        Compose(template, result);

        return 0;
    }

    private static void Compose(FileInfo templateName, FileInfo resultName)
    {
        Console.WriteLine($"Composing: '{templateName}' -> '{resultName}'");

        StringBuilder result = new StringBuilder();
        
        string template = File.ReadAllText(templateName.FullName);

        int pos = 0;
        foreach (Match match in Regex.Matches(template, "```[A-Za-z]*:(.*)\n"))
        {
            var group = match.Groups[1];
            Console.Write(group.Value);
            
            result.AppendLine(template.Substring(pos, group.Index - pos - 1));
            pos = group.Index + group.Length;
            
            result.Append(File.ReadAllText(group.Value));
            
            Console.WriteLine(" ... OK");
        }
        result.Append(template.Substring(pos, template.Length - pos));
        
        File.WriteAllText(resultName.FullName, result.ToString());
        
        Console.WriteLine("Done.");
    }
}