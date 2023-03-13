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

static public class Wild
{
    static public string ToRegexText(this string text)
    {
        var regText = new StringBuilder("^");
        regText.Append(text
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
    }

    static public Regex MakeRegex(this string text,
        bool caseSensitive = false)
    {
        return new Regex(text, caseSensitive
            ? RegexOptions.None : RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// DO NOT put in LINQ clause.
    /// 'where "*.txt".ToWildMatch()(toBeCheckedText)' is POOR.
    /// </summary>
    static public Func<string, bool> ToWildMatch(this string arg,
        bool caseSensitive = false)
    {
        var regexThe = arg.ToRegexText().MakeRegex(caseSensitive);
        return (it) => regexThe.Match(it).Success;
    }

    /// <summary>
    /// DO NOT put in LINQ clause.
    /// 'where wilds.ToWildMatch()(toBeCheckedText)' is POOR.
    /// </summary>
    static public Func<string, bool> ToWildMatch(this IEnumerable<string> args,
        bool caseSensitive = false)
    {
        var likeCsTxtFileChecking = args
            .Select((it) => it.ToWildMatch(caseSensitive))
            .ToArray();
        return (likeCsTxtFileChecking.Length == 0)
            ? (it) => true
            : (it) => likeCsTxtFileChecking.Any((check) => check(it));
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
