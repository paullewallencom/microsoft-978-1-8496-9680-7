using System;
using System.Globalization;
using System.Windows.Data;

namespace SpaceAim3D.Models
{
    /// <summary>The converter of a boolean value into an opacity value.</summary>
    /// <remarks>"TRUE" value should be converted into 0.6f, while "FALSE" - into 0.2f.</remarks>
    public class OpacityConverter : IValueConverter
    {
        /// <summary>Converts a boolean value into an opacity value.</summary>
        /// <param name="value">Source value, i.e. a boolean value.</param>
        /// <param name="targetType">Target type (not used).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">Culture data (not used).</param>
        /// <returns>Converted value (an opacity value).</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? 0.6f : 0.2f;
        }

        /// <summary>Converts an opacity value into a boolean value.</summary>
        /// <param name="value">A opacity value.</param>
        /// <param name="targetType">Target type (not used).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">Culture data (not used).</param>
        /// <returns>Converted value (a boolean value).</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float valueFloat = (float)System.Convert.ToDouble(value);
            return valueFloat == 0.6f;
        }
    }
}
