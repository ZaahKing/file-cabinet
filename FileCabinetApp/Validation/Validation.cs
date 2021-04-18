using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Contain validations.
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Min length.
        /// </summary>
        /// <param name="str">String.</param>
        /// <param name="minLength">Min possible length.</param>
        /// <param name="maxLength">Max possible length.</param>
        /// <returns>'True' if in range.</returns>
        public static bool LengthInRange(this string str, int minLength, int maxLength)
        {
            return str.Length >= minLength && str.Length <= maxLength;
        }

        /// <summary>
        /// Value in range.
        /// </summary>
        /// <typeparam name="T">IComparable.</typeparam>
        /// <param name="value">Value.</param>
        /// <param name="minValue">Min value.</param>
        /// <param name="maxValue">Max value.</param>
        /// <returns>'True' if in range.</returns>
        public static bool ValueInRange<T>(this T value, T minValue, T maxValue)
            where T : IComparable<T>
        {
            return value.CompareTo(minValue) >= 0 && value.CompareTo(maxValue) <= 0;
        }

        /// <summary>
        /// Is negative.
        /// </summary>
        /// <param name="number">Number.</param>
        /// <returns>'True' if positive.</returns>
        public static bool IsNegative(this int number)
        {
            return number < 0;
        }

        /// <summary>
        /// Is classic gender.
        /// </summary>
        /// <param name="letter">Letter.</param>
        /// <returns>'True' if classic gender.</returns>
        public static bool IsClassicGender(this char letter)
        {
            return "fm".Contains(letter, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
