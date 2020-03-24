using Personal_Director.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Personal_Director
{
    public sealed partial class ProjectEdit : Page
    {
        const int STEP_FRAME_NUM = 15;
        private Project project { get; set; }
        private bool IsPutAwayMediaCabinet { get; set; } = true;

        public ProjectEdit()
        {
            this.InitializeComponent();
            ScriptList.Items.Add(new Media()
            {
                Describe = "1"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "2"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "3"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "4"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "1"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "2"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "3"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "4"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "1"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "2"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "3"
            });
            ScriptList.Items.Add(new Media()
            {
                Describe = "4"
            });
            ScrollViewer scrollViewer = new ScrollViewer();
        }

        #region event
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

        private async void AddMedia_ClickAsync(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                this.MediaPreView.Source = Windows.Media.Core.MediaSource.CreateFromStream(stream, file.ContentType);

                const uint requestedSize = 190;
                const ThumbnailMode thumbnailMode = ThumbnailMode.VideosView;
                const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
                var image = new BitmapImage();
                image.SetSource(await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions));
                MediaCabinetList.Items.Add(new Media()
                {
                    Thumbnail = image,
                    Describe = file.Name
                });
            }
            else
            {
                var x = "Operation cancelled.";
            }
        }

        private void PutAwayMediaCabinet_Click(object sender, RoutedEventArgs e)
        {
            this.IsPutAwayMediaCabinet = !this.IsPutAwayMediaCabinet;
            this.PutAwayMediaCabinetIcon.Glyph = this.IsPutAwayMediaCabinet ? "\uE76B"
                                                                            : "\uE76C";
            this.MediaCabinetArea.Width = this.IsPutAwayMediaCabinet ? new GridLength(5, GridUnitType.Star)
                                                                     : new GridLength(1, GridUnitType.Star);


        }

        #endregion

       

    }
}
