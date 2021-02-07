using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace App2.ControlPanelPages
{
    public sealed partial class Import : Page
    {
        CreateQueue x = new CreateQueue();
        public Import()
        {
            this.InitializeComponent();
            
            x.createQueue();
        }

        /********************************************************************/
        private async void ChooseFiles_Click(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
        }

        async private System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mkv");

            var selectedFiles = await openPicker.PickMultipleFilesAsync();
           
            if (selectedFiles.Count > 0)
            {
                SelectedFilesList.Items.Clear();

                progressBar.Visibility = Visibility.Visible;
                progressStatus.Visibility = Visibility.Visible;
                importProgressTitle.Visibility = Visibility.Visible;

                // Append fileNames to view list.
                foreach (StorageFile file in selectedFiles)
                    SelectedFilesList.Items.Add(file.Name);

                //Copy the file(s).
                int count = 1;
                foreach (StorageFile file in selectedFiles)
                {              
                    progressStatus.Text = "(" + count.ToString() + " of " + selectedFiles.Count + ")";
                    count++;

                    StorageFolder videosPath = await StorageFolder.GetFolderFromPathAsync(x.appDataPath + "\\videos\\");
                    await file.CopyAsync(videosPath); // copy the file to appdata\videos
                    
                }

                progressBar.Visibility = Visibility.Collapsed;
                importProgressTitle.Text = "Done!";

                x.createQueue(); 
            }
        }
        /********************************************************************/
    }
}
