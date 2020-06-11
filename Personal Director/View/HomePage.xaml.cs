using Personal_Director.Models;
using Personal_Director.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        /*private void SetViewData(HomePage viewModel)
        {
            ProjectDataList = new ObservableCollection<Project>(viewModel.ProjectList);
        }*/

        private void PrePage_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            this._project.Name = this._project.Name + "1";

            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            if (!this._projectDataList.Any())
            {
                this._projectDataList.Insert(0, new Project(_project));
                this.Frame.Navigate(typeof(ProjectEdit), this._model);
                return;
            }
            this.Frame.Navigate(typeof(ProjectEdit), this._model);
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

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                //TODO: 讀檔 execption還沒做
                string text = await Windows.Storage.FileIO.ReadTextAsync(file);
                bool isProjectSetupSucess = this._viewModel.OpenProject(text);
                Console.WriteLine(isProjectSetupSucess);

                //匯入媒體櫃
                List<string> mediaCabinetPath = this._viewModel.GetCabinetPathFromProject();
                List<string> mediaCabinetGuid = this._viewModel.GetCabinetGuidFromProject();
                for (int i = 0; i < mediaCabinetPath.Count; i++)
                {
                    file = await Windows.Storage.StorageFile.GetFileFromPathAsync(mediaCabinetPath[i]);
                    if (file != null)
                    {
                        var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
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

                //匯入分鏡腳本
                List<Guid> mediaSourceGuids = this._viewModel.GetMediaSourceGuidFromProject();
                ObservableCollection<Media> mediaCabinet = this._model.getAllMediaCabinetData();
                for (int i = 0; i < mediaSourceGuids.Count; i++)
                {
                    StoryBoard storyBoard = new StoryBoard(mediaCabinet.FirstOrDefault(x => x.Guid == mediaSourceGuids[i]));
                    this._model.AddStoryBoardIntoScriptData(storyBoard);
                }
                this.Frame.Navigate(typeof(ProjectEdit), _model);
            }
            else
            {
                this.textBlock.Text = "Operation cancelled.";
            }
        }
    }
}
