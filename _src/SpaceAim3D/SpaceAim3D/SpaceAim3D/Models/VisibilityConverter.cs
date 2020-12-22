using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SpaceAim3D.Models
{
    /// <summary>The converter of a string value into a visibility setting.</summary>
    /// <remarks>Not empty string should be converted into "Visible" value, 
    /// while empty (or equal to null) - into "Collapsed".</remarks>
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>Converts a string value into a visibility setting.</summary>
        /// <param name="value">Source value, i.e. a string value.</param>
        /// <param name="targetType">Target type (not used).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">Culture data (not used).</param>
        /// <returns>Converted value (a visibility setting).</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            return string.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>Converts a visibility setting into a string value.</summary>
        /// <param name="value">A visibility setting.</param>
        /// <param name="targetType">Target type (not used).</param>
        /// <param name="parameter">Optional parameter (not used).</param>
        /// <param name="culture">Culture data (not used).</param>
        /// <returns>This operation is unsupported.</returns>
        /// <exception cref="NotSupportedException">This operation is unsupported.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
