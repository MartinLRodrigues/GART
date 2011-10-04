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
using System.Device.Location;
using System.ComponentModel;
using Matrix = Microsoft.Xna.Framework.Matrix;
using GART.Data;

namespace GART.Controls
{
    /// <summary>
    /// A base control that serves as the starting point for an augmented reality view.
    /// </summary>
    public abstract class ARView : Control, IARView
    {
        #region Static Version
        #region Dependency Properties
        /// <summary>
        /// Identifies the <see cref="Attitude"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty AttitudeProperty = DependencyProperty.Register("Attitude", typeof(Matrix), typeof(ARView), new PropertyMetadata(ARDefaults.EmptyMatrix, OnAttitudeChanged));

        private static void OnAttitudeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARView)d).OnAttitudeChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="AttitudeHeading"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty AttitudeHeadingProperty = DependencyProperty.Register("AttitudeHeading", typeof(double), typeof(ARView), new PropertyMetadata(0d, OnAttitudeHeadingChanged));

        private static void OnAttitudeHeadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARView)d).OnAttitudeHeadingChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="Location"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(GeoCoordinate), typeof(ARView), new PropertyMetadata(ARDefaults.DefaultStartLocation, OnLocationChanged));

        private static void OnLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARView)d).OnLocationChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="TravelHeading"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty TravelHeadingProperty = DependencyProperty.Register("TravelHeading", typeof(double), typeof(ARView), new PropertyMetadata(0d, OnTravelHeadingChanged));

        private static void OnTravelHeadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARView)d).OnTravelHeadingChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="Video"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty VideoProperty = DependencyProperty.Register("Video", typeof(Brush), typeof(ARView), new PropertyMetadata(ARDefaults.VideoPlaceholderBrush, OnVideoChanged));

        private static void OnVideoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARView)d).OnVideoChanged(e);
        }
        #endregion // Dependency Properties
        #endregion // Static Version

        #region Instance Version
        #region Overridables / Event Triggers
        /// <summary>
        /// Occurs when the value of the <see cref="Attitude"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnAttitudeChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        
        /// <summary>
        /// Occurs when the value of the <see cref="AttitudeHeading"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnAttitudeHeadingChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="Location"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnLocationChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="TravelHeading"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnTravelHeadingChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// Occurs when the value of the <see cref="Video"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnVideoChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion // Overridables / Event Triggers

        #region Public Properties
        /// <summary>
        /// Gets or sets a matrix that represents where the user is looking. This is a dependency property.
        /// </summary>
        /// <value>
        /// A matrix that represents where the user is looking.
        /// </value>
        [Category("AR")]
        public Matrix Attitude
        {
            get
            {
                return (Matrix)GetValue(AttitudeProperty);
            }
            set
            {
                SetValue(AttitudeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the direction the user is looking in degrees. This is a dependency property.
        /// </summary>
        /// <value>
        /// The direction the user is looking in degrees.
        /// </value>
        [Category("AR")]
        public double AttitudeHeading
        {
            get
            {
                return (double)GetValue(AttitudeHeadingProperty);
            }
            set
            {
                SetValue(AttitudeHeadingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the location of the user in Geo space. This is a dependency property.
        /// </summary>
        /// <value>
        /// The location of the user in Geo space.
        /// </value>
        [Category("AR")]
        public GeoCoordinate Location
        {
            get
            {
                return (GeoCoordinate)GetValue(LocationProperty);
            }
            set
            {
                SetValue(LocationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the direction the user is traveling in degrees. This is a dependency property.
        /// </summary>
        /// <value>
        /// The direction the user is traveling in degrees.
        /// </value>
        [Category("AR")]
        public double TravelHeading
        {
            get
            {
                return (double)GetValue(TravelHeadingProperty);
            }
            set
            {
                SetValue(TravelHeadingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a brush that represents the video feed from the camera. This is a dependency property.
        /// </summary>
        /// <value>
        /// A brush that represents the video feed from the camera.
        /// </value>
        [Category("AR")]
        public Brush Video
        {
            get
            {
                return (Brush)GetValue(VideoProperty);
            }
            set
            {
                SetValue(VideoProperty, value);
            }
        }
        #endregion // Public Properties
        #endregion // Instance Version
    }
}
