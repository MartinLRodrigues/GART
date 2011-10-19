#region License
/******************************************************************************
 * COPYRIGHT © MICROSOFT CORP. 
 * MICROSOFT LIMITED PERMISSIVE LICENSE (MS-LPL)
 * This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
 * 1. Definitions
 * The terms “reproduce,” “reproduction,” “derivative works,” and “distribution” have the same meaning here as under U.S. copyright law.
 * A “contribution” is the original software, or any additions or changes to the software.
 * A “contributor” is any person that distributes its contribution under this license.
 * “Licensed patents” are a contributor’s patent claims that read directly on its contribution.
 * 2. Grant of Rights
 * (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
 * (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
 * 3. Conditions and Limitations
 * (A) No Trademark License- This license does not grant you rights to use any contributors’ name, logo, or trademarks.
 * (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
 * (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
 * (D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
 * (E) The software is licensed “as-is.” You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
 * (F) Platform Limitation- The licenses granted in sections 2(A) & 2(B) extend only to the software or derivative works that you create that run on a Microsoft Windows operating system product.
 ******************************************************************************/
#endregion // License

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
using Microsoft.Xna.Framework;
using System.Device.Location;
using GART.Controls;

namespace GART.Data
{
    static public class ARHelper
    {
        #region Constants
        private const double EarthRadiusInMeters = 6371000d;
        private const double MetersPerLatitudeDegree = 111133d;
        #endregion // Constants

        /// <summary>
        /// Calculates the distance between two GeoCoordinates in 3D space.
        /// </summary>
        /// <param name="a">
        /// The first GeoCoordinate
        /// </param>
        /// <param name="b">
        /// The second GeoCoordinate
        /// </param>
        /// <returns>
        /// The distance between the two points in 3D space.
        /// </returns>
        /// <remarks>
        /// This method assumes 1 3D unit = 1 meter. This method is also not incredibly accurate 
        /// but it's accurate enough for the scales used in most AR applications.
        /// </remarks>
        static public Vector3 DistanceBetween(GeoCoordinate a, GeoCoordinate b)
        {
            // Use GeoCoordinate provided methods to calculate distance. Use 
            // same longitude on both points to calculate latitude distance and 
            // use same latitude on both points to calculate longitude distance.
            float latitudeMeters = (float)a.GetDistanceTo(new GeoCoordinate(b.Latitude, a.Longitude));
            float longitudeMeters = (float)a.GetDistanceTo(new GeoCoordinate(a.Latitude, b.Longitude));

            // Invert the distance sign if necessary to account for direction 
            if (a.Latitude < b.Latitude)
            {
                latitudeMeters *= -1;
            }
            if (a.Longitude > b.Longitude)
            {
                longitudeMeters *= -1;
            }

            // Now calculate the altitude difference, but only if both sides of the equation have altitudes
            float altitudeMeters = 0f;
            if ((!a.Altitude.Equals(Double.NaN)) && (!b.Altitude.Equals(Double.NaN)))
            {
                altitudeMeters = (float)(b.Altitude - a.Altitude);
            }

            // Return the new point
            return new Vector3(longitudeMeters, altitudeMeters, latitudeMeters);
        }

        /// <summary>
        /// Converts the <see cref="GeoCoordinate"/> to a properly formatted WGS84 string.
        /// </summary>
        /// <param name="coordinate">
        /// The GeoCoordinate to convert.
        /// </param>
        /// <returns>
        /// The WGS84 string.
        /// </returns>
        static public string ToWGS84String(this GeoCoordinate coordinate)
        {
            return string.Format("{0}, {1}", coordinate.Latitude, coordinate.Longitude);
        }

        /// <summary>
        /// Calculates the virtual-world location based on the distance between the user and the geo location.
        /// </summary>
        /// <param name="settings">
        /// The settings used to perform the calculation.
        /// </param>
        /// <param name="item">
        /// The item to calculate and update.
        /// </param>
        static public void WorldFromGeoLocation(ItemCalculationSettings settings, ARItem item)
        {
            // NOTE: Right now we don't support 3D rendering in XNA and right now 
            // ARDisplay assumes that the user is always standing at location 0,0,0. 
            // When we add 3D rendering we will need to allow our position in 3D space 
            // to change, which will cause the calculations below to change as well.

            item.WorldLocation = ARHelper.DistanceBetween(settings.View.Location, item.GeoLocation);
        }

        /// <summary>
        /// Calculates the virtual-world location based on an offset from the users location.
        /// </summary>
        /// <param name="settings">
        /// The settings used to perform the calculation.
        /// </param>
        /// <param name="item">
        /// The item to calculate and update.
        /// </param>
        static public void WorldFromRelativeLocation(ItemCalculationSettings settings, ARItem item)
        {
            // For now just update WorldLocation to be 
            // the same as RelativeLocation. When 3D is 
            // added we'll need to take into account the
            // users current location in 3D space.
            item.WorldLocation = item.RelativeLocation;
        }
    }
}
