using Personal_Director.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Director.ViewModels
{
    public class HomePageViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Model _model;

        public HomePageViewModel(Model model) 
        {
            this._model = model;
        }

        //開啟專案
        public bool OpenProject(string jsonString)
        {
            try
            {
                Project project = new Project(jsonString);
                this._model.Project = project;
            }
            catch
            {
                return false;
            }
            return true;
        }

        //從專案檔獲取媒體櫃影片的路徑
        public List<string> GetCabinetPathFromProject()
        {
            return this._model.GetCabinetPathFromProject();
        }

        //從專案檔獲取媒體櫃影片的Guid
        public List<string> GetCabinetGuidFromProject()
        {
            return this._model.GetCabinetGuidFromProject();
        }

        //從專案檔獲取分鏡腳本內MediaSource的Guid
        public List<Guid> GetMediaSourceGuidFromProject()
        {
            return this._model.GetMediaSourceGuidFromProject();
        }

        //增加媒體至媒體櫃
        public void AddMediaIntoCabinet(Media media)
        {
            this._model.AddMediaIntoCabinetData(media);
        }

        //通知變更
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
