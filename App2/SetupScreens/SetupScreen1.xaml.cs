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

namespace App2.SetupScreens
{
    public sealed partial class SetupScreen1 : Page
    {
        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public SetupScreen1()
        {
            this.InitializeComponent();
        }

        private void trackbar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            localSettings.Values["videos_per_day"] = (int)e.NewValue;
        }
    }
}
