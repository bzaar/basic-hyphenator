using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morpher.Russian;

/// <summary>
/// Реализует самые базовые правила деления слов для переноса на другую строку
/// без учета морфемного строения слов:
/// - В каждой части должна быть хотя бы одна гласная.
/// - Нельзя отрывать гласные, а также буквы ь и ъ от предшествующей согласной.
/// - Нельзя отрывать букву й от предшествующей гласной.
/// - Нельзя оставлять одну букву: а-стра, астр-а (можно ли длинноше-ее?)
/// http://new.gramota.ru/biblio/readingroom/rules/141-perenos
/// </summary>
public static class BasicHyphenator
{
    public static string Hyphenate(string lowercaseRussianWord, char hyphen = '-')
    {
        if (string.IsNullOrEmpty(lowercaseRussianWord))
        {
            throw new ArgumentException("Parameter is null or empty.", nameof(lowercaseRussianWord));
        }

        IEnumerable<int> hyphenPositions = GetHyphenPositions(lowercaseRussianWord);
        return InsertHyphensAtPositions(lowercaseRussianWord, hyphenPositions, hyphen);
    }

    private static string InsertHyphensAtPositions(string word, IEnumerable<int> hyphenPositions, char hyphen)
    {
        var sb = new StringBuilder(word);

        foreach (int i in hyphenPositions.Reverse())
        {
            sb.Insert(i, hyphen);
        }

        return sb.ToString();
    }

    /// <returns>
    /// Последовательность позиций переносов:
    /// например, [1, 3, 5] означает возможность переноса
    /// после первой, третьей и пятой букв.
    /// </returns>
    private static IEnumerable<int> GetHyphenPositions(string word)
    {
        const int start = 2; // нельзя оставлять одну букву в начале
        int end = word.Length-1; // нельзя оставлять одну букву в конце
        
        for (int i = start; i < end; i++)
        {
            if (ContainsVowel(word[..i]) && ContainsVowel(word[i..]))
            {
                char nextc = word[i];
                char prevc = word[i-1];
                
                if (IsConsonant(prevc) && (IsVowel(nextc) || IsSign(nextc)))
                {
                    continue; // нельзя отрывать гласную и буквы Ъ, Ь от предшествующей согласной
                }

                if (IsVowel(prevc) && nextc == 'й')
                {
                    continue; // Нельзя отрывать букву й от предшествующей гласной.
                }
                
                yield return i; // перенос в этой позиции разрешен
            }
        }
    }

    private static bool IsSign(char c) => "ъь".Contains(c);

    private static bool IsConsonant(char c) => !IsVowel(c) && !IsSign(c);

    private static bool IsVowel(char c) => vowels.Contains(c);

    private static bool ContainsVowel(string s) => s.IndexOfAny(vowels) > -1;

    static readonly char[] vowels = "аоуэыяёюеи".ToCharArray();
}