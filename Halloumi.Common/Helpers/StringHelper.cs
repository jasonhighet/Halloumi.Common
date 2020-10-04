using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace Halloumi.Common.Helpers
{
    public static class StringHelper
    {
        #region Public Methods

        /// <summary>
        /// Converts a string to title case. Eg "title case" becomes "Title Case"
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <returns>The text as title case</returns>
        public static string TitleCase(string text)
        {
            // get the culture of current thread and create textinfo object.
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;

            // convert to title case.
            return textInfo.ToTitleCase(text);
        }

        /// <summary>
        /// Converts a string to title case. Eg "LandlordName" becomes "landlordName", "GSTRate" becomes "gstRate"
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <returns>The text as camel case</returns>
        public static string CamelCase(string text)
        {
            var newText = string.Empty;
            var camelling = true;

            for (var i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var nextChar = char.MinValue;
                if (i + 1 < text.Length) nextChar = text[i + 1];

                if (camelling && (i == 0 || (char.IsUpper(currentChar) && !char.IsLower(nextChar))))
                {
                    newText += char.ToLower(currentChar);
                }
                else
                {
                    newText += currentChar;
                }
                if (!char.IsUpper(currentChar)) camelling = false;
            }
            return newText;
        }


        /// <summary>
        /// Seperates a string by placing spaces in between the words, using capitals to determine words.
        /// Eg "LandlordName" becomes "Landlord Name", "GSTRate" becomes "GST Rate"
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <returns>The text with spaces seperating the words</returns>
        public static string SeperateWordsByCapitals(string text)
        {
            var newText = string.Empty;
            for (var i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var nextChar = char.MinValue;
                var nextNextChar = char.MinValue;
                if (i + 1 < text.Length) nextChar = text[i + 1];
                if (i + 2 < text.Length) nextNextChar = text[i + 2];

                newText += currentChar;

                if ((char.IsLower(currentChar) && char.IsUpper(nextChar))
                    || (char.IsUpper(currentChar) && char.IsUpper(nextChar) && char.IsLower(nextNextChar)))
                    newText += " ";
            }
            return newText;
        }

        /// <summary>
        /// Limits the length of a piece of text
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="length">The length.</param>
        /// <returns>The limited text</returns>
        public static string LimitTextLength(string text, int length)
        {
            if (string.IsNullOrEmpty(text) || text.Length < length) return text;
            if (length == 0) return "";

            text = text.Substring(0, length).Trim();

            var index = text.LastIndexOf(" ");
            if (index > length / 2)
            {
                text = text.Substring(0, index).Trim();
            }
            if (text.Length <= length - 3) text += text + "...";

            return text;
        }

        /// <summary>
        /// Does a fuzzy compare of two strings. If they are the same or approximately the same, returns true.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="destination">The destination string.</param>
        /// <returns>True if the two strings are approximately the same</returns>
        public static bool FuzzyCompare(string source, string destination)
        {
            source = GetAlphaNumericCharactersOnly(source.ToLower());
            destination = GetAlphaNumericCharactersOnly(destination.ToLower());

            if (source == destination) return true;
            if (source.Length == 0) return false;
            if (destination.Length == 0) return false;
            

            var sourceNumbers = GetNumberAtEnd(source);
            var destinationNumbers = GetNumberAtEnd(destination);

            if (sourceNumbers != "" && destinationNumbers != "" && sourceNumbers != destinationNumbers)
            {
                if (source.Replace(sourceNumbers, "") == destination.Replace(destinationNumbers, ""))
                {
                    return false;
                }
            }

            var threshold = 10M;
            var oneCharPercent = 1M / Convert.ToDecimal(source.Length) * 100M;
            if (oneCharPercent < 50M && threshold < oneCharPercent) threshold = oneCharPercent;

            var distance = GetLevenshteinDistance(source, destination);
            var distancePercent = Convert.ToDecimal(distance) / Convert.ToDecimal(source.Length) * 100M;
            distancePercent = Decimal.Round(distancePercent, 0);

            return (distancePercent <= threshold);
        }

        /// <summary>
        /// Returns the alpha numeric characters of a string
        /// </summary>
        /// <param name="text">The text to get the alpha numeric characters.</param>
        /// <returns>The text with the non-alphanumeric characters stripped out</returns>
        public static string GetAlphaNumericCharactersOnly(string text)
        {
            if (_alphaNumericRegex == null) _alphaNumericRegex = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            return _alphaNumericRegex.Replace(text, "");
        }
        private static Regex _alphaNumericRegex = null;

        /// <summary>
        /// Returns the alphabetic characters of a string
        /// </summary>
        /// <param name="text">The text to get the alphabetic characters.</param>
        /// <returns>The text with the non-alphabetic characters stripped out</returns>
        public static string GetAlphabeticCharactersOnly(string text)
        {
            if (_alphabeticRegex == null) _alphabeticRegex = new Regex("[^a-zA-Z]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            return _alphabeticRegex.Replace(text, "");
        }
        private static Regex _alphabeticRegex = null;

        /// <summary>
        /// Returns the numeric characters of a string
        /// </summary>
        /// <param name="text">The text to get the numeric characters.</param>
        /// <returns>The text with the non-numeric characters stripped out</returns>
        public static string GetNumericCharactersOnly(string text)
        {
            if (_numericRegex == null) _numericRegex = new Regex("[^0-9]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
            return _numericRegex.Replace(text, "");
        }
        private static Regex _numericRegex = null;

        /// <summary>
        /// Returns the number at the end of a string if there is one, or an empty string if there isn't one
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>the number at the end of a string if there is one, or an empty string if there isn't</returns>
        public static string GetNumberAtEnd(string text)
        {
            if (_numberAtEndRegex == null) _numberAtEndRegex = new Regex(@"(\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

            var match = _numberAtEndRegex.Match(text);
            if (match.Success) return match.Groups[1].Value;
            else return "";
        }
        private static Regex _numberAtEndRegex = null;


        /// <summary>
        /// Gets the levenshtein distance between two strings
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="destination">The destination string.</param>
        /// <returns>The levenshtein distance between the strings (number of differences in characters)</returns>
        public static int GetLevenshteinDistance(string source, string destination)
        {
            if (source == destination) return 0;
            if (source.Length == 0) return destination.Length;
            if (destination.Length == 0) return source.Length;

            var m = destination.Length;
            var n = source.Length;

            var distances = new int[m + 1, n + 1];
            for (var i = 0; i <= m; i++) distances[i, 0] = i;
            for (var j = 0; j <= n; j++) distances[0, j] = j;

            for (var i = 1; i <= m; i++)
            {
                for (var j = 1; j <= n; j++)
                {
                    var cost = destination[i - 1] == source[j - 1] ? 0 : 1;
                    var insertion = distances[i, j - 1] + 1;
                    var substitution = distances[i - 1, j - 1] + cost;
                    var deletion = distances[i - 1, j] + 1;
                    distances[i, j] = Math.Min(Math.Min(deletion, insertion), substitution);
                }
            }
            return distances[m, n];
        }


        #endregion
    }
}
