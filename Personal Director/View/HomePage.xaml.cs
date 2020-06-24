using Personal_Director.Models;
using Personal_Director.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class HomePage : Page
    {
        private Project _project { get; set; }
        private List<Project> _projectDataList { get; set; }

        private Model _model { get; set; }

        private HomePageViewModel _viewModel { get; set; }
        //public bool SelectionCheckBox { get; set; }
        public HomePage()
        {
            this.InitializeComponent();
            this._model = new Model();
            this._viewModel = new HomePageViewModel(this._model);
            List<Media> mediaScriptList = new List<Media>(),
                        mediaCabinetList = new List<Media>();
            this._projectDataList = new List<Project> {
                new Project{Name = "未命名專案1",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
                new Project{Name = "未命名專案2",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
                new Project{Name = "未命名專案3",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
                new Project{Name = "未命名專案4",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
                new Project{Name = "未命名專案5",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
                new Project{Name = "未命名專案6",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
                new Project{Name = "未命名專案7",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
                new Project{Name = "未命名專案8",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
                new Project{Name = "未命名專案9",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
                new Project{Name = "未命名專案10",
                            MediaCabinetList = mediaCabinetList,
                            MediaScriptList = mediaScriptList
                           },
            };
            this._project = new Project()
            {
                Name = "123",
                MediaCabinetList = mediaCabinetList,
                MediaScriptList = mediaScriptList
            };
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            if (!this._projectDataList.Any())
            {
                this._projectDataList.Insert(0, new Project(_project));
                this.Frame.Navigate(typeof(ProjectEdit), this._model, new DrillInNavigationTransitionInfo());
                return;
            }
            this.Frame.Navigate(typeof(ProjectEdit), this._model, new DrillInNavigationTransitionInfo());
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void AppBarToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock1_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        /*private void button_PointerEntered_2(object sender, PointerRoutedEventArgs e)
        {
            checkBox2.Visibility = Visibility.Visible;
            remove_btn.Visibility = Visibility.Visible;
            reserve_btn.Visibility = Visibility.Visible;
        }

        private void button_PointerExited_2(object sender, PointerRoutedEventArgs e)
        {
            checkBox2.Visibility = Visibility.Collapsed;
            remove_btn.Visibility = Visibility.Collapsed;
            reserve_btn.Visibility = Visibility.Collapsed;
        }*/


        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            selectionBar.Visibility = Visibility.Visible;
            selectionCancel_btn.Visibility = Visibility.Visible;
            selection_btn.Visibility = Visibility.Visible;
            selection_btn2.Visibility = Visibility.Visible;
        }

        private void SelectionCancel_btn_Click(object sender, RoutedEventArgs e)
        {
            selectionBar.Visibility = Visibility.Collapsed;
            selectionCancel_btn.Visibility = Visibility.Collapsed;
            selection_btn.Visibility = Visibility.Collapsed;
            selection_btn2.Visibility = Visibility.Collapsed;
            //SelectionCheckBox = false;

        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            //Project project = 
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            selectionBar.Visibility = Visibility.Visible;
            selectionCancel_btn.Visibility = Visibility.Visible;
            selection_btn.Visibility = Visibility.Visible;
            selection_btn2.Visibility = Visibility.Visible;
        }

        private async void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".proj");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string projectJsonString = await Windows.Storage.FileIO.ReadTextAsync(file);
                bool isProjectSetupSucess = this._viewModel.OpenProject(projectJsonString);
                if (!isProjectSetupSucess)
                {
                    // 彈出錯誤視窗
                    ContentDialog noWifiDialog = new ContentDialog
                    {
                        Title = "錯誤",
                        Content = "專案檔損毀！",
                        CloseButtonText = "確認"
                    };
                    await noWifiDialog.ShowAsync();
                    return;
                }

                //匯入媒體櫃
                List<string> mediaCabinetPath = this._viewModel.GetCabinetPathFromProject();
                List<string> mediaCabinetGuid = this._viewModel.GetCabinetGuidFromProject();
                for (int i = 0; i < mediaCabinetPath.Count; i++)
                {
                    //file = await StorageFile.GetFileFromPathAsync(mediaCabinetPath[i]);
                    file = await loadFileFromAbsolutePath(mediaCabinetPath[i]);
                    if (file == null)
                    {
                        //將該媒體標為損毀
                        this._viewModel.AddMediaIntoCabinet(new Media(Guid.Parse(mediaCabinetGuid[i]))
                        {
                            Thumbnail = new BitmapImage(),
                            Describe = "檔案遺失!",
                            SourcePath = null
                        });
                    }
                    else
                    {
                        var stream = await file.OpenAsync(FileAccessMode.Read);
                        const uint requestedSize = 190;
                        const ThumbnailMode thumbnailMode = ThumbnailMode.VideosView;
                        const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
                        var image = new BitmapImage();
                        image.SetSource(await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions));
                        this._viewModel.AddMediaIntoCabinet(new Media(Guid.Parse(mediaCabinetGuid[i]))
                        {
                            Thumbnail = image,
                            Describe = file.Name,
                            SourcePath = file.Path
                        });
                    }
                }

                //從專案檔讀取分鏡腳本資料並轉為Instance
                ObservableCollection<StoryBoard> script = this._model.Project.GetScriptFromProject(this._model.getAllMediaCabinetData());
                this._model.SetScriptData(script);
                foreach (var storyBoard in this._model.getAllStoryBoardScriptData())
                {
                    string outputPath = storyBoard.MediaSource.SourcePath;
                    //套用特效
                    foreach (var effect in storyBoard.GetAllEffects())
                    {
                        effect.SetDataSource(storyBoard.Guid, storyBoard.MediaSource.SourcePath);
                        effect.Excute();
                        storyBoard.MediaSource = new Media(storyBoard.MediaSource.Guid)
                        {
                            SourcePath = effect.OutputPath,
                            Thumbnail = storyBoard.MediaSource.Thumbnail
                        };
                        outputPath = storyBoard.MediaSource.SourcePath;
                    }

                    //讀檔並將縮圖套用至分鏡腳本
                    file = await StorageFile.GetFileFromPathAsync(outputPath);
                    if (file != null)
                    {
                        var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                        const uint requestedSize = 190;
                        const ThumbnailMode thumbnailMode = ThumbnailMode.VideosView;
                        const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
                        var image = new BitmapImage();
                        image.SetSource(await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions));
                        storyBoard.MediaSource.Thumbnail = image;
                    }
                }
                this.Frame.Navigate(typeof(ProjectEdit), _model);
            }
            else
            {
                this.textBlock.Text = "Operation cancelled.";
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
    }
}
