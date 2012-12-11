using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls.Maps.Platform;

namespace GART
{
    static public class GeoExtensions
    {
        static public bool IsUnknown(this Location location)
        {
            return (location.Latitude == 0 && location.Longitude == 0 && location.Altitude == 0);
        }
    }
}
