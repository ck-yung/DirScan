using Dir;

if (args.Length > 0)
{
    Directory.SetCurrentDirectory(args[0]);
}
Console.WriteLine($"This is {Directory.GetCurrentDirectory()}");
foreach (var nameThe in Scan.ListFiles("").Take(12))
{
    Console.WriteLine(nameThe);
}

var dirToBeExcl = "?i*";
Console.WriteLine($"-- excl '{dirToBeExcl}' --");
foreach (var nameThe in Scan.ListFiles("",
    exclDirWild: new string[] { dirToBeExcl })
    .Take(12))
{
    Console.WriteLine(nameThe);
}

Console.WriteLine("-- extra test --");
bool CheckDir(string dirbase, string dirthe)
{
    var dirname = Path.Combine(dirbase, dirthe);
    if (!Directory.Exists(dirname)) return false;
    var info = new DirectoryInfo(dirname);
    var timeDiff = DateTime.Now.Subtract(info.LastWriteTime);
    var rtn = timeDiff < TimeSpan.FromHours(2);
    return rtn;
}

foreach (var nameThe in Scan.ListFiles("", filterDirname: CheckDir)
    .Take(12))
{
    Console.WriteLine(nameThe);
}

Console.WriteLine("-- Ok --");
