using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Popups;

namespace App2
{
    public sealed class CustomMediaTransportControls : MediaTransportControls
    {
        public event EventHandler<EventArgs> NextClicked, ControlPanelOpened;

        public CustomMediaTransportControls()
        {
            this.DefaultStyleKey = typeof(CustomMediaTransportControls);
        }

        protected override void OnApplyTemplate()
        {
            // This is where you would get your custom button and create an event handler for its click method.
            Button nextButton = GetTemplateChild("NextButton") as Button;
            nextButton.Click += NextButton_Click;

            Button ControlPanelButton = GetTemplateChild("ControlPanelButton") as Button;
            ControlPanelButton.Click += ControlPanelButton_Click;

            base.OnApplyTemplate();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NextClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ControlPanelButton_Click(object sender, RoutedEventArgs e)
        {
            ControlPanelOpened?.Invoke(this, EventArgs.Empty);
        }
    }
}
