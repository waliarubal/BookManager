using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;

namespace AvaloniaDemosntration.Converters;
public class PathToImageConverter : IValueConverter
{
    //public static readonly PathToImageConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) 
            return null;

        var width = int.Parse(parameter as string);
        using var stream = File.OpenRead(value as string);
        return Bitmap.DecodeToWidth(stream, width);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
