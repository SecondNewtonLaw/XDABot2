using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

static class FastIteration
{
    /// <summary>
    /// Iterates through an array fast, very fast.
    /// </summary>
    /// <typeparam name="T">Anything that is able to be in an Array.</typeparam>
    public static void FastIterator<T>(this T[] element, Action<T, int> runInLoop)
    {
        Span<T> spanOfElement = element;
        ref var searchSpace = ref MemoryMarshal.GetReference(spanOfElement);
        for (int i = 0; i < spanOfElement.Length; i++)
        {
            var obj = Unsafe.Add(ref searchSpace, i);
            runInLoop?.Invoke(obj, i);
        }
    }
}