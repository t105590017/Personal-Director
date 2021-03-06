﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Personal_Director.Converter;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Personal_Director.Models;
using Personal_Director.ViewModels;
using Windows.Storage.FileProperties;

// 空白頁項目範本已記錄在 https://go.microsoft.com/fwlink/?LinkId=234238

namespace Personal_Director.View
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class ClipEditPage : Page
    {
        MediaPlayer _mediaPlayer = new MediaPlayer();

        MediaTimelineController _mediaTimelineController = new MediaTimelineController();

        TimeSpan _duration;

        private ClipEditPageViewModel ViewModel;

        public ClipEditPage()
        {
            this.InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is StoryBoard)
            {
                this.ViewModel = new ClipEditPageViewModel(e.Parameter as StoryBoard);
                this.LoadMedia();
            }
            else
            {
                throw new Exception("Model passing error!");
            }
            base.OnNavigatedTo(e);
        }

        private async void LoadMedia()
        {
            Media media = this.ViewModel.StoryBoard.MediaSource as Media;

            Windows.Storage.StorageFile file = await Windows.Storage.StorageFile.GetFileFromPathAsync(media.SourcePath);
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                MediaSource mediaSource = MediaSource.CreateFromStream(stream, file.ContentType);
                mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
                this._mediaPlayer.Source = mediaSource;
                this._mediaPlayer.CommandManager.IsEnabled = false;
                this._mediaPlayer.TimelineController = _mediaTimelineController;
                this._mediaPlayerElement.SetMediaPlayer(this._mediaPlayer);

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
                timeLine.Value = _mediaTimelineController.Position.TotalSeconds;

                lowerTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMin));
                upperTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMax));
            }
        }

        private void Start_pause_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaTimelineController.State == MediaTimelineControllerState.Running)
            {
                EllStoryboard.Pause();
                this._mediaTimelineController.Pause();
                start_pause.Icon = new SymbolIcon(Symbol.Play);
            }
            else
            {
                if (_mediaTimelineController.Position.TotalSeconds < RangeSelectorControl.RangeMin)
                {
                    this._mediaTimelineController.Position = TimeSpan.FromSeconds(RangeSelectorControl.RangeMin);
                    EllStoryboard.BeginTime = this._mediaTimelineController.Position;
                }
                EllStoryboard.Begin();
                this._mediaTimelineController.Resume();
                start_pause.Icon = new SymbolIcon(Symbol.Pause);
            }

        }

        void Timer_Tick(object sender, object e)
        {
            timeLine.Value = ((TimeSpan)_mediaTimelineController.Position).TotalSeconds;
            //textBlock.Text = GenTimeSpanFromSeconds(Math.Round(timeLine.Value));
            
            lowerTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMin));
            upperTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMax));
            totalTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMax) - Math.Round(RangeSelectorControl.RangeMin));
            if (timeLine.Value == timeLine.Maximum)
            {
                _mediaTimelineController.Position = TimeSpan.FromSeconds(0);
                _mediaTimelineController.Pause();
                EllStoryboard.Stop();
            }
        }
        

        

        private async void MediaSource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            _duration = sender.Duration.GetValueOrDefault();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                timeLine.Minimum = 0;
                timeLine.Maximum = _duration.TotalSeconds;
                timeLine.StepFrequency = 1;
                RangeSelectorControl.Minimum = 0;
                RangeSelectorControl.Maximum = Math.Round(_duration.TotalSeconds);
                RangeSelectorControl.RangeMax = RangeSelectorControl.Maximum;
                RangeSelectorControl.StepFrequency = 1;

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timeLine.Value = ((TimeSpan)_mediaTimelineController.Position).TotalSeconds;

                //textBlock.Text = GenTimeSpanFromSeconds(Math.Round(timeLine.Value));

                lowerTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMin));
                upperTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMax));
                totalTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMax) - Math.Round(RangeSelectorControl.RangeMin));
                
            });
        }

        static String GenTimeSpanFromSeconds(double seconds)
        {
            // Create a TimeSpan object and TimeSpan string from 
            // a number of seconds.
            TimeSpan interval = TimeSpan.FromSeconds(seconds);
            string timeInterval = interval.ToString();

            // Pad the end of the TimeSpan string with spaces if it 
            // does not contain milliseconds.
            int pIndex = timeInterval.IndexOf(':');
            pIndex = timeInterval.IndexOf('.', pIndex);
            if (pIndex < 0) timeInterval += "";

            return timeInterval;
        }

        private void PrePage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProjectEdit), null);
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

        /// <summary>
        /// 按下完成按鈕後處理影片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void finishButton_Click(object sender, RoutedEventArgs e)
        {
            string outPutPath = this.ViewModel.GetProcessedMediaPath(TimeSpan.FromSeconds(RangeSelectorControl.RangeMin), TimeSpan.FromSeconds(RangeSelectorControl.RangeMax));


            StorageFile file = await StorageFile.GetFileFromPathAsync(outPutPath);
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                const uint requestedSize = 190;
                const ThumbnailMode thumbnailMode = ThumbnailMode.VideosView;
                const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
                var image = new BitmapImage();
                image.SetSource(await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions));
                this.ViewModel.StoryBoard.MediaSource = new Media(this.ViewModel.StoryBoard.MediaSource.Guid)
                {
                    Thumbnail = image,
                    Describe = file.Name,
                    SourcePath = file.Path
                };

                this.Frame.Navigate(typeof(ProjectEdit), this.ViewModel.StoryBoard);
            }
        }
    }
}
