using System.Text;

namespace DiscordBot;

public static partial class Extra
{
    /// <summary>
    /// Removes illegal words from a string.
    /// </summary>
    /// <param name="txt"></param>
    public static string RemoveIllegal(this string txt, string[] illegalWords)
    {
        StringBuilder sb = new();
        string[] splittedTxt = txt.Split(' ');
        Span<string> illegalSpan = illegalWords;

        for (int i = 0; i < illegalSpan.Length; i++)
        {
            for (int j = 0; j < splittedTxt.Length; j++)
            {
                if (splittedTxt[j] == illegalSpan[i])
                    continue; // There may be more forward into the string.

                sb.Append(splittedTxt[j]).Append(' ');
            }
        }
        txt = sb.ToString();
        return txt;
    }

    /// <summary>
    /// Verifies if there are illegal words in a string.
    /// </summary>
    /// <param name="txt"></param>
    public static bool HasIllegal(this string txt, string[] illegalWords)
    {
        Span<string> illegalSpan = illegalWords;
        var splitTxt = txt.Split(' ');
        for (int i = 0; i < illegalSpan.Length; i++)
        {
            if (splitTxt.Contains(illegalSpan[i]))
                return true;
        }
        return false;
    }
}