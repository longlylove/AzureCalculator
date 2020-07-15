using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities
{
    public class RandomHelper
    {
        private static readonly Random Random = new Random();
        private static readonly string AlphabeticalSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static readonly string AlphanumericSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static readonly string NumericSet = "0123456789";
        private static readonly string SpecialCharSet = "!`#~@{}_$^%.&'()-";

        public string GetRandomAlphanumericString (int length)
        {
            return new string(Enumerable.Repeat(AlphanumericSet, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public int GetRandomInteger(int length, bool negative = false)
        {
            var sb = new StringBuilder();
            foreach (var item in Enumerable.Repeat(NumericSet, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray())
            {
                sb.Append(item);
            }

            return negative? int.Parse(sb.ToString()) : -int.Parse(sb.ToString());
        }

        public decimal GetRandomDecimal(int numericLength, int decimalPlace = 2, bool negative = false)
        {
            var dec = decimal.Parse(
                $"{new string(Enumerable.Repeat(NumericSet, numericLength) .Select(s => s[Random.Next(s.Length)]).ToArray())}" +
                "." +
                $"{new string(Enumerable.Repeat(NumericSet, decimalPlace) .Select(s => s[Random.Next(s.Length)]).ToArray())}");
            return negative ? dec : -dec;
        }
    }
}
