using MinimalAPIValidationDemo.Models;
using System;
using System.Text.Json;

namespace MinimalAPIValidationDemo.Utility
{
    public static class UrlShortenerSettings
    {
        public const int Length = 7;
        public const string Alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly JsonSerializerOptions CachedJsonSerializerOptions = new() { WriteIndented = true };
        public static string Code
        {
            get
            {
                Random _random = new();
                var codeChars = new char[UrlShortenerSettings.Length];
                int maxValue = UrlShortenerSettings.Alphabet.Length;

                while (true)
                {
                    for (var i = 0; i < UrlShortenerSettings.Length; i++)
                    {
                        var randomIndex = _random.Next(maxValue);

                        codeChars[i] = UrlShortenerSettings.Alphabet[randomIndex];
                    }
                    return new string(codeChars);
                }
            }
        }
    }
}
