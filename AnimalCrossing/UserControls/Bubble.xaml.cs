using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace AnimalCrossing.Controls
{
    public sealed partial class Bubble : UserControl
    {
        public Bubble()
        {
            InitializeComponent();

            BottomRectangles.Add(Rectangle1);
            BottomRectangles.Add(Rectangle2);

            Loaded += OnLoaded;
        }

        private List<Rectangle> BottomRectangles { get; } = new List<Rectangle>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var compositor = Window.Current.Compositor;

            var rotationAnimation = compositor.CreateScalarKeyFrameAnimation();
            var linear = compositor.CreateLinearEasingFunction();
            rotationAnimation.InsertKeyFrame(1.0f, 360, linear);
            rotationAnimation.Duration = TimeSpan.FromSeconds(9);
            rotationAnimation.Target = nameof(Rectangle1.Rotation);
            rotationAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            Rectangle1.CenterPoint = new Vector3((float)Rectangle1.Width / 2, (float)Rectangle1.Height / 2, 0);
            Rectangle2.CenterPoint = new Vector3((float)Rectangle2.Width / 2, (float)Rectangle2.Height / 2, 0);

            Rectangle1.StartAnimation(rotationAnimation);

            rotationAnimation.Duration = TimeSpan.FromSeconds(8);

            Rectangle2.StartAnimation(rotationAnimation);
        }
    }
}
