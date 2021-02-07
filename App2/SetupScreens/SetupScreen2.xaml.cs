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
    public sealed partial class SetupScreen2 : Page
    {
        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public SetupScreen2()
        {
            this.InitializeComponent();
        }

        private void TimeRestriction_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;

            if (toggleSwitch != null)
                localSettings.Values["TimeRestriction_IsEnabled"] = TimeFrom.IsEnabled = TimeTo.IsEnabled = toggleSwitch.IsOn;
        }

        private void TimeFrom_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            localSettings.Values["TimeRestriction_TimeFrom"] = TimeFrom.Time;
        }

        private void TimeTo_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            localSettings.Values["TimeRestriction_TimeTo"] = TimeTo.Time;
        }
    }
}
