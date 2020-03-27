using Personal_Director.Model;
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

        public HomePage()
        {
            this.InitializeComponent();
            this.project = new Project()
            {
                Name = "123"
            };
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
            this.Frame.Navigate(typeof(ProjectEdit), project);
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

        private void button_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {
            checkBox1.Visibility = Visibility.Visible;
           
        }

        private void button_PointerExited_1(object sender, PointerRoutedEventArgs e)
        {
            checkBox1.Visibility = Visibility.Collapsed;

        }

        private void button_PointerEntered_2(object sender, PointerRoutedEventArgs e)
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
        }


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
            //checkBox1.Visibility = Visibility.Collapsed;
        }

        /*private void CheckBox1_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            checkBox1.Visibility = Visibility.Visible;
        }*/

       
    }
}
