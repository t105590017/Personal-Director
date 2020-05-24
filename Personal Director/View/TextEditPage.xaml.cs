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
        //TimeLineConverter _converter;
        // MediaPlaybackSession _mediaPlaybackSession = new MediaPlaybackSession();
        public TextEditPage()
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
            Console.WriteLine("this is text page!!!!!!!!!");

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
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Top;
            subtitle.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Top;
            subtitle.HorizontalAlignment = HorizontalAlignment.Right;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Center;
            subtitle.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Center;
            subtitle.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Center;
            subtitle.HorizontalAlignment = HorizontalAlignment.Right;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Bottom;
            subtitle.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Bottom;
            subtitle.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            subtitle.VerticalAlignment = VerticalAlignment.Bottom;
            subtitle.HorizontalAlignment = HorizontalAlignment.Right;
        }

        private void Saving_Click(object sender, RoutedEventArgs e)
        {
            output.Text = currentTime.Text.ToString() + '/' + subtitle.Text + '/' + subtitle.FontSize.ToString() + '/'
                + color.Text.ToString() + '/' + FontsCombo.SelectedValue.ToString() + "/VerticalAlignment:" 
                + subtitle.VerticalAlignment.ToString() + "/HorizontalAlignment:" + subtitle.HorizontalAlignment.ToString();
        }
    }
}
