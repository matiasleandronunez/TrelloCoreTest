using System;

namespace TrelloCoreTest.Support
{
    public static class GenericUtils
    {
        /// <summary>
        /// Generates alphanumeric [a-zA-Z0-9] string
        /// </summary>
        /// <param name="length">
        /// Length of the string to generate, defaults to 6
        /// </param>
        /// <returns>
        /// Alphanumeric string of the indicated length
        /// </returns>
        public static string RandomString(int length=6)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }
    }
}
