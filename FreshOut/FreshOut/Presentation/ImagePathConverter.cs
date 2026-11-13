using Microsoft.UI.Xaml.Data;

namespace GUI.Presentation;

public class ImagePathConverter : IValueConverter
{
    /// <summary>
    /// Converts a file name to the corresponding profile picture asset path.
    /// </summary>
    /// <param name="value">The value to convert. Expected to be a string representing the file name.</param>
    /// <param name="targetType">The type to convert the value to. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter for the conversion. This parameter is not used.</param>
    /// <param name="language">The language to use in the converter. This parameter is not used.</param>
    /// <returns>A string containing the relative path to the profile picture asset if the input value is a string; otherwise, null.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string fileName)
            return $"/Assets/ProfilePics/{fileName}.png";
        return null;
    }

    /// <summary>
    /// Not supported. Always returns a value indicating that reverse conversion is not implemented.
    /// </summary>
    /// <remarks>This method is typically used in data binding scenarios where only one-way conversion is
    /// supported. Calling this method will always return <see cref="DependencyProperty.UnsetValue"/>, signaling that
    /// the operation is not implemented.</remarks>
    /// <param name="value">The value produced by the binding target to be converted back to the source type. This parameter is ignored.</param>
    /// <param name="targetType">The type to convert the value to. This parameter is ignored.</param>
    /// <param name="parameter">An optional parameter to be used in the conversion logic. This parameter is ignored.</param>
    /// <param name="language">The culture to use in the converter. This parameter is ignored.</param>
    /// <returns>A sentinel value indicating that reverse conversion is not supported.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return DependencyProperty.UnsetValue;
    }
}
