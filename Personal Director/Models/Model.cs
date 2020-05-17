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
        ObservableCollection<Media> _mediaCabinetData;

        ObservableCollection<Media> _mediaScriptData;

        Project _project;

        public Model()
        {
            this._mediaCabinetData = new ObservableCollection<Media>();
            this._mediaScriptData = new ObservableCollection<Media>();
            this._project = new Project();
        }

        public void SetProject(Project project) 
        {
            this._project = project;
        }

        public List<string> GetCabinetPathFromProject()
        {
            return this._project.GetMediaCabinetPath();
        }

        //取得所有媒體櫃中的資料
        public ObservableCollection<Media> getAllMediaCabinetData()
        {
            return this._mediaCabinetData;
        }

        //取得所有分鏡腳本中的資料
        public ObservableCollection<Media> getAllMediaScriptData()
        {
            return this._mediaScriptData;
        }

        public void AddMediaIntoCabinetData (Media media)
        {
            this._mediaCabinetData.Add(media);
        }

        public void InsertMediaIntoScriptData(int index, Media media)
        {
            this._mediaScriptData.Insert(index, media);
        }

        public void RemoveMediaFromScriptData(Media media)
        {
            this._mediaScriptData.Remove(media);
        }
    }
}
