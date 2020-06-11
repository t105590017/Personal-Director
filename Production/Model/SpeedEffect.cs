using Production.MediaProcess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Production.Model
{
    public class SpeedEffect : IEffect
    {
        private int Count { get; set; }
        private VideoHandlerObject videoHandler { get; set; }

        /// <summary>
        /// 修改速度
        /// </summary>
        /// <param name="multiple">倍率(須為2的次方)</param>
        public SpeedEffect(double multiple)
        {
            this.Count = (int)Math.Log(multiple, 2);
        }

        public string OutputPath { get; private set; }

        public void SetDataSource(Guid guid, string path)
        {
            videoHandler = VideoHandler.SetSource(guid, path);
            this.OutputPath = path;
        }

        public void Excute()
        {
            if (Count == 1)
                return;
            int i = this.Count;
            for (; i < 0; i++)
            {
                videoHandler.ChangeAudioSpeed(0.5).ChangeVideoSpeed(0.5);
            }
            for (; i > 0; i--)
            {
                videoHandler.ChangeAudioSpeed(2).ChangeVideoSpeed(2);
            }
            this.OutputPath = videoHandler.OutputPath;
        }

    }
}
