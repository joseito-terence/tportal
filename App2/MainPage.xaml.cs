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
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Popups;
using System.Timers;

namespace App2
{
    public sealed partial class MainPage : Page
    {
        private readonly CreateQueue x = new CreateQueue();
        private readonly Encryption encryption = new Encryption();

        static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        readonly string controlPanelPassword = (string)localSettings.Values["controlPanelPassword"];

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer EndTimer = new DispatcherTimer();

        private DateTime TimeFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0),
                         TimeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        private TimeSpan timeLeft;

        public MainPage()
        {
            this.InitializeComponent();

            if (localSettings.Values["controlPanelPassword"] != null)
            {
                if ((bool)localSettings.Values["TimeRestriction_IsEnabled"])
                {
                    TimeFrom += (TimeSpan)localSettings.Values["TimeRestriction_TimeFrom"];
                    TimeTo += (TimeSpan)localSettings.Values["TimeRestriction_TimeTo"];


                    if (DateTime.Now < TimeFrom)
                    {
                        timer.Tick += timer_Tick;
                        timer.Interval = new TimeSpan(0, 0, 1);
                        timer_label.Visibility = Visibility.Visible;

                        timeLeft = TimeFrom.Subtract(DateTime.Now);

                        timer.Start();
                    }
                    else if (DateTime.Now < TimeTo)
                    {
                        assignVideoToPlayer();
                    }

                    return;
                }

                assignVideoToPlayer(); //_onLoad
            } 
        }

        private void timer_Tick(object sender, object e)
        {
            if (timeLeft.ToString(@"hh\:mm\:ss") == "00:00:00")
            {
                timer.Stop();
                timer_label.Visibility = Visibility.Collapsed;
                assignVideoToPlayer();
                return;
            }
            timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
            timer_label.Text = timeLeft.ToString(@"hh\:mm\:ss");
        }
        /*********************************************************************************/

        private void assignVideoToPlayer() // Function to assign video to player.
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            Object videos_per_day = localSettings.Values["videos_per_day"];
            int value = Convert.ToInt32(videos_per_day);

            
            Uri uri = new Uri("ms-appdata:///local/videos/" + videoPicker(value));
            mediaPlayer.Source = MediaSource.CreateFromUri(uri);
            //mediaPlayer.AutoPlay = true;

            if ((bool)localSettings.Values["TimeRestriction_IsEnabled"])
            {
                EndTimer.Interval = new TimeSpan(0, 0, 1);
                EndTimer.Tick += EndTimer_Tick;
                EndTimer.Start();
            }
        }
        private void EndTimer_Tick(object sender, object e)
        {
            if (DateTime.Now.ToString("HH:mm:ss") == TimeTo.ToString(@"HH\:mm\:ss"))
            {
                timer_label.Visibility = Visibility.Visible;
                timer_label.Text = "Time's Up!";
                mediaPlayer.Source = null;
                EndTimer.Stop();
            }
        }

        /*********************************************/
        private string videoPicker(int no_of_videos_per_day = 1)  // ---------Video Picker------------
        {
            string fileName = "", queuePath = x.appDataPath + "\\src\\queue.txt";
            if (checkDateOccurance() < no_of_videos_per_day)
            {
                if (new FileInfo(queuePath).Length != 0)
                    fileName = File.ReadLines(queuePath).First();
            }
            else
            {
                timer_label.Visibility = Visibility.Visible;
                timer_label.Text = "Done for the Day!";
            }
                
            return fileName;
        }
        /*********************************************/

        private async void NextBtn_NextClicked(object sender, EventArgs e) //Next >> Button.
        {
            MessageDialog msgbox = new MessageDialog("Are you sure ?", "Play Next");

            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "No", Id = 1 });
            var res = await msgbox.ShowAsync();
            
            if ((int)res.Id == 0) doneWatching();
        }

        /*********************************************/
        private void doneWatching()
        {
            File.Delete(x.appDataPath + "\\videos\\" + videoPicker()); // Delete the file.
            x.createQueue();              // refresh/re-create queue.

            string curr_Date = DateTime.Today.ToString("d"); // current date.
            using (StreamWriter sw = File.AppendText(x.appDataPath + "/src/cache.txt")) // make an entry of current date.
            {
                sw.WriteLine(curr_Date);
            }
            assignVideoToPlayer();          // assign next video to player.
        }

        /*********************************************/
       
        private int checkDateOccurance() // Check how many times the date has occured in the file.
        {
            string path = x.appDataPath + "/src/cache.txt";
            int count = 0;

            if (!File.Exists(path)) File.Create(path);
            
            else{
                string curr_Date = DateTime.Today.ToString("d");

                // Open the file to read from.
                using (StreamReader sr = File.OpenText(path)){
                    string line;
                    while((line = sr.ReadLine()) != null)  if(line == curr_Date) count++;
                }
            }
            return count;
        }

        /*********************************************/
        private async void OpenControlPanel_ControlPanelOpened(object sender, EventArgs e)
        {
            if (controlPanelPassword != null)
            {
                await AuthenticationDialog.ShowAsync();
                return;
            }
            await CreatePasswordDialog.ShowAsync();
        }
        /********************************************/

        //-----------------------------------//
        //		CREATE PASSWORD DIALOG  	 //
        //-----------------------------------//
        /* Everything here executes only for the first load of the application. */
        private void ConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (NewPassword.Password == ConfirmPassword.Password)
            {
                CreatePasswordDialog.IsPrimaryButtonEnabled = true;
                return;
            }
            CreatePasswordDialog.IsPrimaryButtonEnabled = false;
        }

        private void SetPassword_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Init localSettings variables
            localSettings.Values["controlPanelPassword"] = encryption.EnryptString(NewPassword.Password);
            localSettings.Values["videos_per_day"] = 1;
            localSettings.Values["TimeRestriction_IsEnabled"] = false;

            // Create Required directories.
            Directory.CreateDirectory(x.appDataPath + "\\src");
            Directory.CreateDirectory(x.appDataPath + "\\videos");

            // Create Required Files.
            using (File.Create(x.appDataPath + "\\src\\cache.txt")) { }
            using (File.Create(x.appDataPath + "\\src\\queue.txt")) { }

            // Navigate to Control Panel.
            this.Frame.Navigate(typeof(ControlPanel));
        }

        private void CreatePasswordDialog_Cancel(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            CreatePasswordDialog.IsPrimaryButtonEnabled = false;
            NewPassword.Password = ConfirmPassword.Password = "";
        }

        //****************END****************//


        //-----------------------------------//
        //		 AUTHENTICATION DIALOG       //
        //-----------------------------------//
        private void CheckPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Password.Password == encryption.DecryptString(controlPanelPassword))
            {
                AuthenticationDialog.IsPrimaryButtonEnabled = true;
                return;
            }
            AuthenticationDialog.IsPrimaryButtonEnabled = false;
        }

        private void Authenticate_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Frame.Navigate(typeof(ControlPanel));
        }

        private void AuthenticationDialog_Cancel(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            AuthenticationDialog.IsPrimaryButtonEnabled = false;
            Password.Password = "";
        }

        //****************END****************//

    }
}
