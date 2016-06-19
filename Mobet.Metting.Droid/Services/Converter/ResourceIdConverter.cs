using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MvvmCross.Platform.Converters;

namespace Mobet.Metting.Droid.Services.Converter
{
    public class ResourceIdConverter : MvxValueConverter<string, int>
    {
        protected override int Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            int resourceId = 0;
            if (value == "Icon")
                resourceId = Resource.Drawable.Icon;

            return resourceId;
        }
    }
}