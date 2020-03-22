using Personal_Director.Model;
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

namespace Personal_Director
{
    public sealed partial class ProjectEdit : Page
    {
        private Project project { get; set; }

        public ProjectEdit()
        {
            this.InitializeComponent();
        }

        private void PrePage_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            this.project.Name = this.project.Name + "1";

            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Project)
            {
                project = (Project)e.Parameter;
            }
            else
            {
                throw new Exception("ne project !!");
            }
            base.OnNavigatedTo(e);
        }
    }
}
