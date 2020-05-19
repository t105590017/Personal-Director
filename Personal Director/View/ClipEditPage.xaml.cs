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
        //TimeLineConverter _converter;
        // MediaPlaybackSession _mediaPlaybackSession = new MediaPlaybackSession();
        public ClipEditPage()
        {
            this.InitializeComponent();
            var mediaSource = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/video1.MP4"));
            mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
            _mediaPlayer.Source = mediaSource;
            _mediaPlayer.CommandManager.IsEnabled = false;
            _mediaPlayer.TimelineController = _mediaTimelineController;
            //_mediaPlayer.Play();
            _mediaPlayerElement.SetMediaPlayer(_mediaPlayer);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timeLine.Value = ((TimeSpan)_mediaTimelineController.Position).TotalSeconds;

            //textBlock.Text = GenTimeSpanFromSeconds(Math.Round(timeLine.Value));
            lowerTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMin));
            upperTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMax));
            timer.Tick += timer_Tick;

        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_mediaTimelineController.State == MediaTimelineControllerState.Running)
                {
                    EllStoryboard.Pause();
                    _mediaTimelineController.Pause();
                }
                else
                {
                    //EllStoryboard.Resume();
                    EllStoryboard.Begin();
                    _mediaTimelineController.Resume();
                }
            }
            catch
            {

            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();
                EllStoryboard.Begin();
                _mediaTimelineController.Start();
            }
            catch
            {

            }

        }
        void timer_Tick(object sender, object e)
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

        /*private void Slider_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Slider MySlider = sender as Slider;
            _mediaTimelineController.Position = TimeSpan.FromSeconds(MySlider.Value);
            textBlock.Text = _mediaTimelineController.Position.ToString();
        }*/

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _mediaTimelineController.Position = TimeSpan.FromSeconds(0);
                _mediaTimelineController.Pause();
                EllStoryboard.Stop();
            }
            catch
            {

            }

        }

        /*private void display_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            bool isInFullScreenMode = view.IsFullScreenMode;
            if (isInFullScreenMode)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/3.jpg", UriKind.Absolute));
                MyGrid.Background = imageBrush;
                MyGrid.Background.Opacity = 0.5;
                view.ExitFullScreenMode();
            }
            else
            {
                MyGrid.Background = new SolidColorBrush(Colors.Black);// Windows.UI.Xaml.Media.Brush
                view.TryEnterFullScreenMode();
            }
        }*/

        

        /*private void Volumn_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            _mediaPlayer.Volume = (double)Volumn.Value;
        }*/

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
                RangeSelectorControl.StepFrequency = 1;

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timeLine.Value = ((TimeSpan)_mediaTimelineController.Position).TotalSeconds;
                //textBlock.Text = GenTimeSpanFromSeconds(Math.Round(timeLine.Value));

                lowerTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMin));
                upperTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMax));
                totalTime.Text = GenTimeSpanFromSeconds(Math.Round(RangeSelectorControl.RangeMax) - Math.Round(RangeSelectorControl.RangeMin));

                /*timeLineUpper.Minimum = 0;
                timeLineUpper.Maximum = _duration.TotalSeconds;
                timeLineUpper.StepFrequency = 1;
                timeLineLower.Minimum = 0;
                timeLineLower.Maximum = _duration.TotalSeconds;
                timeLineLower.StepFrequency = 1;*/
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
    }
}
