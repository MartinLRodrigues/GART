#if WP7
using Microsoft.Phone.Controls.Maps.Platform;
#else
using Bing.Maps;
#endif

namespace GART
{
    static public class GeoExtensions
    {
        static public bool IsUnknown(this Location location)
        {
            #if WP7
            return (location.Latitude == 0 && location.Longitude == 0 && location.Altitude == 0);
            #else
            return (location.Latitude == 0 && location.Longitude == 0);
            #endif
        }
    }
}
