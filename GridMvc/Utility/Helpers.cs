using System;

namespace GridMvc.Utility
{
    internal static class Helpers
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int IdSize = 5;

        public static string GenerateGridId()
        {
            var rng = new Random();
            var buffer = new char[IdSize];

            for (int i = 0; i < IdSize; i++)
            {
                buffer[i] = Chars[rng.Next(Chars.Length)];
            }
            return new string(buffer);
        }

        public static string SanitizeGridId(string id)
        {
            return id.Replace(" ", string.Empty); //TODO add other sanitize rules
        }
    }
}