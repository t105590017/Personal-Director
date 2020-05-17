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

        private JsonArray _mediaCabinetJson { get; set; }

        private JsonArray _scriptJson { get; set; }

        public Project()
        {
            this.Guid = System.Guid.NewGuid();
        }

        //將專案檔資料讀入
        public Project(string jsonString)
        {
            JsonObject jsonObject = JsonObject.Parse(jsonString);
            //TODO: 專案檔正確性偵測未寫
            this._mediaCabinetJson = jsonObject.GetNamedArray("MediaCabinet");
            this._scriptJson = jsonObject.GetNamedArray("Script");
            //foreach (JsonValue mediaCabinet in this.mediaCabinetsJson)
            //{
            //    JsonObject Cabinet = mediaCabinet.GetObject();
            //    string path = Cabinet.GetNamedString("path");
            //}
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

        public List<string> GetMediaCabinetPath()
        {
            List<string> mediaCabinets = new List<string>();
            foreach (JsonValue jsonValue in this._mediaCabinetJson)
            {
                JsonObject media = jsonValue.GetObject();
                mediaCabinets.Add(media.GetNamedString("path"));
            }
            return mediaCabinets;
        }
        public DateTime Date { get; }

        public Guid Guid { get; private set; }

        public IEnumerable<Media> MediaCabinetList { get; set; }
        public IEnumerable<Media> MediaScriptList { get; set; }
    }
}
