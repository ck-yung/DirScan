using Dir;

if (args.Length > 0)
{
    Directory.SetCurrentDirectory(args[0]);
}
Console.WriteLine($"This is {Directory.GetCurrentDirectory()}");

Console.WriteLine();
Console.WriteLine($"-- list first 5 files --");
foreach (var nameThe in Scan.ListFiles("", exclDir:".git").Take(5))
{
    Console.WriteLine(nameThe);
}

Console.WriteLine();
Console.WriteLine($"-- list '*.txt' files --");
var likeTxtFile = "*.txt".ToWildMatch();
foreach (var nameThe in Scan.ListFiles("", exclDir: ".git")
    .Where((it) => likeTxtFile(it)))
{
    Console.WriteLine(nameThe);
}

Console.WriteLine();
Console.WriteLine(  $"-- list '*cs' and '*.txt' files --");
var wildsThe = new string[] { "*.cs", "*.txt" };
Func<string, bool> likeCsTxtFiles = wildsThe.ToWildMatch();
foreach (var nameThe in Scan.ListFiles("", exclDir: ".git")
    .Where((it) => likeCsTxtFiles(it)))
{
    Console.WriteLine(nameThe);
}
/* Remark to 'likeCsTxtFiles'
 * DO NOT PUT 'ToWildMatch' in 'Where'
 *
 * Reason:
 *
 * 1. 'ToWildMatch' is called before related 'Where':
 * The related 'Regex' objects are created once.
 *
 * 2. 'ToWildMatch' is called in 'Where':
 * 'Regex' objects are created each time for each checking.
 */

Console.WriteLine();
Console.WriteLine("-- extra test 1 --");
bool CheckDir(string dirbase, string dirthe)
{
    var dirname = Path.Combine(dirbase, dirthe);
    if (!Directory.Exists(dirname)) return false;
    if ((dirthe.Length > 0) && (dirthe[0] == '.')) return false;
    var info = new DirectoryInfo(dirname);
    var timeDiff = DateTime.Now.Subtract(info.LastWriteTime);
    var rtn = timeDiff >= TimeSpan.FromHours(2);
    return rtn;
}

foreach (var nameThe in Scan.ListFiles("", filterDirname: CheckDir)
    )
{
    Console.WriteLine(nameThe);
}

Console.WriteLine("-- Ok --");
