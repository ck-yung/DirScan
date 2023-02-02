using Dir;

var dirThe = ".";
Console.WriteLine($"This is {Path.GetFullPath(dirThe)}");
foreach (var nameThe in Scan.ListFile(dirThe))
{
    Console.WriteLine(nameThe);
}
