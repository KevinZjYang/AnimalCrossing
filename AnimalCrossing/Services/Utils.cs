using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AnimalCrossing.Services
{
    public static class Utils
    {
        public static void RunOnDispatcher(this DependencyObject d, Action a)
        {
            if (d.Dispatcher.HasThreadAccess)
                a();
            else
                _ = d.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => a());
        }
    }
}
