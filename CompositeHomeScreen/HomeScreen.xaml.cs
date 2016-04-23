using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CompositionExpressionToolkit;
using CompositionTests.Pages;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CompositeHomeScreen
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeScreen : Page
    {
        private const double MenuGridScaleUpDuration = 0.45;
        private const double MenuGridScaleDownDuration = 0.35;
        private const double FadeInMenuDuration = 0.45;
        private const double FadeOutMenuDuration = 0.35;
        private const double FadeInDuration = 0.45;
        private const double FadeOutDuration = 0.35;
        private const double ItemContainerSize = 100;
        private const double ItemSize = 70;

        private Random _random = new Random();
        private float _scaleX;
        private float _scaleY;

        private Compositor _compositor;
        private Vector2KeyFrameAnimation _scaleUpMenuAnimation;
        private Vector2KeyFrameAnimation _scaleDownMenuAnimation;
        private Vector2KeyFrameAnimation _scaleUpAnimation;
        private Vector2KeyFrameAnimation _scaleDownAnimation;
        private Vector2KeyFrameAnimation _scaleUpContentAnimation;
        private Vector2KeyFrameAnimation _scaleDownContentAnimation;
        private Vector3KeyFrameAnimation _offsetUpAnimation;
        private Vector3KeyFrameAnimation _offsetDownAnimation;
        private ScalarKeyFrameAnimation _fadeOutMenuAnimation;
        private ScalarKeyFrameAnimation _fadeInMenuAnimation;
        private ScalarKeyFrameAnimation _fadeOutAnimation;
        private ScalarKeyFrameAnimation _fadeInAnimation;
        private ScalarKeyFrameAnimation _fadeOutContentAnimation;
        private ScalarKeyFrameAnimation _fadeInContentAnimation;

        private Visual menuGridVisual;
        private Visual transitionContentVisual;
        private SpriteVisual transitionBGVisual;

        // Flag to indicate whether the page should be loaded during animation
        private bool _loadContentDuringAnimation = true;
        private bool _isContentDisplayed = false;

        public ObservableCollection<MenuButton> Items { get; set; }

        public HomeScreen()
        {
            this.InitializeComponent();
            Items = new ObservableCollection<MenuButton>()
            {
                new MenuButton() { AppName = "AppStore",    PageType = typeof(AppStorePage),    ImageUrl = new Uri("ms-appx:///Assets/Icons/AppStore.png"),     Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Calculator",  PageType = typeof(CalculatorPage),  ImageUrl = new Uri("ms-appx:///Assets/Icons/Calculator.png"),   Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Camera",      PageType = typeof(CameraPage),      ImageUrl = new Uri("ms-appx:///Assets/Icons/Camera.png"),       Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Clock",       PageType = typeof(ClockPage),       ImageUrl = new Uri("ms-appx:///Assets/Icons/Clock.png"),        Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Contacts",    PageType = typeof(ContactsPage),    ImageUrl = new Uri("ms-appx:///Assets/Icons/Contacts.png"),     Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "FaceTime",    PageType = typeof(FaceTimePage),    ImageUrl = new Uri("ms-appx:///Assets/Icons/FaceTime.png"),     Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Game Center", PageType = typeof(GameCenterPage),  ImageUrl = new Uri("ms-appx:///Assets/Icons/GameCenter.png"),   Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "iBooks",      PageType = typeof(iBooksPage),      ImageUrl = new Uri("ms-appx:///Assets/Icons/iBooks.png"),       Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "iCloud",      PageType = typeof(iCloudPage),      ImageUrl = new Uri("ms-appx:///Assets/Icons/iCloud.png"),       Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Maps",        PageType = typeof(MapsPage),        ImageUrl = new Uri("ms-appx:///Assets/Icons/Maps.png"),         Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Music",       PageType = typeof(MusicPage),       ImageUrl = new Uri("ms-appx:///Assets/Icons/Music.png"),        Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "News",        PageType = typeof(NewsPage),        ImageUrl = new Uri("ms-appx:///Assets/Icons/News.png"),         Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Notes",       PageType = typeof(NotesPage),       ImageUrl = new Uri("ms-appx:///Assets/Icons/Notes.png"),        Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Photos",      PageType = typeof(PhotosPage),      ImageUrl = new Uri("ms-appx:///Assets/Icons/Photos.png"),       Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Reminders",   PageType = typeof(RemindersPage),   ImageUrl = new Uri("ms-appx:///Assets/Icons/Reminders.png"),    Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Settings",    PageType = typeof(SettingsPage),    ImageUrl = new Uri("ms-appx:///Assets/Icons/Settings.png"),     Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Skype",       PageType = typeof(SkypePage),       ImageUrl = new Uri("ms-appx:///Assets/Icons/Skype.png"),        Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Wallet",      PageType = typeof(WalletPage),      ImageUrl = new Uri("ms-appx:///Assets/Icons/Wallet.png"),       Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Weather",     PageType = typeof(WeatherPage),     ImageUrl = new Uri("ms-appx:///Assets/Icons/Weather.png"),      Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "YouTube",     PageType = typeof(YoutubePage),     ImageUrl = new Uri("ms-appx:///Assets/Icons/Youtube.png"),      Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Phone",       PageType = typeof(PhonePage),       ImageUrl = new Uri("ms-appx:///Assets/Icons/Phone.png"),        Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Messages",    PageType = typeof(MessagesPage),    ImageUrl = new Uri("ms-appx:///Assets/Icons/Messages.png"),     Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Mail",        PageType = typeof(MailPage),        ImageUrl = new Uri("ms-appx:///Assets/Icons/Mail.png"),         Width= ItemSize, Height= ItemSize },
                new MenuButton() { AppName = "Safari",      PageType = typeof(SafariPage),      ImageUrl = new Uri("ms-appx:///Assets/Icons/Safari.png"),       Width= ItemSize, Height= ItemSize }
                };

            var row = 0;
            var col = 0;
            foreach (var item in Items)
            {
                item.PointerPressed += OnMenuButtonClicked;
                Grid.SetRow(item, row);
                Grid.SetColumn(item, col);
                MenuGrid.Children.Add(item);
                col = (col + 1) % 4;
                if (col == 0)
                {
                    row++;
                    if (row == 5)
                        row++;
                }
            }

            _isContentDisplayed = false;
        }

        private void OnHomeScreenSizeChanged(object sender, SizeChangedEventArgs e)
        {
            HomeScreenClip.Rect = new Rect(0d, 0d, e.NewSize.Width, e.NewSize.Height);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            InitializeComposition();
        }

        private void InitializeComposition()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            menuGridVisual = ElementCompositionPreview.GetElementVisual(MenuGrid);

            var scaleFactor = 5f;
            _scaleUpMenuAnimation = _compositor.CreateVector2KeyFrameAnimation();
            _scaleUpMenuAnimation.InsertKeyFrame(1, new Vector2(scaleFactor, scaleFactor));
            _scaleUpMenuAnimation.Duration = TimeSpan.FromSeconds(MenuGridScaleUpDuration);

            _scaleDownMenuAnimation = _compositor.CreateVector2KeyFrameAnimation();
            _scaleDownMenuAnimation.InsertKeyFrame(1, Vector2.One);
            _scaleDownMenuAnimation.Duration = TimeSpan.FromSeconds(MenuGridScaleDownDuration);

            _scaleX = (float)(400.00 / (ItemSize / 1f));
            _scaleY = (float)(700.0 / (ItemSize / 1f));
            _scaleUpAnimation = _compositor.CreateVector2KeyFrameAnimation();
            _scaleUpAnimation.InsertKeyFrame(1, new Vector2(_scaleX, _scaleY));
            _scaleUpAnimation.Duration = TimeSpan.FromSeconds(MenuGridScaleUpDuration);

            _scaleDownAnimation = _compositor.CreateVector2KeyFrameAnimation();
            _scaleDownAnimation.InsertKeyFrame(1, Vector2.One);
            _scaleDownAnimation.Duration = TimeSpan.FromSeconds(MenuGridScaleDownDuration);

            _scaleUpContentAnimation = _compositor.CreateVector2KeyFrameAnimation();
            _scaleUpContentAnimation.InsertKeyFrame(1, Vector2.One);
            _scaleUpContentAnimation.Duration = TimeSpan.FromSeconds(MenuGridScaleUpDuration);

            _scaleDownContentAnimation = _compositor.CreateVector2KeyFrameAnimation();
            _scaleDownContentAnimation.InsertKeyFrame(1, new Vector2(1 / _scaleX, 1 / _scaleY));
            _scaleDownContentAnimation.Duration = TimeSpan.FromSeconds(MenuGridScaleDownDuration);

            _offsetUpAnimation = _compositor.CreateVector3KeyFrameAnimation();
            _offsetUpAnimation.InsertKeyFrame(1, Vector3.Zero);
            _offsetUpAnimation.Duration = TimeSpan.FromSeconds(MenuGridScaleUpDuration);

            _offsetDownAnimation = _compositor.CreateVector3KeyFrameAnimation();
            _offsetDownAnimation.Duration = TimeSpan.FromSeconds(MenuGridScaleDownDuration);

            _fadeInMenuAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _fadeInMenuAnimation.InsertKeyFrame(1f, 1);
            _fadeInMenuAnimation.Duration = TimeSpan.FromSeconds(FadeInMenuDuration);

            _fadeOutMenuAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _fadeOutMenuAnimation.InsertKeyFrame(0.5f, 0f);
            _fadeOutMenuAnimation.Duration = TimeSpan.FromSeconds(FadeOutMenuDuration);

            _fadeInAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _fadeInAnimation.InsertKeyFrame(0.5f, 1f, _compositor.CreateLinearEasingFunction());
            _fadeInAnimation.Duration = TimeSpan.FromSeconds(FadeInDuration);

            _fadeOutAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _fadeOutAnimation.InsertKeyFrame(0.65f, 0);
            _fadeOutAnimation.Duration = TimeSpan.FromSeconds(FadeOutDuration);

            _fadeInContentAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _fadeInContentAnimation.InsertKeyFrame(0.5f, 1f, _compositor.CreateLinearEasingFunction());
            _fadeInContentAnimation.Duration = TimeSpan.FromSeconds(FadeInDuration);

            _fadeOutContentAnimation = _compositor.CreateScalarKeyFrameAnimation();
            _fadeOutContentAnimation.InsertKeyFrame(0.5f, 0);
            _fadeOutContentAnimation.Duration = TimeSpan.FromSeconds(FadeOutDuration);
        }

        private void OnMenuButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            var control = sender as MenuButton;
            if (control == null)
                return;

            // Find location of control within MenuGrid
            GeneralTransform coordinate = control.TransformToVisual(MenuGrid);
            Point position = coordinate.TransformPoint(new Point(0, 0));
            var offset = new Vector3((float)position.X, (float)position.Y, 0);
            var centerPoint = new Vector3((float)(position.X + (control.Width / 2.0)),
                                          (float)(position.Y + (control.Height / 2.0)),
                                          0);
            // MenuGrid Center Point
            menuGridVisual.CenterPoint = centerPoint;
            // Transition Content Visual
            transitionContentVisual = ElementCompositionPreview.GetElementVisual(ContentGrid);
            transitionContentVisual.Scale = new Vector3(1 / _scaleX, 1 / _scaleY, 0);
            transitionContentVisual.Offset = offset;
            //transitionContentVisual.CenterPoint = centerPoint;
            // Transition Background Visual
            transitionBGVisual = _compositor.CreateSpriteVisual();
            transitionBGVisual.Brush = _compositor.CreateColorBrush(Colors.White);
            transitionBGVisual.Size = new Vector2((float)ItemSize, (float)ItemSize);
            transitionBGVisual.Offset = offset;
            ElementCompositionPreview.SetElementChildVisual(CompositionGrid, transitionBGVisual);
            // Set _offsetDownAnimation's final keyframe
            _offsetDownAnimation.InsertKeyFrame(1, transitionBGVisual.Offset);
            ContentFrame.Content = null;
            ContentContainer.Visibility = Visibility.Visible;



            ScopedBatchHelper.CreateScopedBatch(_compositor, CompositionBatchTypes.Animation,
                () => // Action
                {
                    if (_loadContentDuringAnimation)
                    {
                        ContentFrame.Navigate(control.PageType, control.AppName);
                    }
                    transitionBGVisual.StartAnimation(nameof(transitionBGVisual.Offset), _offsetUpAnimation);
                    transitionBGVisual.StartAnimation("Scale.XY", _scaleUpAnimation);
                    transitionBGVisual.StartAnimation(nameof(transitionBGVisual.Opacity), _fadeInAnimation);
                    menuGridVisual.StartAnimation("Scale.XY", _scaleUpMenuAnimation);
                    menuGridVisual.StartAnimation("Opacity", _fadeOutMenuAnimation);
                    transitionContentVisual.StartAnimation(nameof(transitionBGVisual.Offset), _offsetUpAnimation);
                    transitionContentVisual.StartAnimation("Scale.XY", _scaleUpContentAnimation);
                    transitionContentVisual.StartAnimation(nameof(transitionBGVisual.Opacity), _fadeInContentAnimation);
                },
                () => // Post Action
                {
                    _isContentDisplayed = true;
                    if (!_loadContentDuringAnimation)
                    {
                        ContentFrame.Navigate(control.PageType, control.AppName);
                    }
                });

        }

        private void OnHomeButtonPressed(object sender, RoutedEventArgs e)
        {
            if (!_isContentDisplayed)
                return;

            ScopedBatchHelper.CreateScopedBatch(_compositor, CompositionBatchTypes.Animation,
                () => // Action
                {
                    transitionBGVisual.StartAnimation(nameof(transitionBGVisual.Offset), _offsetDownAnimation);
                    transitionBGVisual.StartAnimation("Scale.XY", _scaleDownAnimation);
                    transitionBGVisual.StartAnimation(nameof(transitionBGVisual.Opacity), _fadeOutAnimation);
                    menuGridVisual.StartAnimation("Scale.XY", _scaleDownMenuAnimation);
                    menuGridVisual.StartAnimation(nameof(menuGridVisual.Opacity), _fadeInMenuAnimation);
                    transitionContentVisual.CenterPoint = new Vector3();
                    transitionContentVisual.StartAnimation(nameof(transitionBGVisual.Offset), _offsetDownAnimation);
                    transitionContentVisual.StartAnimation("Scale.XY", _scaleDownContentAnimation);
                    transitionContentVisual.StartAnimation(nameof(transitionBGVisual.Opacity), _fadeOutContentAnimation);
                },
                () => // Post Action
                {
                    ElementCompositionPreview.SetElementChildVisual(CompositionGrid, null);
                    _isContentDisplayed = false;
                    ContentFrame.Content = null;
                    ContentContainer.Visibility = Visibility.Collapsed;
                });
        }
    }
}
