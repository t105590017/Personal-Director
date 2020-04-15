﻿using Personal_Director.Models;
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
    public sealed partial class HomePage : Page
    {
        private Project project { get; set; }

        public HomePage()
        {
            this.InitializeComponent();

            List<Media> mediaScriptList = new List<Media>(),
                        mediaCabinetList = new List<Media>();

            this.project = new Project()
            {
                Name = "123",
                MediaCabinetList = mediaCabinetList,
                MediaScriptList = mediaScriptList
            };
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

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProjectEdit), project);
        }
    }
}
