using Production.Enum;
using Production.MediaProcess;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Production.Model
{
    public class TextEffect : IEffect
    {
        private string Text { get; set; }
        private int X { get; set; }
        private int Y { get; set; }
        private VideoPosition Position { get; set; }
        private Color Color { get; set; }
        private string Fontfile { get; set; }
        private int Fontsize { get; set; }
        private VideoHandlerObject videoHandler { get; set; }
        private int _type;

        /// <summary>
        /// 加字幕 給座標
        /// </summary>
        /// <param name="text">字幕</param>
        /// <param name="x">x座標</param>
        /// <param name="y">y座標</param>
        /// <param name="fontcolor">文字顏色</param>
        /// <param name="fontfile">C:/Windows/Fonts/ 下的 font file name</param>
        /// <param name="fontsize">文字大小</param>
        /// <returns></returns>
        public TextEffect (string text, int x, int y, Color fontcolor, string fontfile = "msjh.ttc", int fontsize = 24)
        {
            this.Text = text;
            this.X = x;
            this.Y = y;
            this.Color = fontcolor;
            this.Fontfile = fontfile;
            this.Fontsize = fontsize;
            this._type = 0;
        }

        /// <summary>
        /// 加字幕 給Enum
        /// </summary>
        /// <param name="text">字幕</param>
        /// <param name="position">位置列舉</param>
        /// <param name="fontcolor">文字顏色</param>
        /// <param name="fontfile">C:/Windows/Fonts/ 下的 font file name</param>
        /// <param name="fontsize">文字大小</param>
        /// <returns></returns
        public TextEffect (string text, VideoPosition position, Color fontcolor, string fontfile = "msjh.ttc", int fontsize = 24)
        {
            this.Text = text;
            this.Position = position;
            this.Color = fontcolor;
            this.Fontfile = fontfile;
            this.Fontsize = fontsize;
            this._type = 1;
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

            result.Add(this.Text);
            result.Add(this.Position);
            result.Add(this.Color);
            result.Add(this.Fontfile);
            result.Add(this.Fontsize);

            return result;
        }

        public void Excute()
        {
            if (this._type == 0)
                videoHandler.AddTextToVideo(this.Text, this.X, this.Y, this.Color, this.Fontfile, this.Fontsize);
            else
                videoHandler.AddTextToVideo(this.Text, this.Position, this.Color, this.Fontfile, this.Fontsize);
            this.OutputPath = videoHandler.OutputPath;
        }
    }
}
