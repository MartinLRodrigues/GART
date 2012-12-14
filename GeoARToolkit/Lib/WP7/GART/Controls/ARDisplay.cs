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

#if WP7
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps.Platform;
using System.Device.Location;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using VideoSource = System.Windows.Media.Brush;
#else
using Bing.Maps;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Motion = Windows.Devices.Sensors.Inclinometer;
using Windows.Media.Capture;
using VideoSource = Windows.Media.Capture.MediaCapture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

#endif 

using GART.BaseControls;
using GART.Data;
using Microsoft.Xna.Framework;
using Matrix = Microsoft.Xna.Framework.Matrix;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace GART.Controls
{
    #if WP7
    [ContentProperty("Views")]
    #else
    [ContentProperty(Name="Views")]
    #endif
    public class ARDisplay : Grid, IARItemsView
    {
        #region Static Version

        #region Dependency Properties
        /// <summary>
        /// Identifies the <see cref="ARItems"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty ARItemsProperty = DependencyProperty.Register("ARItems", typeof(ObservableCollection<ARItem>), typeof(ARDisplay), new PropertyMetadata(new ObservableCollection<ARItem>(), OnARItemsChanged));

        private static void OnARItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnARItemsChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="Attitude"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty AttitudeProperty = DependencyProperty.Register("Attitude", typeof(Matrix), typeof(ARDisplay), new PropertyMetadata(ARDefaults.EmptyMatrix, OnAttitudeChanged));

        private static void OnAttitudeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnAttitudeChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="CameraEnabled"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty CameraEnabledProperty = DependencyProperty.Register("CameraEnabled", typeof(bool), typeof(ARDisplay), new PropertyMetadata(true, OnCameraEnabledChanged));

        private static void OnCameraEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnCameraEnabledChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="AttitudeHeading"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty AttitudeHeadingProperty = DependencyProperty.Register("AttitudeHeading", typeof(double), typeof(ARDisplay), new PropertyMetadata(0d, OnAttitudeHeadingChanged));

        private static void OnAttitudeHeadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnAttitudeHeadingChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="Location"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Location), typeof(ARDisplay), new PropertyMetadata(ARDefaults.DefaultStartLocation, OnLocationChanged));

        private static void OnLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnLocationChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="LocationEnabled"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty LocationEnabledProperty = DependencyProperty.Register("LocationEnabled", typeof(bool), typeof(ARDisplay), new PropertyMetadata(true, OnLocationEnabledChanged));

        private static void OnLocationEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnLocationEnabledChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="MotionEnabled"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty MotionEnabledProperty = DependencyProperty.Register("MotionEnabled", typeof(bool), typeof(ARDisplay), new PropertyMetadata(true, OnMotionEnabledChanged));

        private static void OnMotionEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnMotionEnabledChanged(e);
        }

        static public readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(ControlOrientation), typeof(ARDisplay), new PropertyMetadata(ControlOrientation.Landscape, OnOrientationChanged));

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnOrientationChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="TravelHeading"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty TravelHeadingProperty = DependencyProperty.Register("TravelHeading", typeof(double), typeof(ARDisplay), new PropertyMetadata(0d, OnTravelHeadingChanged));

        private static void OnTravelHeadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnTravelHeadingChanged(e);
        }

        /// <summary>
        /// Identifies the <see cref="VideoSource"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty VideoSourceProperty = DependencyProperty.Register("VideoSource", typeof(VideoSource), typeof(ARDisplay), new PropertyMetadata(ARDefaults.DefaultVideoSource, OnVideoSourceChanged));

        private static void OnVideoSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ARDisplay)d).OnVideoSourceChanged(e);
        }

        #endregion // Dependency Properties
        #endregion // Static Version

        #region Instance Version
        #region Member Variables

        #if WP7
        private GeoCoordinateWatcher location;
        private Motion motion;
        private PhotoCamera photoCamera;
        #else
        private Geolocator location;
        private Inclinometer motion;
        #endif

        private bool servicesRunning;
        private ObservableCollection<IARView> views;
        #endregion // Member Variables

        #region Constructors
        public ARDisplay()
        {
            // Create the views collection
            views = new ObservableCollection<IARView>();

            // Subscribe to events
            views.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(views_CollectionChanged);
        }
        #endregion // Constructors

        #region Internal Methods
        private void CalculateItemLocations()
        {
            // Create the settings instance
            ItemCalculationSettings settings = new ItemCalculationSettings { View = this };

            // Loop through all the items
            for (int i = 0; i < ARItems.Count; i++)
            {
                // Get the item
                ARItem item = ARItems[i];

                // If a calculation exists, call it
                if (item.WorldCalculation != null)
                {
                    item.WorldCalculation(settings, item);
                }
            }
        }

        #if WP7
        private void CreateVideoBrush()
        {
            // Create our brush
            VideoBrush vb = new VideoBrush();

            // The video from the camera comes across at 640 x 480 but in portraid mode we need to 
            // rotate the video 90 degrees.
            vb.RelativeTransform = new CompositeTransform { CenterX = 0.5, CenterY = 0.5, Rotation = photoCamera.Orientation };

            // Ideally we would make sure video is never stretched, but 
            // this method doesn't seem to work correctly when the video 
            // brush is used in multiple places.
            // vb.Stretch = Stretch.Uniform;

            // Update our dependency property
            VideoSource = vb;
        }
        #endif

        private void AddViews(IList views)
        {
            // Can only add and remove if the view container has been loaded from the template
            foreach (object view in views)
            {
                // Make sure it's not us
                if (view == this) { throw new InvalidOperationException("Cannot add the ARDisplay as a view, this would cause a circular reference."); }

                // Look for known interfaces
                IARView arView = view as IARView;
                IARItemsView itemView = view as IARItemsView;
                UIElement uie = view as UIElement;

                // If it's an IARView (which it always should be) setup current values
                if (arView != null)
                {
                    // arView.Attitude = this.Attitude; // HACK: Uncommenting this line breaks the Visual Studio designer. No reason is known.
                    arView.AttitudeHeading = this.AttitudeHeading;
                    arView.Location = this.Location;
                    arView.VideoSource = this.VideoSource;
                }

                // If it's an IARItemsView give it the list of current items
                if (itemView != null)
                {
                    itemView.ARItems = this.ARItems;
                }

                // If it's a UIElement, add it to the screen.
                if (uie != null)
                {
                    Children.Add(uie);
                }
            }
        }

        private void RemoveViews(IList views)
        {
            foreach (object view in views)
            {
                // Look for known interfaces
                IARView arView = view as IARView;
                IARItemsView itemView = view as IARItemsView;
                UIElement uie = view as UIElement;

                // If it's an IARView (which it always should be) disconnect current values
                if (arView != null)
                {
                    // Set current values
                    arView.VideoSource = null;
                }

                // If it's an IARItemsView disconnect the list of current items
                if (itemView != null)
                {
                    itemView.ARItems = null;
                }

                // If it's a UIElement, remove it from the screen.
                if (uie != null)
                {
                    Children.Remove(uie);
                }
            }
        }

        #if WP7
        private void StartCamera()
        {
            // If the camera hasn't been created yet, create it.
            if (photoCamera == null)
            {
                photoCamera = new PhotoCamera();
            }

            // If our video brush hasn't been created yet, create it
            VideoBrush vb = VideoSource as VideoBrush;
            if (vb == null)
            {
                CreateVideoBrush();
                vb = VideoSource as VideoBrush;
            }

            // Connect the video brush to the camera
            vb.SetSource(photoCamera);
        }
        #else
        private async Task StartCamera()
        {
            if (VideoSource == null)
            {
                var source = new Windows.Media.Capture.MediaCapture();
                try
                {
                    await source.InitializeAsync();
                    VideoSource = source;
                    await source.StartPreviewAsync();
                }
                catch (Exception ex)
                {
                    OnServiceError(new ServiceErrorEventArgs(ARService.Camera, ex));
                }
            }
        }
        #endif

        private void StartLocation()
        {
            // If the Location object is null, initialize it and add a CurrentValueChanged
            // event handler.
            if (location == null)
            {
                #if WP7

                location = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                location.MovementThreshold = 0; // TODO: Do we leave this? High battery cost but most accurate AR simulation
                location.PositionChanged += location_PositionChanged;

                // Try to start the Motion API.
                try
                {
                    location.Start();
                }
                catch (Exception ex)
                {
                    OnServiceError(new ServiceErrorEventArgs(ARService.Location, ex));
                }

                #else

                location = new Geolocator();
                location.MovementThreshold = 0; // TODO: Do we leave this? High battery cost but most accurate AR simulation
                location.PositionChanged += location_PositionChanged;
                
                #endif
            }
        }


        private void StartMotion()
        {
            #if WP7

            if (Motion.IsSupported)
            {
                // If the Motion object is null, initialize it and add a CurrentValueChanged
                // event handler.
                if (motion == null)
                {
                    motion = new Motion();
                    motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                    motion.CurrentValueChanged += motion_CurrentValueChanged;
                }

                // Try to start the Motion API.
                try
                {
                    motion.Start();
                }
                catch (Exception ex)
                {
                    OnServiceError(new ServiceErrorEventArgs(ARService.Motion, ex));
                }
            }
            else
            {
                OnServiceError(new ServiceErrorEventArgs(ARService.Motion, new InvalidOperationException("The Motion API is not supported on this device.")));
            }
            
            #else

            motion = Inclinometer.GetDefault();
            if (motion != null)
            {
                motion.ReportInterval = 20;
                motion.ReadingChanged += motion_ReadingChanged;
            }
            else
            {
                OnServiceError(new ServiceErrorEventArgs(ARService.Motion, new InvalidOperationException("Inclinometer is not supported on this device.")));
            }

            #endif
        }

        private void StopCamera()
        {
            #if WP7
            if (photoCamera != null)
            {
                photoCamera.Dispose();
                photoCamera = null;
            }
            #else
            VideoSource = null;
            #endif
        }

        private void StopLocation()
        {
            location.PositionChanged -= location_PositionChanged;

            #if WP7
            location.Stop();
            location.Dispose();
            #endif
            
            Location = null;
        }

        private void StopMotion()
        {
            #if WP7
            motion.CurrentValueChanged -= motion_CurrentValueChanged;
            motion.Stop();
            motion.Dispose();
            #else
            motion.ReadingChanged -= motion_ReadingChanged;
            #endif

            motion = null;
        }
        #endregion // Internal Methods

        #region Overrides / Event Handlers
        #if WP7
        private void location_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // This event arrives on a background thread. Use BeginInvoke to call
            // CurrentValueChanged on the UI thread.
            Dispatcher.BeginInvoke(() =>
            {
                // Update ourslves which will in turn update all the views
                var loc = e.Position.Location;
                this.Location = loc; // new Location(loc.Latitude, loc.Longitude, loc.Altitude, loc.HorizontalAccuracy, loc.VerticalAccuracy);
                this.TravelHeading = loc.Course;
            });
        }
        #else
        private void location_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            // Update ourslves which will in turn update all the views
            var loc = e.Position.Coordinate;
            this.Location = new Location(loc.Latitude, loc.Longitude);
            this.TravelHeading = loc.Heading ?? 0; // Force to 0 degrees if unknown.
        }
        #endif

        #if WP7
        private void motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            // This event arrives on a background thread. Use BeginInvoke to call
            // CurrentValueChanged on the UI thread.
            Dispatcher.BeginInvoke(() =>
            {
                MotionReading mr = e.SensorReading;

                // Update ourslves which will in turn update all the views
                this.Attitude = mr.Attitude.RotationMatrix;
                this.AttitudeHeading = MathHelper.ToDegrees(mr.Attitude.Yaw);
            });
        }
        #else
        private void motion_ReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
        {
            // This event arrives on a background thread. Use Dispatcher to call
            // CurrentValueChanged on the UI thread.
            #if WP7
            Dispatcher.BeginInvoke(() =>
            #else
            var t = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            #endif 
                {
                    // PORT: How do we do setup a rotation matrix? Is it even used?
                    // this.Attitude = mr.Attitude.RotationMatrix; 
                    this.AttitudeHeading = args.Reading.YawDegrees;
                });
        }
        #endif

        private void views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Add or remove from view container?
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AddViews(e.NewItems);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveViews(e.NewItems);
            }
            // TODO: Link or unlink the items collection.
        }
        #endregion // Overrides / Event Handlers

        #region Overridables / Event Triggers
        /// <summary>
        /// Occurs when the value of the <see cref="ARItems"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnARItemsChanged(DependencyPropertyChangedEventArgs e)
        {
            // Calculate the items locations before updating views
            CalculateItemLocations();

            // Update views
            foreach (IARView view in views)
            {
                IARItemsView itemsView = view as IARItemsView;
                if (itemsView != null)
                {
                    itemsView.ARItems = this.ARItems;
                }
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Attitude"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnAttitudeChanged(DependencyPropertyChangedEventArgs e)
        {
            // Update views
            foreach (IARView view in views)
            {
                view.Attitude = this.Attitude;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="CameraEnabled"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnCameraEnabledChanged(DependencyPropertyChangedEventArgs e)
        {
            // If services ar running, attemt to start or stop the camera
            if (servicesRunning)
            {
                if (CameraEnabled)
                {
                    var t = StartCamera();
                }
                else
                {
                    StopCamera();
                }
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="AttitudeHeading"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnAttitudeHeadingChanged(DependencyPropertyChangedEventArgs e)
        {
            // Update views
            foreach (IARView view in views)
            {
                view.AttitudeHeading = this.AttitudeHeading;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Location"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnLocationChanged(DependencyPropertyChangedEventArgs e)
        {
            // Update views
            foreach (IARView view in views)
            {
                view.Location = this.Location;
            }

            // Run any necessary calculations on the items 
            CalculateItemLocations();

            // Notify subscribers
            if (LocationChanged != null)
            {
                LocationChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="LocationEnabled"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnLocationEnabledChanged(DependencyPropertyChangedEventArgs e)
        {
            // If services ar running, attemt to start or stop Location tracking
            if (servicesRunning)
            {
                if (LocationEnabled)
                {
                    StartLocation();
                }
                else
                {
                    StopLocation();
                }
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="MotionEnabled"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnMotionEnabledChanged(DependencyPropertyChangedEventArgs e)
        {
            // If services ar running, attemt to start or stop motion tracking
            if (servicesRunning)
            {
                if (MotionEnabled)
                {
                    StartMotion();
                }
                else
                {
                    StopMotion();
                }
            }
        }

        /// <summary>
        /// Occurs when an error was encountered starting or stopping a service.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnServiceError(ServiceErrorEventArgs e)
        {
            if (ServiceError != null)
            {
                ServiceError(this, e);
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="TravelHeading"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnTravelHeadingChanged(DependencyPropertyChangedEventArgs e)
        {
            // Update views
            foreach (IARView view in views)
            {
                view.TravelHeading = this.TravelHeading;
            }
        }

        /// <summary>
        /// Occurs when the value of the <see cref="VideoSource"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnVideoSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            // Update views
            foreach (IARView view in views)
            {
                view.VideoSource = this.VideoSource;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnOrientationChanged(DependencyPropertyChangedEventArgs e)
        {
            foreach (IARView view in views)
            {
                view.Orientation = this.Orientation;
            }

            // Need to handle videobrush rotation as well:
            ControlOrientation newOrientation = (ControlOrientation)(e.NewValue);

            #if WP7
            CompositeTransform orientationRotation; 
            switch (newOrientation)
            {
                case ControlOrientation.LandscapeLeft:
                    orientationRotation = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 0 };
                    break;

                case ControlOrientation.LandscapeRight:
                    orientationRotation = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = landscapeRightRotation };
                    break;

                default:
                    orientationRotation = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 90 };
                    break;
            } // end switch 

            // Update our dependency property
            VideoSource.RelativeTransform = orientationRotation;

            #else
            VideoRotation orientationRotation = VideoRotation.None;
            switch (newOrientation)
            {
                case ControlOrientation.PortraitDown:
                    orientationRotation = VideoRotation.Clockwise90Degrees;
                    break;
                case ControlOrientation.PortraitUp:
                    orientationRotation = VideoRotation.Clockwise270Degrees;
                    break;
                case ControlOrientation.LandscapeLeft:
                    orientationRotation = VideoRotation.None;
                    break;

                case ControlOrientation.LandscapeRight:
                    orientationRotation = VideoRotation.Clockwise180Degrees;
                    break;
            } // end switch 

            VideoSource.SetPreviewRotation(orientationRotation);
            #endif
        }
        #endregion // Overridables / Event Triggers

        #region Public Methods
        /// <summary>
        /// Starts any enabled AR services (Motion, Camera, etc)
        /// </summary>
        public void StartServices()
        {
            // If services are already started, ignore
            if (servicesRunning) { return; }

            if (CameraEnabled)
            {
                var t = StartCamera();
            }
            if (LocationEnabled)
            {
                StartLocation();
            }
            if (MotionEnabled)
            {
                StartMotion();
            }

            // Started
            servicesRunning = true;
        }

        public void HandleOrientationChange(ControlOrientation newOrientation)
        {
            Orientation = newOrientation;
        }

        /// <summary>
        /// Stops any enabled AR services (Motion, Camera, etc)
        /// </summary>
        public void StopServices()
        {
            // If services are not started, ignore
            if (!servicesRunning) { return; }

            if (CameraEnabled)
            {
                StopCamera();
            }
            if (LocationEnabled)
            {
                StopLocation();
            }
            if (MotionEnabled)
            {
                StopMotion();
            }

            // Not started
            servicesRunning = false;
        }
        #endregion // Public Methods

        #region Public Properties
        /// <summary>
        /// Gets or sets the collection of ARItems rendered by the <see cref="ARDisplay"/>. This is a dependency property.
        /// </summary>
        /// <value>
        /// The collection of ARItems rendered by the <see cref="ARDisplay"/>.
        /// </value>
        #if WP7
        [Category("AR")]
        #endif
        public ObservableCollection<ARItem> ARItems
        {
            get
            {
                return (ObservableCollection<ARItem>)GetValue(ARItemsProperty);
            }
            set
            {
                SetValue(ARItemsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a matrix that represents where the user is looking. This is a dependency property.
        /// </summary>
        /// <value>
        /// A matrix that represents where the user is looking.
        /// </value>
        #if WP7
        [Category("AR")]
        #endif
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
        /// Gets or sets a value that indicates if the camera preview is enabled. This is a dependency property.
        /// </summary>
        /// <value>
        /// <c>true</c> if the camera preview is enabled; otherwise <c>false</c>.
        /// </value>
        #if WP7
        [Category("AR")]
        #endif
        public bool CameraEnabled
        {
            get
            {
                return (bool)GetValue(CameraEnabledProperty);
            }
            set
            {
                SetValue(CameraEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the direction the user is looking in degrees. This is a dependency property.
        /// </summary>
        /// <value>
        /// The direction the user is looking in degrees.
        /// </value>
        #if WP7
        [Category("AR")]
        #endif
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
        #if WP7
        [Category("AR")]
        #endif
        public Location Location
        {
            get
            {
                return (Location)GetValue(LocationProperty);
            }
            set
            {
                SetValue(LocationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates if Location tracking is enabled. This is a dependency property.
        /// </summary>
        /// <value>
        /// <c>true</c> if Location tracking is enabled; otherwise <c>false</c>.
        /// </value>
        #if WP7
        [Category("AR")]
        #endif
        public bool LocationEnabled
        {
            get
            {
                return (bool)GetValue(LocationEnabledProperty);
            }
            set
            {
                SetValue(LocationEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets the <see cref="Motion"/> instance used by the ARDisplay.
        /// </summary>
        #if WP7
        [Category("AR")]
        #endif
        public Motion Motion
        {
            get
            {
                return motion;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates if motion tracking is enabled. This is a dependency property.
        /// </summary>
        /// <value>
        /// <c>true</c> if motion tracking is enabled; otherwise <c>false</c>.
        /// </value>
        #if WP7
        [Category("AR")]
        #endif
        public bool MotionEnabled
        {
            get
            {
                return (bool)GetValue(MotionEnabledProperty);
            }
            set
            {
                SetValue(MotionEnabledProperty, value);
            }
        }


        #if WP7
        [Category("AR")]
        #endif
        public ControlOrientation Orientation
        {
            get { return (ControlOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets the PhotoCamera used by the ARDisplay.
        /// </summary>
        #if WP7
        [Category("AR")]
        public PhotoCamera PhotoCamera
        {
            get
            {
                return photoCamera;
            }
        }
        #endif

        /// <summary>
        /// Gets a value that indicates if AR services (Motion, Camera, etc.) have been started.
        /// </summary>
        /// <value><c>true</c> if AR services have been started; otherwise <c>false</c>.</value>
        #if WP7
        [Category("AR")]
        #endif
        public bool ServicesRunning
        {
            get
            {
                return servicesRunning;
            }
        }

        /// <summary>
        /// Gets or sets the direction the user is traveling in degrees. This is a dependency property.
        /// </summary>
        /// <value>
        /// The direction the user is traveling in degrees.
        /// </value>
        #if WP7
        [Category("AR")]
        #endif
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
        /// Gets or sets the video source for the camera. This is a dependency property.
        /// </summary>
        /// <value>
        /// The video source for the camera.
        /// </value>
        #if WP7
        [Category("AR")]
        #endif
        public VideoSource VideoSource
        {
            get
            {
                return (VideoSource)GetValue(VideoSourceProperty);
            }
            set
            {
                SetValue(VideoSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets the collection of child views associated with this display.
        /// </summary>
        #if WP7
        [Category("AR")]
        #endif
        public ObservableCollection<IARView> Views
        {
            get
            {
                return views;
            }
        }
        #endregion // Public Properties

        #region Public Events
        /// <summary>
        /// Occurs when the value of the <see cref="Location"/> property has changed.
        /// </summary>
        public event EventHandler LocationChanged;

        /// <summary>
        /// Occurs when an error was encountered starting or stopping a service.
        /// </summary>
        public event EventHandler<ServiceErrorEventArgs> ServiceError;
        #endregion // Public Events

        #endregion // Instance Version
    }

}
