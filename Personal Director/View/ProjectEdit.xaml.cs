using Personal_Director.Models;
using Personal_Director.View;
using Personal_Director.ViewModels;
using Production.MediaProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
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
        private string MediaSelectGuid { get; set; }

        private StoryBoard _selectedStoryBoard;

        private ProjectEditViewModel ViewModel { get; set; }

        public ProjectEdit()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 複寫OnNavigateTo
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Model)
            {
                this.ViewModel = new ProjectEditViewModel((Model)e.Parameter);
            }
            else
            {
                throw new Exception("Model passing error!");
            }
            base.OnNavigatedTo(e);
        }

        #region event

        /// <summary>
        /// 影片處理測試事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ffmpegTest_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                Guid guid = Guid.NewGuid();
                //guids.Add(guid.ToString());

                VideoHandler.SetSource(guid, file.Path)
                            .CutVideo(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20))
                            .AddTextToVideo(guid.ToString(), Production.Enum.VideoPosition.Center, System.Drawing.Color.Blue, fontsize: 72);
                //if (guids.Count() == 3)
                //{
                //    VideoHandlerObject videoHandlerObject = VideoHandler.Export(guids.ToArray());
                //}
            }
        }

        /// <summary>
        /// 按下上一頁按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrePage_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        /// <summary>
        /// 是否可回上一頁
        /// </summary>
        /// <returns></returns>
        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 新增媒體至媒體櫃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddMedia_ClickAsync(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                this.MediaPreView.Source = MediaSource.CreateFromStream(stream, file.ContentType);

                const uint requestedSize = 190;
                const ThumbnailMode thumbnailMode = ThumbnailMode.VideosView;
                const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
                var image = new BitmapImage();
                image.SetSource(await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions));
                Media media = new Media()
                {
                    Thumbnail = image,
                    Describe = file.Name,
                    SourcePath = file.Path
                };
                this.ViewModel.AddMediaIntoCabinet(media);
                this.ViewModel.AddMediaIntoProjectInfo(file.Path, media);
            }
            else
            {
                //var x = "Operation cancelled.";
            }
        }

        /// <summary>
        /// 點擊收合展開媒體櫃按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PutAwayMediaCabinet_Click(object sender, RoutedEventArgs e)
        {
            this.IsPutAwayMediaCabinet = !this.IsPutAwayMediaCabinet;
            this.PutAwayMediaCabinetIcon.Glyph = this.IsPutAwayMediaCabinet ? "\uE76B"
                                                                            : "\uE76C";
            this.MediaCabinetArea.Width = this.IsPutAwayMediaCabinet ? new GridLength(5, GridUnitType.Star)
                                                                     : new GridLength(50);
            this.MediaCabinetGrid();
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

        /// <summary>
        /// 拖曳經過分鏡腳本時顯示連結提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaScriptList_DragOver(object sender, DragEventArgs e)
        {
            if(e.Data != null && e.Data.GetView().Contains("MediaDataGuid"))
            {
                e.AcceptedOperation = DataPackageOperation.Link;
                return;
            }
            e.AcceptedOperation = DataPackageOperation.None;
        }

        /// <summary>
        /// 將媒體櫃放入分鏡腳本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MediaScriptList_Drop(object sender, DragEventArgs e)
        {
            var guid = await e.Data.GetView().GetTextAsync("MediaDataGuid");

            Media media = this.ViewModel.GridViewMediaCabinetList.FirstOrDefault(i => i.Guid.ToString() == guid);
            StoryBoard storyBoard = new StoryBoard(media);
            if (!this.ViewModel.GridViewStoryBoardScriptDataList.Any())
            {
                this.ViewModel.InsertStoryBoardIntoScript(0, storyBoard.MediaSource);
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

            this.ViewModel.InsertStoryBoardIntoScript(index, storyBoard.MediaSource);
        }

        private void StoryBoardDelete(object sender, RoutedEventArgs e)
        {
            this.ViewModel.RemoveMediaFromScript(this.MediaSelectGuid);
        }

        /// <summary>
        /// 點選媒體櫃，右鍵按下刪除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MediaCabinetDelete(object sender, RoutedEventArgs e)
        {
            // 彈出視窗 沒用
            ContentDialog noWifiDialog = new ContentDialog
            {
                Title = "Test",
                Content = "媒體櫃刪除",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
            // 彈出視窗 沒用

            //this.ViewModel.RemoveMediaFromScript(this.MediaSelectGuid);
        }

        private void RightTappedStoryBoard(object sender, RightTappedRoutedEventArgs e)
        {
            var flyout = this.StoryBoardCommandsFlyout;
            this.MediaSelectGuid = ((Grid)sender).Tag.ToString();
            var options = new FlyoutShowOptions()
            {
                Position = e.GetPosition((FrameworkElement)sender),
                ShowMode = FlyoutShowMode.Transient
            };
            flyout?.ShowAt((FrameworkElement)sender, options);
        }

        private void RightTappedMediaCabinet(object sender, RightTappedRoutedEventArgs e)
        {
            var flyout = this.MediaCabinetCommandsFlyout;
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

        /// <summary>
        /// 按下儲存專案, 選擇導出的路徑並儲存專案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Personal Director 專案檔", new List<string>() { ".proj" });
            savePicker.SuggestedFileName = "Personal-Director-project";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                //禁止更新文件的遠程版本，直到完成更改並call CompleteUpdatesAsync為止。
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                //寫入檔案
                await Windows.Storage.FileIO.WriteTextAsync(file, this.ViewModel.GetProjectInfoToSaving());
                //跟Windows確認檔案狀態
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                //如果成功儲存
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    //this.textBlock.Text = "File " + file.Name + " was saved.";
                }
                else
                {
                    //this.textBlock.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                //取消動作
                //this.textBlock.Text = "Operation cancelled.";
            }
        }
       
        private async void MediaCabinetList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Media item = e.ClickedItem as Media;

            Windows.Storage.StorageFile file = await Windows.Storage.StorageFile.GetFileFromPathAsync(item.SourcePath);
            if (file != null) 
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                this.MediaPreView.Source = Windows.Media.Core.MediaSource.CreateFromStream(stream, file.ContentType);
            }
        }

        private async void StoryBoardScriptList_ItemClick(object sender, ItemClickEventArgs e)
        {
            this._selectedStoryBoard = e.ClickedItem as StoryBoard;

            StorageFile file = await StorageFile.GetFileFromPathAsync(_selectedStoryBoard.MediaSource.SourcePath);
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                this.MediaPreView.Source = MediaSource.CreateFromStream(stream, file.ContentType);
            }
        }

        private void ClipButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectedStoryBoard != null)
            {
                this.Frame.Navigate(typeof(ClipEditPage), _selectedStoryBoard);
            }
        }

        private void AddTextButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectedStoryBoard != null)
            {
                this.Frame.Navigate(typeof(TextEditPage));
            }
        }

        #endregion
    }
}
