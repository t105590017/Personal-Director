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

        public string GetProjectInfoToSaving() 
        {
            return this._model.Project.GetProjectInfo();
        }

        public void AddMediaIntoProjectInfo(string path, Media media) 
        {
            this._model.Project.AddMediaIntoCabinetJson(path, media);
        }

        public void AddStoryBoardIntoProjectInfo(StoryBoard storyBoard)
        {
            this._model.Project.AddStoryBoardIntoScriptJson(storyBoard);
        }

        public ObservableCollection<Media> GridViewMediaCabinetList
        {
            get 
            {
                return this._model.getAllMediaCabinetData();
            }
        }

        public ObservableCollection<StoryBoard> GridViewStoryBoardScriptDataList
        {
            get
            {
                return this._model.getAllStoryBoardScriptData();
            }
        }

        //增加媒體至媒體櫃
        public void AddMediaIntoCabinet(Media media)
        {
            this._model.AddMediaIntoCabinetData(media);         
        }

        //增加分鏡至分鏡腳本
        public void InsertStoryBoardIntoScript(int index, Media media)
        {
            StoryBoard storyBoard = new StoryBoard(media);
            this._model.InsertStoryBoardIntoScriptData(index, storyBoard);
        }

        //刪除分鏡腳本中的分鏡
        public void RemoveMediaFromScript(string selectedGuid)
        {
            StoryBoard slelctedItem = this.GridViewStoryBoardScriptDataList.FirstOrDefault(i => i.MediaSource.Guid.ToString() == selectedGuid);
            this._model.RemoveStoryBoardFromScriptData(slelctedItem);
        }

        //通知變更
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
