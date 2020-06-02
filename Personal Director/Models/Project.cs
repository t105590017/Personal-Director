using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Personal_Director.Models
{
    public class Project
    {
        public string Name { get; set; }

        private JsonArray _mediaCabinetJson;

        private JsonArray _scriptJson;

        public Project()
        {
            this.Guid = System.Guid.NewGuid();
            this._mediaCabinetJson = new JsonArray();
            this._scriptJson = new JsonArray();
        }

        //將專案檔資料讀入
        public Project(string jsonString)
        {
            JsonObject jsonObject = JsonObject.Parse(jsonString);
            //TODO: 專案檔正確性偵測未寫
            this._mediaCabinetJson = jsonObject.GetNamedArray("MediaCabinet");
            this._scriptJson = jsonObject.GetNamedArray("Script");
        }

        public Project(Guid guid)
        {
            this.Guid = guid;
        }
        public Project(Project project)
        {
            this.Guid = System.Guid.NewGuid();
            this.Date = project.Date;
        }

        public string GetProjectInfo()
        {
            JsonObject result = new JsonObject();
            result.Add("MediaCabinet", this._mediaCabinetJson);
            result.Add("Script", this._scriptJson);
            return result.ToString();
        }

        //將新增的Media寫入JsonArray
        public void AddMediaIntoCabinetJson(string path, Media media) 
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.Add("path", JsonValue.CreateStringValue(path));
            jsonObject.Add("Guid", JsonValue.CreateStringValue(media.Guid.ToString()));
            this._mediaCabinetJson.Add(jsonObject);
        }

        //將新增的StoryBoard寫入JsonArray
        public void AddStoryBoardIntoScriptJson(StoryBoard storyBoard)
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.Add("Guid", JsonValue.CreateStringValue(storyBoard.MediaSource.Guid.ToString()));
            this._scriptJson.Add(jsonObject);
        }

        public List<string> GetMediaCabinetPaths()
        {
            List<string> mediaCabinets = new List<string>();
            //foreach (JsonValue jsonValue in this._mediaCabinetJson)
            //{
            //    JsonObject media = jsonValue.GetObject();
            //    mediaCabinets.Add(media.GetNamedString("path"));
            //}
            for (int i = 0; i < this._mediaCabinetJson.Count; i++)
            {
                JsonObject media = this._mediaCabinetJson[i].GetObject();
                mediaCabinets.Add(media.GetNamedString("path"));
            }

            return mediaCabinets;
        }

        public List<string> GetMediaCabinetGuids()
        {
            List<string> mediaCabinetGuids = new List<string>();
            for (int i = 0; i < this._mediaCabinetJson.Count; i++)
            {
                JsonObject media = this._mediaCabinetJson[i].GetObject();
                mediaCabinetGuids.Add(media.GetNamedString("Guid"));
            }

            return mediaCabinetGuids;
        }

        public List<Guid> GetMediaSourceGuids()
        {
            List<Guid> mediaSourceGuids = new List<Guid>();
            for (int i = 0; i < this._scriptJson.Count; i++)
            {
                JsonObject storyBoard = this._scriptJson[i].GetObject();
                mediaSourceGuids.Add(Guid.Parse(storyBoard.GetNamedString("Guid")));
            }
            return mediaSourceGuids;
        }
        public DateTime Date { get; }

        public Guid Guid { get; private set; }

        public IEnumerable<Media> MediaCabinetList { get; set; }
        public IEnumerable<Media> MediaScriptList { get; set; }
    }
}
