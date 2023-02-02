namespace Dir;

public static class Scan
{
    static public IEnumerable<string> ListFile(string path)
    {
        return Directory.GetFiles(path);
    }
}
