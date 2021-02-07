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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace App2.SetupScreens
{
    public sealed partial class SetupScreen3 : Page
    {
        Encryption encryption = new Encryption();
        CreateQueue x = new CreateQueue();
        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public SetupScreen3()
        {
            this.InitializeComponent();
        }

        private void ConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Password.Password == ConfirmPassword.Password)
            {
                // Init localSettings variables
                localSettings.Values["controlPanelPassword"] = encryption.EnryptString(Password.Password);
                localSettings.Values["videos_per_day"] = 1;
                localSettings.Values["TimeRestriction_IsEnabled"] = false;

                // Create Required directories.
                Directory.CreateDirectory(x.appDataPath + "\\src");
                Directory.CreateDirectory(x.appDataPath + "\\videos");

                // Create Required Files.
                using (File.Create(x.appDataPath + "\\src\\cache.txt")) { }
                using (File.Create(x.appDataPath + "\\src\\queue.txt")) { }                
            }
                

            FirstLaunchScreen.NextButton.IsEnabled = (Password.Password == ConfirmPassword.Password);   
        }
    }
}
