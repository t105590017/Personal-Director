using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal_Director.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Personal_Director.ViewModels
{
    public class ProjectEditViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Model _model;

        public ProjectEditViewModel(Model model)
        {
            this._model = model;
        }

        public ObservableCollection<Media> GridViewMediaCabinetList
        {
            get 
            {
                return this._model.getAllMediaCabinetData();
            }
        }

        public ObservableCollection<Media> GridViewMediaScriptDataList
        {
            get
            {
                return this._model.getAllMediaScriptData();
            }
        }

        //增加媒體至媒體櫃
        public void AddMediaIntoCabinet(Media media)
        {
            this._model.AddMediaIntoCabinetData(media);         
        }

        //增加媒體至媒體櫃
        public void InsertMediaIntoCabinet(int index, Media media)
        {
            this._model.InsertMediaIntoScriptData(index, media);
        }

        //刪除分鏡腳本中的分鏡
        public void RemoveMediaFromScript(string selectedGuid)
        {
            Media slelctedItem = this.GridViewMediaScriptDataList.FirstOrDefault(i => i.Guid.ToString() == selectedGuid);
            this._model.RemoveMediaFromScriptData(slelctedItem);
        }

        //通知變更
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
