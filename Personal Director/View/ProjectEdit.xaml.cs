using Personal_Director.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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
        private Project Project { get; set; }
        private bool IsPutAwayMediaCabinet { get; set; } = true;
        private ObservableCollection<Media> MediaCabinetDataList { get; set; }
        private ObservableCollection<Media> MediaScriptDataList { get; set; }
        private string MediaSelectGuid { get; set; }

        public ProjectEdit()
        {
            this.InitializeComponent();
        }

        #region event
        private void PrePage_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
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
                Project = (Project)e.Parameter;
                SetViewData(Project);
            }
            else
            {
                throw new Exception("ne project !!");
            }
            base.OnNavigatedTo(e);
        }

        private void SetViewData(Project viewModel)
        {
            MediaScriptDataList = new ObservableCollection<Media>(viewModel.MediaScriptList);
            MediaCabinetDataList = new ObservableCollection<Media>(viewModel.MediaCabinetList);
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
                this.MediaCabinetDataList.Add(new Media()
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
                                                                     : new GridLength(50);
            MediaCabinetGrid();
            var panel = (ItemsWrapGrid)MediaCabinetList.ItemsPanelRoot;
            panel.ItemWidth = this.IsPutAwayMediaCabinet ? 210 : 0;
        }

        private void MediaCabinetGrid()
        {
            if (this.IsPutAwayMediaCabinet)
            {
                this.MediaCabinetLeftArea.Width = new GridLength(40);
                this.MediaCabinetGridArea.Width = new GridLength(1, GridUnitType.Star);
                return;
            }
            this.MediaCabinetLeftArea.Width = new GridLength(10);
            this.MediaCabinetGridArea.Width = new GridLength(0);
        }

        private void MediaScriptList_DragOver(object sender, DragEventArgs e)
        {
            if(e.Data != null && e.Data.GetView().Contains("MediaDataGuid"))
            {
                e.AcceptedOperation = DataPackageOperation.Link;
                return;
            }
            e.AcceptedOperation = DataPackageOperation.None;
        }

        private async void MediaScriptList_Drop(object sender, DragEventArgs e)
        {
            var guid = await e.Data.GetView().GetTextAsync("MediaDataGuid");

            Media media = MediaCabinetDataList.FirstOrDefault(i => i.Guid.ToString() == guid);

            if (!this.MediaScriptDataList.Any())
            {
                this.MediaScriptDataList.Insert(0, new Media(media));
                return;
            }

            var gridView = ((GridView)sender);

            //Find the position where item will be dropped in the gridview
            Point pos = e.GetPosition(gridView.ItemsPanelRoot);

            //Get the size of one of the list items
            GridViewItem gvi = (GridViewItem)gridView.ContainerFromIndex(0);
            double itemHeight = gvi.ActualWidth + gvi.Margin.Left + gvi.Margin.Right;

            //Determine the index of the item from the item position (assumed all items are the same size)
            int index = Math.Min(gridView.Items.Count - 1, (int)(pos.X / itemHeight));

            this.MediaScriptDataList.Insert(index, new Media(media));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.MediaScriptDataList.Remove(this.MediaScriptDataList.FirstOrDefault(i => i.Guid.ToString() == this.MediaSelectGuid));
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var flyout = this.ImageCommandsFlyout;
            this.MediaSelectGuid = ((Grid)sender).Tag.ToString();
            var options = new FlyoutShowOptions()
            {
                Position = e.GetPosition((FrameworkElement)sender),
                ShowMode = FlyoutShowMode.Transient
            };
            flyout?.ShowAt((FrameworkElement)sender, options);
        }

        private void MediaCabinetList_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            e.Data.SetData("MediaDataGuid", (e.Items[0] as Media).Guid.ToString());
        }

        #endregion
    }
}
