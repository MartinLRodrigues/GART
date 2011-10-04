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

namespace GART.Data
{
    #region Conversion Notes
    /******************************************************************************
     * http://geography.about.com/library/faq/blqzdistancedegree.htm
     * 
     * 
     * Because the lines of latitude are parallel and evenly spaced, a degree of latitude represents a constant distance on the ground.
     * Because the lines of longitude converge at the poles, a degree of longitude represents a varying distance on the ground, depending on the latitude.
     * 
     * Each degree of latitude is approximately 111.133km apart.
     * A degree of longitude is widest at the equator at 111.321km and gradually shrinks to 0km at the poles.
     * 
     * Ignoring curvature of the earth and assuming a mean earth radius of 6371 km, the distance between 
     * lines of longitude that are 1 degree apart = 6371 * 2 * pi /360 * cosine(latitude) in km. 
     * 
     *****************************************************************************/
    #endregion // Conversion Notes 

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
            // Determine the mid-point between our two latitudes
            double midLatitude = (a.Latitude + b.Latitude) / 2;

            // Determine how many meters there are between longitude lines at our mid-point
            double longMetersAtMidpoint = EarthRadiusInMeters * 2 * (Math.PI / 360) * Math.Cos(midLatitude);

            // Calculate latitude and longitude distance in meters
            double latMeters = (a.Latitude - b.Latitude) * MetersPerLatitudeDegree;
            double longMeters = (a.Longitude - b.Longitude) * longMetersAtMidpoint;

            // Return in Vector3 format
            return new Vector3(-(float)longMeters, 0f, (float)latMeters);
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
    }
}
