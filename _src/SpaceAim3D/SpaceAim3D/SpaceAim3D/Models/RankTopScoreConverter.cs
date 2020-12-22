using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SpaceAim3D.Models
{
    /// <summary>The converter of a boolean value into a font weight.</summary>
    /// <remarks>"TRUE" value should be converted into "Bold" font, while "FALSE" - into "Normal".</remarks>
    public class RankTopScoreConverter : IValueConverter
    {
        /// <summary>Converts a boolean value into a font weight.</summary>
        /// <param name="value">Source value, i.e. a boolean value.</param>
        /// <param name="targetType">Target type (not used).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">Culture data (not used).</param>
        /// <returns>Converted value (a font weight).</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? FontWeights.Bold : FontWeights.Normal;
        }

        /// <summary>Converts a font weight into a boolean value.</summary>
        /// <param name="value">A font weight.</param>
        /// <param name="targetType">Target type (not used).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">Culture data (not used).</param>
        /// <returns>Converted value (a boolean value).</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FontWeight)
            {
                return (FontWeight)value == FontWeights.Bold;
            }

            return null;
        }
    }
}
