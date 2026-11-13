using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace GUI.Presentation;

public class StringToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converts a value from the source type to the target type in a data binding scenario.
    /// </summary>
    /// <param name="value">The value produced by the binding source to be converted.</param>
    /// <param name="targetType">The type to convert the value to.</param>
    /// <param name="parameter">An optional parameter to be used in the conversion logic. This value may be null.</param>
    /// <param name="language">The language to use in the converter. This value may be null.</param>
    /// <returns>The converted value to pass to the binding target.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
    }

    /// <summary>
    /// Converts a value from the target type back to the source type in a data binding scenario.
    /// </summary>
    /// <remarks>This method is typically used in data binding scenarios where only one-way conversion is
    /// supported. Calling this method will always return <see cref="DependencyProperty.UnsetValue"/>, signaling that
    /// the operation is not implemented.</remarks>
    /// <param name="value">The value produced by the binding target to be converted.</param>
    /// <param name="targetType">The type to convert the value to.</param>
    /// <param name="parameter">An optional parameter to be used in the conversion logic. This value may be null.</param>
    /// <param name="language">The language to use in the converter. This value may be null.</param>
    /// <returns>
    /// For one-way bindings this converter does not perform a back-conversion. Return
    /// DependencyProperty.UnsetValue to indicate the conversion is not supported.
    /// </returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return DependencyProperty.UnsetValue;
    }
}
