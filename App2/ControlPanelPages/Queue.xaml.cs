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
    public sealed partial class Queue : Page
    {
        private readonly CreateQueue x = new CreateQueue();
        
        public Queue()
        {
            this.InitializeComponent();
            displayQueue();
  
        }

        public void displayQueue()
        {
            using (StreamReader sr = File.OpenText(x.appDataPath + "/src/queue.txt"))
            {
                queueList.Items.Clear();
                string line;
                while ((line = sr.ReadLine()) != null) queueList.Items.Add(line);
            }
        }

        private void refreshQueue_Click(object sender, RoutedEventArgs e)
        {
            x.createQueue();
            displayQueue();
        }

        private async void deleteFile_Click(object sender, RoutedEventArgs e)
        {  
            if(queueList.SelectedItem != null)
            {
                MessageDialog msgbox = new MessageDialog("Are you sure you want to delete ?", "Delete File");

                msgbox.Commands.Clear();
                msgbox.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
                msgbox.Commands.Add(new UICommand { Label = "No", Id = 1 });
                var res = await msgbox.ShowAsync();

                if ((int)res.Id == 0)
                {
                    string path = x.appDataPath + "/videos/" + queueList.SelectedItem.ToString();
                    File.Delete(path);

                    x.createQueue();
                    displayQueue();
                }
            }
        }
    }
}
