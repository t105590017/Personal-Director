using System;
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
using Microsoft.Graphics.Canvas.Text;
using Personal_Director.Models;
using Personal_Director.ViewModels;
using Production.Enum;
using Windows.Storage.FileProperties;

// 空白頁項目範本已記錄在 https://go.microsoft.com/fwlink/?LinkId=234238

namespace Personal_Director.View
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class TextEditPage : Page
    {
        MediaPlayer _mediaPlayer = new MediaPlayer();
        MediaTimelineController _mediaTimelineController = new MediaTimelineController();
        TimeSpan _duration;

        private TextEditPageViewModel ViewModel;

        private VideoPosition _textPosition;

        public TextEditPage()
        {
            this.InitializeComponent();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is StoryBoard)
            {
                this.ViewModel = new TextEditPageViewModel((StoryBoard)e.Parameter);
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
                timeLine.Value = ((TimeSpan)_mediaTimelineController.Position).TotalSeconds;
            }
        }

        private void Start_pause_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (_mediaTimelineController.State == MediaTimelineControllerState.Running)
                {
                    EllStoryboard.Pause();
                    _mediaTimelineController.Pause();
                    start_pause.Icon = new SymbolIcon(Symbol.Play);
                }
                else
                {
                    //EllStoryboard.Resume();
                    EllStoryboard.Begin();
                    _mediaTimelineController.Resume();
                    start_pause.Icon = new SymbolIcon(Symbol.Pause);
                }
            }
            catch
            {

            }

        }

        void Timer_Tick(object sender, object e)
        {
            timeLine.Value = ((TimeSpan)_mediaTimelineController.Position).TotalSeconds;
            //textBlock.Text = GenTimeSpanFromSeconds(Math.Round(timeLine.Value));

            currentTime.Text = GenTimeSpanFromSeconds(Math.Round(timeLine.Value));

            if (timeLine.Value <= RangeSelectorControl.RangeMin || timeLine.Value >= RangeSelectorControl.RangeMax)
            {
                subtitle.Visibility = Visibility.Collapsed;
            }
            else subtitle.Visibility = Visibility.Visible;

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

                currentTime.Text = "00:00:00";
                totalTime.Text = GenTimeSpanFromSeconds(Math.Round(_duration.TotalSeconds));
                
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

        public List<string> TextFonts
        {
            get
            {
                return CanvasTextFormat.GetSystemFontFamilies().OrderBy(f => f).ToList();
            }
        }

        private void FontsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            subtitle.FontFamily = new FontFamily(FontsCombo.SelectedValue.ToString());
        }


        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            //textBlock.Text =  e.ClickedItem.ToString();
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Top;
            subtitle.HorizontalAlignment = HorizontalAlignment.Left;
            this._textPosition = VideoPosition.TopLeft;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Top;
            subtitle.HorizontalAlignment = HorizontalAlignment.Center;
            this._textPosition = VideoPosition.TopCenter;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Top;
            subtitle.HorizontalAlignment = HorizontalAlignment.Right;
            this._textPosition = VideoPosition.TopRight;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Center;
            subtitle.HorizontalAlignment = HorizontalAlignment.Left;
            this._textPosition = VideoPosition.CenterLeft;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Center;
            subtitle.HorizontalAlignment = HorizontalAlignment.Center;
            this._textPosition = VideoPosition.Center;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Center;
            subtitle.HorizontalAlignment = HorizontalAlignment.Right;
            this._textPosition = VideoPosition.CenterRight;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Bottom;
            subtitle.HorizontalAlignment = HorizontalAlignment.Left;
            this._textPosition = VideoPosition.ButtomLeft;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Bottom;
            subtitle.HorizontalAlignment = HorizontalAlignment.Center;
            this._textPosition = VideoPosition.ButtomCenter;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Bottom;
            subtitle.HorizontalAlignment = HorizontalAlignment.Right;
            this._textPosition = VideoPosition.ButtomRight;

        }

        private async void Saving_Click(object sender, RoutedEventArgs e)
        {
            output.Text = currentTime.Text.ToString() + '/' + subtitle.Text + '/' + subtitle.FontSize.ToString() + '/'
                + this.color.Text.ToString() + '/' + FontsCombo.SelectedValue.ToString() + "/VerticalAlignment:" 
                + subtitle.VerticalAlignment.ToString() + "/HorizontalAlignment:" + subtitle.HorizontalAlignment.ToString();

            Color colorItem= this.colorPicker.Color;
            var systemColor = System.Drawing.Color.FromArgb(colorItem.A, colorItem.R, colorItem.G, colorItem.B);
            string outPutPath = this.ViewModel.GetProcessedMediaPath(this.subtitle.Text, this._textPosition, systemColor, this.FontsCombo.SelectedValue.ToString(), Convert.ToInt32(this.subtitle.FontSize));

            StorageFile file = await StorageFile.GetFileFromPathAsync(outPutPath);
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                const uint requestedSize = 190;
                const ThumbnailMode thumbnailMode = ThumbnailMode.VideosView;
                const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
                var image = new BitmapImage();
                image.SetSource(await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions));
                this.ViewModel.StoryBoard.MediaSource = new Media
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
