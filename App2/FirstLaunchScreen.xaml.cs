using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


namespace App2
{
    public sealed partial class FirstLaunchScreen : Page
    {
        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        readonly SlideNavigationTransitionInfo SlideFromRightTransition = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
        readonly SlideNavigationTransitionInfo SlideFromLeftTransition  = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft  };

        int screenIndex = -1;
        readonly Type[] screens = {
            typeof(SetupScreens.SetupScreen1),
            typeof(SetupScreens.SetupScreen2),
            typeof(SetupScreens.SetupScreen3),
            typeof(SetupScreens.FinalScreen)
        };


        public static Button NextButton = new Button()
        {
            Margin = new Thickness(0, 0, 20, 20),
            Width = 100,
            Background = new SolidColorBrush(Colors.DodgerBlue),
            VerticalAlignment = VerticalAlignment.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Visibility = Visibility.Collapsed,
            Content = "Next",
            Name = "NextButton"
        };

        public FirstLaunchScreen()
        {
            this.InitializeComponent();

            NextButton.Click += NextBtn_Click;
            EmptyGrid.Children.Add(NextButton);
        }

        private void GetStarted_Click(object sender, RoutedEventArgs e)
        {
            screenIndex++;
            ContentFrame.Navigate(screens[screenIndex], null, SlideFromRightTransition);
            NextButton.Visibility = BackBtn.Visibility = ContentFrame.Visibility = Visibility.Visible;
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            screenIndex++;                                                                // Increment screenIndex
            ContentFrame.Navigate(screens[screenIndex], null, SlideFromRightTransition);  // Navigate to next screen  
            if (screenIndex == 3) NextButton.Visibility = BackBtn.Visibility = Visibility.Collapsed;
            else if (screenIndex == 2) NextButton.IsEnabled = false;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            screenIndex--;
            if (screenIndex != -1)
                ContentFrame.Navigate(screens[screenIndex], null, SlideFromLeftTransition);
            else
                NextButton.Visibility = BackBtn.Visibility = ContentFrame.Visibility = Visibility.Collapsed;
            
            if (screenIndex == 1) NextButton.IsEnabled = true;
        }
    }
}
