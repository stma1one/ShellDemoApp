using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBaseApp.Converters
{
	internal class boolToIconConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value != null)
			{
				bool isTrue = (bool)value;
				return isTrue ? Helper.FontHelper.CLOSED_EYE_ICON : Helper.FontHelper.OPEN_EYE_ICON;  // Return the appropriate icon based on the boolean value
			}
			return Helper.FontHelper.CLOSED_EYE_ICON;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
