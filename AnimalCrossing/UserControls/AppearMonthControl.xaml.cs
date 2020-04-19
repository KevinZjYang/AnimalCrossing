using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace AnimalCrossing.UserControls
{
    public sealed partial class AppearMonthControl : UserControl
    {
        public List<bool> AppearMonth
        {
            get { return (List<bool>)GetValue(AppearMonthProperty); }
            set { SetValue(AppearMonthProperty, value); }
        }

        public static readonly DependencyProperty AppearMonthProperty =
            DependencyProperty.Register("AppearMonth", typeof(List<bool>), typeof(AppearMonthControl), new PropertyMetadata(null));

        public AppearMonthControl()
        {
            this.InitializeComponent();
        }
    }
}
