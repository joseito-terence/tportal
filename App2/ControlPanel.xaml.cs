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

namespace App2
{  
    public sealed partial class ControlPanel : Page
    {
        public ControlPanel()
        {
            this.InitializeComponent();

            // Set XAML element as a draggable region.
            Window.Current.SetTitleBar(AppTitleBar);

            // Navigate to the Import page at startup.
            NavView.SelectedItem = NavView.MenuItems.ElementAt(0);
        }


        // Function to go back to main view i.e the video player.
        private void Back_Click(object sender, RoutedEventArgs e) 
        {
            //if (this.Frame.CanGoBack) this.Frame.GoBack();
            this.Frame.Navigate(typeof(MainPage));
        }

        /**********************************************************/
        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(ControlPanelPages.Settings));
                NavView.Header = "Settings";
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                switch (item.Tag.ToString())
                {
                    case "Import":
                        ContentFrame.Navigate(typeof(ControlPanelPages.Import));
                        NavView.Header = "Import";
                        break;
                    case "Queue":
                        ContentFrame.Navigate(typeof(ControlPanelPages.Queue));
                        NavView.Header = "Queue";
                        break;
                }
            }
        }
    }
}
