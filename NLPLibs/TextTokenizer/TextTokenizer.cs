using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace TextTokenizer
{
    /// <summary>
    /// Defines Regex'es for the assembly.
    /// </summary>
    internal static class Regexes
    {
        public static readonly Regex Word = new Regex(@"(?:\b(?:\p{L}+(?:-|'))*\p{L}+\b)|(?:\d+)", RegexOptions.Compiled);
        public static readonly Regex Sentence = new Regex(@"((?>[\(\xab`""]*\w[\w\-']*[\xbb\)]*)(?>[,;:]|\s?\u2014)?\s?)+(!|\?!?|\.(\.\.)?)\)?", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        public static readonly Regex WordPunct = new Regex(@"(?:\b(?:\p{L}+(?:-|'))*\p{L}+\b)|(?:\p{P})|(?:\d+)", RegexOptions.Compiled);
    };

    /// <summary>
    /// Class for sentence tokenizing
    /// </summary>
    public class SentenceTokenizer
    {
        /// <summary>
        /// Tokenize text from list of sentences.
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Array of sentences (each sentence is string).</returns>
        public static string[] tokenize(string text)
        {
            return (from Match x in Regexes.Sentence.Matches(text) select x.Value).ToArray();
        }
    };

    /// <summary>
    /// Class for word tokenizing
    /// </summary>
    public class WordTokenizer
    {
        /// <summary>
        /// Tokenize sentence from list of words.
        /// </summary>
        /// <param name="text">Sentence as string.</param>
        /// <returns>List of strings; each string is sentence.</returns>
        public static string[] tokenize(string text)
        {
            return (from Match x in Regexes.Word.Matches(text) select x.Value).ToArray();
        }
    };

    /// <summary>
    /// Class for word and punctuation tokenizing
    /// </summary>
    public class WordPunctuationTokenizer
    {
        /// <summary>
        /// Tokenize sentence into list of words and punctuation items.
        /// </summary>
        /// <param name="text">Sentence as string</param>
        /// <returns>List of strings; each string is words or punctuation sign.</returns>
        public static string[] tokenize(string text)
        {
            return (from Match x in Regexes.WordPunct.Matches(text) select x.Value).ToArray();
        }
    };

}
