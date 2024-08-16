using System.Numerics;
using UnityEngine;

public class CurrencyFormatter : MonoBehaviour
{
    private static readonly string[] Suffixes;

    static CurrencyFormatter()
    {
        Suffixes = new string[26];
        for (var i = 0; i < 26; i++)
        {
            Suffixes[i] = ((char)('A' + i - 1)).ToString();
        }
    }

    public static string FormatBigInteger(BigInteger value)
    {
        if (value < 0) return "?";
        if (value < 1000) return value.ToString();
        
        var suffixIndex = 0;
        var decimalValue = (decimal)value;

        while (decimalValue >= 1000 && suffixIndex < Suffixes.Length - 1)
        {
            decimalValue /= 1000;
            suffixIndex++;
        }
        
        return decimalValue.ToString("0.0") + Suffixes[suffixIndex];
    }
}