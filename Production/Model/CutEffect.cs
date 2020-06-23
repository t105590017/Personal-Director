using Production.MediaProcess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Production.Model
{
    public class CutEffect : IEffect
    {
        public TimeSpan Begin { get; private set; }
        public TimeSpan End { get; private set; }
        private VideoHandlerObject videoHandler { get; set; }

        /// <summary>
        /// 影片剪裁
        /// </summary>
        /// <param name="Begin">開始時間</param>
        /// <param name="End">結束時間</param>
        public CutEffect(TimeSpan Begin, TimeSpan End)
        {
            this.Begin = Begin;
            this.End = End;
        }

        public string OutputPath { get; private set; }

        public void SetDataSource(Guid guid, string path)
        {
            videoHandler = VideoHandler.SetSource(guid, path);
            this.OutputPath = path;
        }

        public List<object> GetParameters()
        {
            List<object> result = new List<object>();

            result.Add(this.Begin);
            result.Add(this.End);

            return result;
        }

        public void Excute()
        {
            this.videoHandler.CutVideo(this.Begin, this.End);
            this.OutputPath = videoHandler.OutputPath;
        }
    }
}
