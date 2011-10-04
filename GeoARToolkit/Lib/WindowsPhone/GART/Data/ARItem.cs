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
    public enum WorldCalculationMode
    {
        /// <summary>
        /// <see cref="ARItem.WorldLocation">WorldLocation</see> is an absolute value and will not be calculated. 
        /// </summary>
        None,
        /// <summary>
        /// <see cref="ARItem.WorldLocation">WorldLocation</see> is calculated by determining the distance between  
        /// <see cref="ARItem.GeoLocation">GeoLocation</see> and the users current location. 
        /// </summary>
        GeoRelativeToLocation,
        /// <summary>
        /// <see cref="ARItem.WorldLocation">WorldLocation</see> is calculated by treating 
        /// <see cref="ARItem.RelativeLocation">RelativeLocation</see> as a constant distance away from the users 
        /// current location. This method is useful for heads-up display items and items that should appear to 
        /// travel with the user.
        /// </summary>
        RelativeToLocation
    }

    /// <summary>
    /// An item that is rendered in one or more ARViews.
    /// </summary>
    public class ARItem : ObservableObject
    {
        #region Instance Version
        #region Member Variables
        private object content;
        private GeoCoordinate geoLocation = GeoCoordinate.Unknown;
        private Vector3 relativeLocation = Vector3.Zero;
        private WorldCalculationMode worldCalculationMode;
        private Vector3 worldLocation = Vector3.Zero;
        #endregion // Member Variables

        #region Public Properties
        /// <summary>
        /// Gets or sets the content used to represnt the item.
        /// </summary>
        /// <value>
        /// The content used to represnt the item.
        /// </value>
        public object Content
        {
            get
            {
                return content;
            }
            set
            {
                if (content != value)
                {
                    content = value;
                    NotifyPropertyChanged(() => Content);
                }
            }
        }

        /// <summary>
        /// Gets or sets the location of the item in geo space.
        /// </summary>
        /// <value>
        /// The location of the item in virtual geo space.
        /// </value>
        public GeoCoordinate GeoLocation
        {
            get
            {
                return geoLocation;
            }
            set
            {
                if (geoLocation != value)
                {
                    geoLocation = value;
                    NotifyPropertyChanged(() => GeoLocation);
                }
            }
        }

        /// <summary>
        /// Gets or sets the location of the item in virtual relative space.
        /// </summary>
        /// <value>
        /// The location of the item in virtual relative space.
        /// </value>
        public Vector3 RelativeLocation
        {
            get
            {
                return relativeLocation;
            }
            set
            {
                if (relativeLocation != value)
                {
                    relativeLocation = value;
                    NotifyPropertyChanged(() => RelativeLocation);
                }
            }
        }

        /// <summary>
        /// Gets or sets the mode used to calculate the items position in virtual world space.
        /// </summary>
        /// <value>
        /// The mode used to calculate the items position in virtual world space.
        /// </value>
        public WorldCalculationMode WorldCalculationMode
        {
            get
            {
                return worldCalculationMode;
            }
            set
            {
                if (worldCalculationMode != value)
                {
                    worldCalculationMode = value;
                    NotifyPropertyChanged(() => WorldCalculationMode);
                }
            }
        }

        /// <summary>
        /// Gets or sets the location of the item in virtual world space.
        /// </summary>
        /// <value>
        /// The location of the item in virtual world space.
        /// </value>
        public Vector3 WorldLocation
        {
            get
            {
                return worldLocation;
            }
            set
            {
                if (worldLocation != value)
                {
                    worldLocation = value;
                    NotifyPropertyChanged(() => WorldLocation);
                }
            }
        }
        #endregion // Public Properties
        #endregion // Instance Version
    }
}
