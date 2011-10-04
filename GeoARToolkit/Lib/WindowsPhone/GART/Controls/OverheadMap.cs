﻿#region License
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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Phone.Controls.Maps;
using Matrix = Microsoft.Xna.Framework.Matrix;
using System.ComponentModel;
using System.Device.Location;
using System.Collections.ObjectModel;
using GART.Data;
using Microsoft.Phone.Controls.Maps.Design;

namespace GART.Controls
{
    [TemplatePart(Name = OverheadMap.PartNames.Map, Type = typeof(Map))]
    public class OverheadMap : ARRotateView, IARItemsView
    {
        #region Static Version
        #region Part Names
        static internal class PartNames
        {
            public const string Map = "Map";
        }
        #endregion // Part Names

        #region Dependency Properties
        /// <summary>
        /// Identifies the <see cref="ZoomLevel"/> dependency property.
        /// </summary>
        static public readonly DependencyProperty ZoomLevelProperty = DependencyProperty.Register("ZoomLevel", typeof(double), typeof(OverheadMap), new PropertyMetadata(.85d, OnZoomLevelChanged));

        private static void OnZoomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OverheadMap)d).OnZoomLevelChanged(e);
        }
        #endregion // Dependency Properties
        #endregion // Static Version

        #region Instance Version
        #region Member Variables
        private CredentialsProvider credentialsProvider;
        private ObservableCollection<ARItem> arItems;
        private Map map;
        #endregion // Member Variables

        #region Constructors
        public OverheadMap()
        {
            DefaultStyleKey = typeof(OverheadMap);

            // Subscribe to LayoutUpdated so we can clip the map correctly.
            // This is necessary because we have to make the map larger than our bounds so rotation works.
            this.LayoutUpdated += new EventHandler(OverheadMap_LayoutUpdated);
        }
        #endregion // Constructors

        #region Overrides / Event Handlers
        private void map_MapPan(object sender, MapDragEventArgs e)
        {
            // Disable panning
            e.Handled = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            map = GetTemplateChild(PartNames.Map) as Map;

            // Validate the template
            if (map == null)
            {
                throw new InvalidOperationException(string.Format("{0} template is invalid. A {1} named {2} must be supplied.", GetType().Name, typeof(Map).Name, PartNames.Map));
            }

            // Keep the map from panning
            map.MapPan += new EventHandler<MapDragEventArgs>(map_MapPan);
            map.ManipulationStarted += new EventHandler<System.Windows.Input.ManipulationStartedEventArgs>(map_ManipulationStarted);
            map.ManipulationDelta += new EventHandler<System.Windows.Input.ManipulationDeltaEventArgs>(map_ManipulationDelta);
            map.ManipulationCompleted += new EventHandler<System.Windows.Input.ManipulationCompletedEventArgs>(map_ManipulationCompleted);

            // Connect credentials
            map.CredentialsProvider = credentialsProvider;

            // Connect data
            map.DataContext = arItems;
        }

        void map_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Manip Completed");
        }

        void map_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Manip Completed");
        }

        void map_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Manip Completed");
        }

        private void OverheadMap_LayoutUpdated(object sender, EventArgs e)
        {
            // Clip all children so the map doesn't get drawn outside our bounds
            RectangleGeometry clipGeometry = new RectangleGeometry();
            clipGeometry.Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            this.Clip = clipGeometry;
        }
        #endregion // Overrides / Event Handlers

        #region Overridables / Event Triggers
        /// <summary>
        /// Occurs when the value of the <see cref="ZoomLevel"/> property has changed.
        /// </summary>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> containing event information.
        /// </param>
        protected virtual void OnZoomLevelChanged(DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion // Overridables / Event Triggers

        #region Public Properties
        /// <summary>
        /// Gets or sets the collection of ARItem objects that should be rendered in the view.
        /// </summary>
        [Category("AR")]
        public ObservableCollection<ARItem> ARItems
        {
            get
            {
                return arItems;
            }
            set
            {
                arItems = value;
                if (map != null)
                {
                    map.DataContext = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the credentials provider used by the underlying map.
        /// </summary>
        [Category("Map")]
        [TypeConverter(typeof(ApplicationIdCredentialsProviderConverter))]
        public CredentialsProvider CredentialsProvider
        {
            get
            {
                return credentialsProvider;
            }
            set
            {
                credentialsProvider = value;
                if (map != null)
                {
                    map.CredentialsProvider = value;
                }
            }
        }


        /// <summary>
        /// Gets the <see cref="Map"/> instance used by the OverheadMap.
        /// </summary>
        [Category("Map")]
        public Map Map
        {
            get
            {
                return map;
            }
        }

        /// <summary>
        /// Gets or sets the ZoomLevel of the <see cref="OverheadMap"/>. This is a dependency property.
        /// </summary>
        /// <value>
        /// The ZoomLevel of the <see cref="OverheadMap"/>. 0 is zoomed out completely and 1 is zoomed in completely.
        /// </value>
        [Category("Map")]
        public double ZoomLevel
        {
            get
            {
                return (double)GetValue(ZoomLevelProperty);
            }
            set
            {
                SetValue(ZoomLevelProperty, value);
            }
        }
        #endregion // Public Properties
        #endregion // Instance Version


    }
}
