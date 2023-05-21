using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutSolver.Converters
{
    public class IsCandidateToColorConverter : IValueConverter
    {
        public Color CandidateColor { get; set; } = Colors.DarkGreen;
        public Color NotCandidateColor { get; set; } = Colors.DarkRed;

        public IsCandidateToColorConverter() { }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var e = value as bool?;
            if (e == null)
                return null;
            else return e.Value ? CandidateColor : NotCandidateColor;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as Color;
            if (color == null)
                return null;
            else if (color == CandidateColor)
                return true;
            else if (color == NotCandidateColor)
                return false;
            return null;
        }
    }
}
