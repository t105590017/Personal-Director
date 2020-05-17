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

        public bool OpenProject(string jsonString)
        {
            //TODO: 防呆待寫
            Project project = new Project(jsonString);
            this._model.SetProject(project);
            return true;
        }

        //從專案檔獲取媒體櫃影片的路徑
        public List<string> GetCabinetPathFromProject()
        {
            return this._model.GetCabinetPathFromProject();
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
