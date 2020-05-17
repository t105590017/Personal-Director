using Personal_Director.Models;
using Personal_Director.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Personal_Director
{
    public sealed partial class HomePage : Page
    {
        private Project project { get; set; }
        private List<Project> ProjectDataList { get; set; }
        //public bool SelectionCheckBox { get; set; }
        public HomePage()
        {
            this.InitializeComponent();

            List<Media> mediaScriptList = new List<Media>(),
                        mediaCabinetList = new List<Media>();
            this.ProjectDataList = new List<Project> {
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
            this.project = new Project()
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
            this.project.Name = this.project.Name + "1";

            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            if (!this.ProjectDataList.Any())
            {
                this.ProjectDataList.Insert(0, new Project(project));
                this.Frame.Navigate(typeof(ProjectEdit), project);
                return;
            }
            this.Frame.Navigate(typeof(ProjectEdit), project);
            //int index = this.ProjectDataList.Count() + 1;
            //this.ProjectDataList.Insert(index, new Project(project));
            //this.Frame.Navigate(typeof(ProjectEdit), project);


            //var x = new GridView();

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
    }
}
