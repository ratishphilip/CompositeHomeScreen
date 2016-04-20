using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CompositeHomeScreen
{
    public sealed partial class MenuButton : UserControl
    {
        #region ImageUrl

        /// <summary>
        /// ImageUrl Dependency Property
        /// </summary>
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register("ImageUrl", typeof(Uri), typeof(MenuButton),
                new PropertyMetadata(null, OnImageUrlChanged));

        /// <summary>
        /// Gets or sets the ImageUrl property. This dependency property 
        /// indicates the Uri of the image to be displayed in the Menu Button.
        /// </summary>
        public Uri ImageUrl
        {
            get { return (Uri)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ImageUrl property.
        /// </summary>
        /// <param name="d">MenuButton</param>
		/// <param name="e">DependencyProperty changed event arguments</param>
        private static void OnImageUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = (MenuButton)d;
            var oldImageUrl = (Uri)e.OldValue;
            var newImageUrl = button.ImageUrl;
            button.OnImageUrlChanged(oldImageUrl, newImageUrl);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ImageUrl property.
        /// </summary>
		/// <param name="oldImageUrl">Old Value</param>
		/// <param name="newImageUrl">New Value</param>
        void OnImageUrlChanged(Uri oldImageUrl, Uri newImageUrl)
        {
            AppImage.Source = newImageUrl;
        }

        #endregion

        #region PageType

        /// <summary>
        /// PageType Dependency Property
        /// </summary>
        public static readonly DependencyProperty PageTypeProperty =
            DependencyProperty.Register("PageType", typeof(Type), typeof(MenuButton),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the PageType property. This dependency property 
        /// indicates the type of page to navigate to when this button is clicked.
        /// </summary>
        public Type PageType
        {
            get { return (Type)GetValue(PageTypeProperty); }
            set { SetValue(PageTypeProperty, value); }
        }

        #endregion

        #region AppName

        /// <summary>
        /// AppName Dependency Property
        /// </summary>
        public static readonly DependencyProperty AppNameProperty =
            DependencyProperty.Register("AppName", typeof(string), typeof(MenuButton),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the AppName property. This dependency property 
        /// indicates the AppName to be displayed in the content page (which is
        /// displayed when this button is clicked).
        /// </summary>
        public string AppName
        {
            get { return (string)GetValue(AppNameProperty); }
            set { SetValue(AppNameProperty, value); }
        }

        #endregion

        public MenuButton()
        {
            this.InitializeComponent();
        }
    }
}
