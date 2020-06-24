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

        //從專案取得媒體的檔案路徑
        public List<string> GetCabinetPathFromProject()
        {
            return this.Project.GetMediaCabinetPaths();
        }

        //從專案取得媒體櫃的Guid
        public List<string> GetCabinetGuidFromProject()
        {
            return this.Project.GetMediaCabinetGuids();
        }

        //從專案取得媒體的Guid
        public List<Guid> GetMediaSourceGuidFromProject()
        {
            return this.Project.GetMediaSourceGuids();
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

        //新增媒體至媒體櫃
        public void AddMediaIntoCabinetData (Media media)
        {
            this._mediaCabinetData.Add(media);
        }

        public void SetScriptData(ObservableCollection<StoryBoard> script)
        {
            this._storyBoardScriptData = script;
        }

        //插入分鏡至分鏡腳本
        public void InsertStoryBoardIntoScriptData(int index, StoryBoard storyboard)
        {
            this._storyBoardScriptData.Insert(index, storyboard);
            this.Project.InsertStoryBoardIntoScriptJson(index, storyboard);
        }

        //新增分鏡至分鏡腳本
        public void AddStoryBoardIntoScriptData(StoryBoard storyboard)
        {
            this._storyBoardScriptData.Add(storyboard);
        }

        //從分鏡腳本內刪除分鏡
        public void RemoveStoryBoardFromScriptData(StoryBoard storyboard)
        {
            int index = this._storyBoardScriptData.IndexOf(this._storyBoardScriptData.FirstOrDefault(x=> x.Guid == storyboard.Guid));
            this._storyBoardScriptData.Remove(storyboard);
            this.Project.RemoveStoryBoardFromScriptJson(index);
        }

        /// <summary>
        /// 更新StoryBoard
        /// </summary>
        /// <param name="updatedStoryBoard"></param>
        public void UpdateStoryBoard(StoryBoard updatedStoryBoard)
        {
            var storyBoard = this._storyBoardScriptData.FirstOrDefault(x => x.Guid == updatedStoryBoard.Guid);
            int index = this._storyBoardScriptData.IndexOf(storyBoard);
            this._storyBoardScriptData[index] = updatedStoryBoard;
            this.Project.UpdateStoryBoard(index, updatedStoryBoard);
        }

        public void RemoveMediaFormCabinetData(int index)
        {
            this._mediaCabinetData.RemoveAt(index);
            this.Project.RemoveMediaFromCabinetJson(index);
        }
    }
}
