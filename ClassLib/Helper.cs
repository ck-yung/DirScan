using System.Text;
using System.Text.RegularExpressions;

namespace Dir;

internal class Always<T>
{
    static public readonly Func<T, bool> True = (_) => true;
}

internal class Always<T, T2>
{
    static public readonly Func<T, T2, bool> True = (_, _) => true;
}

static internal class Wild
{
    static internal StringComparer StringComparer
    { get; private set; } = StringComparer.OrdinalIgnoreCase;

    static internal Func<string, bool, Regex> MakeRegex
    { get; private set; } = (it, flag) => new Regex(it,
        flag ? RegexOptions.None : RegexOptions.IgnoreCase);

    static internal Func<string, string> ToRegexText
    { get; private set; } = (it) =>
    {
        var regText = new StringBuilder("^");
        regText.Append(it
            .Replace(@"\", @"\\")
            .Replace("^", @"\^")
            .Replace("$", @"\$")
            .Replace(".", @"\.")
            .Replace("?", ".")
            .Replace("*", ".*")
            .Replace("(", @"\(")
            .Replace(")", @"\)")
            .Replace("[", @"\[")
            .Replace("]", @"\]")
            .Replace("{", @"\{")
            .Replace("}", @"\}")
            ).Append('$');
        return regText.ToString();
    };

    static internal Func<string, bool> ToMatch(string arg,
        bool caseSensitive = false)
    {
        var regThe = MakeRegex(ToRegexText(arg), caseSensitive);
        return (it) => regThe.Match(it).Success;
    }
}

static internal class Helper
{
    #region Call Enumerator Function Safely
    static internal readonly IEnumerator<string> EmptyEnumStrings
        = Enumerable.Empty<string>().GetEnumerator();

    static internal IEnumerator<string> SafeGetFileEnumerator(string dirname)
    {
        try { return Directory.EnumerateFiles(dirname).GetEnumerator(); }
        catch { return EmptyEnumStrings; }
    }

    static internal IEnumerator<string> SafeGetDirectoryEnumerator(string dirname)
    {
        try { return Directory.EnumerateDirectories(dirname).GetEnumerator(); }
        catch { return EmptyEnumStrings; }
    }

    static internal bool SafeMoveNext(IEnumerator<string> it)
    {
        try { return it.MoveNext(); }
        catch { return false; }
    }

    static internal string SafeGetCurrent(IEnumerator<string> it)
    {
        try { return it.Current; }
        catch { return string.Empty; }
    }
    #endregion
}
