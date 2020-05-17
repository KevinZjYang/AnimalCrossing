using AnimalCrossing.Services;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AnimalCrossing
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ExtendedSplash : Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private SplashScreen splash; // Variable to hold the splash screen object.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;
        public static ExtendedSplash Current;

        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();
            Current = this;

            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            splash = splashscreen;

            RestoreState(loadState);
            Unloaded += ExtendedSplash_Unloaded;
        }

        private void ExtendedSplash_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);
        }

        private async void ExtendedSplashImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            if (splash != null)
            {
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                splashImageRect = splash.ImageLocation;

                PositionImage();
                PositionRing();
                PositionBubble();
            }
            rootFrame = new Frame();

            Window.Current.Activate();

            await InitializeAsync();
        }

        private void RestoreState(bool loadState)
        {
            if (loadState)
            {
                // TODO: write code to load state
            }
        }

        // Position the extended splash screen image in the same location as the system splash
        // screen image.
        private void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            extendedSplashImage.Height = splashImageRect.Height;
            extendedSplashImage.Width = splashImageRect.Width;
        }

        private void PositionBubble()
        {
            bubble.SetValue(Canvas.LeftProperty, splashImageRect.X);
            bubble.SetValue(Canvas.TopProperty, splashImageRect.Y);
            bubble.Height = splashImageRect.Height;
            bubble.Width = splashImageRect.Width;
        }

        private void PositionRing()
        {
            splashMessageTb.SetValue(Canvas.LeftProperty, splashImageRect.X + (splashImageRect.Width * 0.5) - (splashMessageTb.Width * 0.3));
            splashMessageTb.SetValue(Canvas.TopProperty, (splashImageRect.Y + splashImageRect.Height + splashImageRect.Height * 0.1));
        }

        private void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be
            // fired in response to snapping, unsnapping, rotation, etc...
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
                PositionRing();
                PositionBubble();
            }
        }

        // Include code to be executed when the system has transitioned from the splash screen to
        // the extended splash screen (application's first view).
        private void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;
        }

        private void DismissExtendedSplash()
        {
            // Navigate to mainpage
            rootFrame.Navigate(typeof(Views.MainPage));
            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
        }

        private async Task InitializeAsync()
        {
            await ActivationService.InitializeAsync();
            var result = await DbUpdateService.DownloadIfAppropriateAsync().ConfigureAwait(true);
            if (result)
            {
                DismissExtendedSplash();

                await ActivationService.StartupAsync();
            }
        }

        public void SetMessage(string message)
        {
            splashMessageTb.Text = message;
            //PositionRing();
        }

        private void DismissSplashButton_Click(object sender, RoutedEventArgs e)
        {
            DismissExtendedSplash();
        }
    }
}
