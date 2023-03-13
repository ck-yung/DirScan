namespace Dir;
using static Dir.Helper;

public static class Scan
{
    /// <summary>
    /// Scan file recursively
    /// </summary>
    /// <param name="path">Base directory</param>
    /// <returns>All filename under the base directory</returns>
    /// <remarks>See <link>https://github.com/ck-yung/DirScan/blob/main/README.md</link></remarks>
    static public IEnumerable<string> ListFiles(string path)
    {
        return ListFiles(path, Always<string, string>.True);
    }

    /// <summary>
    /// Scan file recursively
    /// </summary>
    /// <param name="path">Base directory</param>
    /// <param name="exclDir">excl dir wild</param>
    /// <param name="caseSensitive">true if Case-Sensitvie</param>
    /// <returns>All filename under the base directory excluding to 'exclDirWild'</returns>
    /// <remarks>See <link>https://github.com/ck-yung/DirScan/blob/main/README.md</link></remarks>
    static public IEnumerable<string> ListFiles(string path,
        string exclDir, bool caseSensitive = false)
    {
        Func<string, string, bool> allowDir = Always<string, string>.True;
        if (!string.IsNullOrEmpty(exclDir))
        {
            var wildThe = exclDir.ToWildMatch(caseSensitive);
            allowDir = (_, dirthe) => false == wildThe(dirthe);
        }
        return ListFiles(path, allowDir);
    }

    /// <summary>
    /// Scan file recursively
    /// </summary>
    /// <param name="path">Base directory</param>
    /// <param name="exclDirWild">array/list of excl dir wild</param>
    /// <param name="caseSensitive">true if Case-Sensitvie</param>
    /// <returns>All filename under the base directory excluding to 'exclDirWild'</returns>
    /// <remarks>See <link>https://github.com/ck-yung/DirScan/blob/main/README.md</link></remarks>
    static public IEnumerable<string> ListFiles(string path,
        IEnumerable<string> exclDirWild, bool caseSensitive = false)
    {
        var wildFuncs = exclDirWild
            .Select((it) => it.ToWildMatch(caseSensitive))
            .ToArray();
        Func<string, string, bool> allowDir = (exclDirWild.Count() == 0)
            ? Always<string, string>.True
            : (_, it) => wildFuncs.All((check) => false == check(it));
        return ListFiles(path, allowDir);
    }

    /// <summary>
    /// Scan file recursively
    /// </summary>
    /// <param name="path">Base directory</param>
    /// <param name="filterDirname">Function (predicate lambda) to filter (dirbase, dirname)</param>
    /// <returns>All filename under the base directory satifying 'filterDirname'</returns>
    /// <remarks>See <link>https://github.com/ck-yung/DirScan/blob/main/README.md</link></remarks>
    static public IEnumerable<string> ListFiles(string path,
        Func<string, string, bool> filterDirname)
    {
        if (path == null) path = string.Empty;

        if (path.EndsWith(":") || string.IsNullOrEmpty(path))
        {
            path += ".";
        }

        if (!path.EndsWith(Path.DirectorySeparatorChar))
        {
            path += Path.DirectorySeparatorChar;
        }

        IEnumerable<string> ImpListFiles(string dirThe)
        {
            var enumFile = SafeGetFileEnumerator(dirThe);
            while (SafeMoveNext(enumFile))
            {
                var currentFilename = SafeGetCurrent(enumFile);
                if (string.IsNullOrEmpty(currentFilename)) continue;
                yield return currentFilename;
            }

            var enumDir = SafeGetDirectoryEnumerator(dirThe);
            while (enumDir.MoveNext())
            {
                var currentDirname = SafeGetCurrent(enumDir);
                if (string.IsNullOrEmpty(currentDirname)) continue;
                if (false == filterDirname(
                    Path.GetDirectoryName(currentDirname) ?? string.Empty,
                    Path.GetFileName(currentDirname) ?? string.Empty))
                {
                    continue;
                }
                foreach (var pathThe in ImpListFiles(currentDirname))
                {
                    yield return pathThe;
                }
            }
        }

        var pathLen = path.Length;
        foreach (var filenameThe in ImpListFiles(path))
        {
            yield return filenameThe.Substring(pathLen);
        }
    }
}
