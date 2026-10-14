using System.Globalization;

namespace TransferMarketApp.Converters;

/// <summary>
/// Inverts a boolean. Used to show/hide UI based on the inverse of a flag.
/// </summary>
public class InverseBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool b && !b;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool b && !b;
    }
}