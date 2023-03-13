# DirScan
Scan file recursively [enumerable]

## Examples
### [See demo](https://github.com/ck-yung/DirScan/blob/main/DemoCs/Program.cs)

* To get all files on current directory,
```
using Dir;
foreach (var nameThe in Scan.ListFiles(""))
{
    Console.WriteLine(nameThe);
}
```

* To get all files on ```data``` but excluding ```Temp*``` and ```Tmp*```,
```
using Dir;
foreach (var nameThe in Scan.ListFiles("data",
exclDirWild: new string[] { "temp*", "tmp*" }))
{
    Console.WriteLine(nameThe);
}
```

* Complex example,
```
using Dir;
bool CheckDir(string dirbase, string dirthe)
{
    var dirname = Path.Combine(dirbase, dirthe);
    if (!Directory.Exists(dirname)) return false;
    var info = new DirectoryInfo(dirname);
    var timeDiff = DateTime.Now.Subtract(info.LastWriteTime);
    var rtn = timeDiff < TimeSpan.FromHours(2);
    return rtn;
}

foreach (var nameThe in Scan.ListFiles("", filterDirname: CheckDir))
{
    Console.WriteLine(nameThe);
}
```

<p>2023 Yung, Chun Kau</p>
yung.chun.kau@gmail.com
