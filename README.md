# DirScan
Scan file recursively [enumerable]

## Examples
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
bool CheckDir(string dirname)
{
    if (!Directory.Exists(dirname)) return false;
    var info = new DirectoryInfo(dirname);
    var timeDiff = DateTime.Now.Subtract(info.LastWriteTime);
    return timeDiff < TimeSpan.FromHours(2);
}

foreach (var nameThe in Scan.ListFiles("", filterDirname: CheckDir)
    .Take(12))
{
    Console.WriteLine(nameThe);
}
```

2023 Yung, Chun Kau<br/>
yung.chun.kau@gmail.com
