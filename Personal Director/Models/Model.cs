using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Director.Models
{
    public class Model
    {
        private ObservableCollection<Media> _mediaCabinetData;

        private ObservableCollection<StoryBoard> _storyBoardScriptData;

        public Project Project;

        public Model()
        {
            this._mediaCabinetData = new ObservableCollection<Media>();
            this._storyBoardScriptData = new ObservableCollection<StoryBoard>();
            this.Project = new Project();
        }

        public List<string> GetCabinetPathFromProject()
        {
            return this.Project.GetMediaCabinetPath();
        }

        public List<string> GetCabinetGuidFromProject()
        {
            return this.Project.GetMediaCabinetGuid();
        }

        public List<Guid> GetMediaSourceGuidFromProject()
        {
            return this.Project.GetMediaSourceGuid();
        }

        //取得所有媒體櫃中的資料
        public ObservableCollection<Media> getAllMediaCabinetData()
        {
            return this._mediaCabinetData;
        }

        //取得所有分鏡腳本中的資料
        public ObservableCollection<StoryBoard> getAllStoryBoardScriptData()
        {
            return this._storyBoardScriptData;
        }

        public void AddMediaIntoCabinetData (Media media)
        {
            this._mediaCabinetData.Add(media);
        }

        public void InsertStoryBoardIntoScriptData(int index, StoryBoard storyboard)
        {
            this._storyBoardScriptData.Insert(index, storyboard);
        }

        public void AddStoryBoardIntoScriptData(StoryBoard storyboard)
        {
            this._storyBoardScriptData.Add(storyboard);
        }

        public void RemoveStoryBoardFromScriptData(StoryBoard storyboard)
        {
            this._storyBoardScriptData.Remove(storyboard);
        }

        
    }
}
