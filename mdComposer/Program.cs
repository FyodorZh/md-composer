using MdComposer;

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
        FileInfo result = new FileInfo(args[1]);
        
        if (!template.Exists)
        {
            Console.WriteLine($"File {args[0]} not found");
            return 1;
        }
        
        Console.WriteLine($"Composing: '{template}' -> '{result}'");

        var composer = new Composer();
        string composition = composer.Compose(
            File.ReadAllText(template.FullName),
            path =>
            {
                Console.WriteLine(path);
                Uri templateUri = new Uri(template.FullName);
                Uri pathUri = new Uri(templateUri, path);
                return File.ReadAllText(pathUri.AbsolutePath);
            });

        File.WriteAllText(result.FullName, composition);
        
        Console.WriteLine("Done.");
        return 0;
    }

    private static void Compose(FileInfo templateName, FileInfo resultName)
    {
        Console.WriteLine($"Composing: '{templateName}' -> '{resultName}'");

        var composer = new Composer();
        string result = composer.Compose(
            File.ReadAllText(templateName.FullName),
            path =>
            {
                Console.WriteLine(path);
                return File.ReadAllText(path);
            });

        File.WriteAllText(resultName.FullName, result);
        
        Console.WriteLine("Done.");
    }
}