using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace App2.ControlPanelPages
{
    public sealed partial class Settings : Page
    {
        CreateQueue x = new CreateQueue();
        Encryption encryption = new Encryption();

        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        readonly int videos_per_day = (int)localSettings.Values["videos_per_day"];
        readonly bool TimeRestriction_IsEnabled = (bool)localSettings.Values["TimeRestriction_IsEnabled"];
        readonly string controlPanelPassword = (string)localSettings.Values["controlPanelPassword"];

        public Settings()
        {
            this.InitializeComponent();

            InsertSliderToPage(); // insert slider for videos_per_day to the page.

            TimeRestrictionToggleSwitch.IsOn = TimeRestriction_IsEnabled;

            if (TimeRestriction_IsEnabled)
            {
                TimeFrom.Time = (TimeSpan)localSettings.Values["TimeRestriction_TimeFrom"];
                TimeTo.Time = (TimeSpan)localSettings.Values["TimeRestriction_TimeTo"];
            }
        }

        private void InsertSliderToPage()
        {
            /* 
             * Defining slider in xaml causes a havoc with it's Value init.
             * Therefore, this function.
             */
            Thickness sliderMargin = new Thickness(122, 2, 0, 0);

            Slider slider = new Slider() {
                Value = videos_per_day,     //initial value
                Maximum = 10,
                Minimum = 1,
                Margin = sliderMargin,
                Width = 300,
                VerticalAlignment = 0,
                HorizontalAlignment = 0,
                Name = "trackbar",
            };
            slider.ValueChanged += Slider_ValueChanged;

            settings.Children.Add(slider); // append to grid.
        }
        

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            localSettings.Values["videos_per_day"] = (int)e.NewValue;
        }

               
        //-----------------------------------//
        //		  Reset Cache File 			 //
        //-----------------------------------//
        private void ClearCache_Click(object sender, RoutedEventArgs e)
        {
            using (System.IO.StreamWriter sw = File.CreateText(x.appDataPath + "/src/cache.txt"))
            {
                sw.WriteLine("00-00-0000");
            }
            ClearCacheMark.Visibility = Visibility.Visible;
        }
        //****************END****************//


        //-----------------------------------//
        //		  Time Restriction 			 //
        //-----------------------------------//
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
        //****************END****************//


        //-----------------------------------//
        //		   Change Password 		     //
        //-----------------------------------//
        private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            // display change password dialog.
            await changePasswordDialog.ShowAsync();
        }

        private void OldPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (OldPassword.Password == encryption.DecryptString(controlPanelPassword))
            {
                NewPassword.IsEnabled = ConfirmPassword.IsEnabled = true;
                return;
            }
            NewPassword.IsEnabled = ConfirmPassword.IsEnabled = false;
        }

        private void ConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (NewPassword.Password == ConfirmPassword.Password)
            {
                changePasswordDialog.IsPrimaryButtonEnabled = true;
                return;
            }           
            changePasswordDialog.IsPrimaryButtonEnabled = false;
        }
        void contentDialog_Reset()
        {
            changePasswordDialog.IsPrimaryButtonEnabled = false;
            NewPassword.IsEnabled = ConfirmPassword.IsEnabled = false;
            NewPassword.Password = OldPassword.Password = ConfirmPassword.Password = "";
        }
        private void ApplyNewPassword_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            localSettings.Values["controlPanelPassword"] = encryption.EnryptString(NewPassword.Password);
            contentDialog_Reset();
        }

        private void CloseDialog_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            contentDialog_Reset();
        }
        //****************END****************//
    }
}
