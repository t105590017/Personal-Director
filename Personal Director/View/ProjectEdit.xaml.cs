using Personal_Director.Models;
using Personal_Director.View;
using Personal_Director.ViewModels;
using Production.MediaProcess;
using Production.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Personal_Director
{
    public sealed partial class ProjectEdit : Page
    {
        private bool _isPutAwayMediaCabinet  = true;

        private string _mediaSelectGuid;

        private StoryBoard _selectedStoryBoard;

        ProjectEditViewModel ViewModel { get; set; }

        public ProjectEdit()
        {
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.InitializeComponent();
        }

        /// <summary>
        /// 複寫OnNavigateTo
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Model && this.ViewModel == null)
            {
                this.ViewModel = new ProjectEditViewModel((Model)e.Parameter);
            }
            else if (e.Parameter is StoryBoard)
            {
                this.ViewModel.UpdateStoryBoard(e.Parameter as StoryBoard);
            }
            base.OnNavigatedTo(e);
        }

        #region events

 #region Common Tools function demonstration
    #if DEBUG
        List<string> guids = new List<string>();
        /// <summary>
        /// 功能列刪除(debug下是測試)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StoryBoardDelete_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                Guid guid = Guid.NewGuid();
                guids.Add(guid.ToString());

                #region 直接操作Handler
                //VideoHandler.SetSource(guid, file.Path)
                //            .CutVideo(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20))
                //            .ChangeVideoSpeed(4)
                //            .ChangeAudioSpeed(2).ChangeAudioSpeed(2)
                //            .AddTextToVideo(guid.ToString(), Production.Enum.VideoPosition.Center, System.Drawing.Color.Blue, fontsize: 72);
                #endregion
                #region 操作介面
                List<IEffect> effects = new List<IEffect>()
                {
                    new CutEffect(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)),
                    new SpeedEffect(4),
                    new TextEffect(guid.ToString(), Production.Enum.VideoPosition.Center, System.Drawing.Color.Blue, Fontsize: 72)
                };
                effects.ForEach(i =>
                {
                    i.SetDataSource(guid, file.Path);
                    i.Excute();
                });
                #endregion

                if (guids.Count() > 1)
                {
                    var outPath = VideoHandler.Export(guids.ToArray()).OutputPath;
                }
            }
        }
    #endif
#endregion

        /// <summary>
        /// 按下上一頁按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrePage_Click(object sender, RoutedEventArgs e)
        {
            //回到首頁必須清空本頁面快取
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            this.Frame.Navigate(typeof(HomePage), new DrillInNavigationTransitionInfo());
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
            this._isPutAwayMediaCabinet = !this._isPutAwayMediaCabinet;
            this.PutAwayMediaCabinetIcon.Glyph = this._isPutAwayMediaCabinet ? "\uE76B"
                                                                            : "\uE76C";
            this.MediaCabinetArea.Width = this._isPutAwayMediaCabinet ? new GridLength(5, GridUnitType.Star)
                                                                     : new GridLength(50);
            this.MediaCabinetGrid();
            var panel = (ItemsWrapGrid)MediaCabinetList.ItemsPanelRoot;
            panel.ItemWidth = this._isPutAwayMediaCabinet ? 210 : 0;
        }

        private void MediaCabinetGrid()
        {
            if (this._isPutAwayMediaCabinet)
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
            this.ViewModel.RemoveMediaFromScript(this._mediaSelectGuid);
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
                Title = "刪除媒體?",
                PrimaryButtonText = "確認",
                CloseButtonText = "取消"
            };

            ContentDialogResult result = await noWifiDialog.ShowAsync();
            // 彈出視窗 沒用

            switch (result)
            {
                case ContentDialogResult.Primary:
                    this.ViewModel.RemoveMediaFormCabinet(Guid.Parse(this._mediaSelectGuid));
                    break;
            }

            //this.ViewModel.RemoveMediaFromScript(this.MediaSelectGuid);
        }

        private void RightTappedStoryBoard(object sender, RightTappedRoutedEventArgs e)
        {
            var flyout = this.StoryBoardCommandsFlyout;
            this._mediaSelectGuid = ((Grid)sender).Tag.ToString();
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
            this._mediaSelectGuid = ((Grid)sender).Tag.ToString();
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
            }
        }
        
        /// <summary>
        /// 按下媒體時更新右方預覽畫面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MediaCabinetList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Media item = e.ClickedItem as Media;
            StorageFile file = await this.loadFileFromAbsolutePath(item.SourcePath);
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                this.MediaPreView.Source = MediaSource.CreateFromStream(stream, file.ContentType);
            }
        }

        private async void StoryBoardScriptList_ItemClick(object sender, ItemClickEventArgs e)
        {
            this._selectedStoryBoard = e.ClickedItem as StoryBoard;
            StorageFile file = await this.loadFileFromAbsolutePath(_selectedStoryBoard.MediaSource.SourcePath);
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                this.MediaPreView.Source = MediaSource.CreateFromStream(stream, file.ContentType);
            }
        }

        private async Task<StorageFile> loadFileFromAbsolutePath(string path)
        {
            StorageFile file;
            try
            {
                file = await StorageFile.GetFileFromPathAsync(path);
            }
            catch
            {
                return null;
            }
            return file;
        }

        private void ClipButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectedStoryBoard != null)
            {
                this.Frame.Navigate(typeof(ClipEditPage), _selectedStoryBoard);
                this._selectedStoryBoard = null;
            }
        }

        private void AddTextButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectedStoryBoard != null)
            {
                this.Frame.Navigate(typeof(TextEditPage), _selectedStoryBoard);
            }
        }

        //匯出影片
        private async void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Video", new List<string>() { ".mp4" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "My Video";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                //TODO: 空腳本輸出防呆
                List<string> StoryBoardGuids = this.ViewModel.getAllStoryBoardGuids();
                StorageFile video = await StorageFile.GetFileFromPathAsync(VideoHandler.Export(StoryBoardGuids.ToArray()).OutputPath);
                StorageFile copiedVideo = await video.CopyAsync(await file.GetParentAsync(), file.Name, NameCollisionOption.ReplaceExisting);

                // 彈出視窗 沒用
                ContentDialog noWifiDialog = new ContentDialog
                {
                    Title = "Test",
                    Content = "匯出成功",
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await noWifiDialog.ShowAsync();

            }
        }

        /// <summary>
        /// 按下預覽按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            string outputPath = VideoHandler.Export(this.ViewModel.getAllStoryBoardGuids().ToArray()).OutputPath;
            StorageFile file = await StorageFile.GetFileFromPathAsync(outputPath);
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                this.MediaPreView.Source = MediaSource.CreateFromStream(stream, file.ContentType);
            }
        }

        #endregion


    }
}
