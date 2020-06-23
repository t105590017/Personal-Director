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
        public string Text { get; private set; }
        public int? X { get; private set; }
        public int? Y { get; private set; }
        public VideoPosition? Position { get; private set; }
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }
        public string Fontfile { get; private set; }
        public int Fontsize { get; private set; }
        private VideoHandlerObject videoHandler { get; set; }

        /// <summary>
        /// 加字幕 給座標
        /// </summary>
        /// <param name="Text">字幕</param>
        /// <param name="X">x座標</param>
        /// <param name="Y">y座標</param>
        /// <param name="Color">文字顏色</param>
        /// <param name="Fontfile">C:/Windows/Fonts/ 下的 font file name</param>
        /// <param name="Fontsize">文字大小</param>
        /// <returns></returns>
        public TextEffect(string Text, int X, int Y, int Red, int Green, int Blue, string Fontfile = "msjh.ttc", int Fontsize = 24)
        {
            this.Text = Text;
            this.X = X;
            this.Y = Y;
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Fontfile = Fontfile;
            this.Fontsize = Fontsize;
        }

        /// <summary>
        /// 加字幕 給座標
        /// </summary>
        /// <param name="Text">字幕</param>
        /// <param name="X">x座標</param>
        /// <param name="Y">y座標</param>
        /// <param name="color">文字顏色</param>
        /// <param name="Fontfile">C:/Windows/Fonts/ 下的 font file name</param>
        /// <param name="Fontsize">文字大小</param>
        /// <returns></returns>
        public TextEffect (string Text, int X, int Y, Color color, string Fontfile = "msjh.ttc", int Fontsize = 24)
        {
            this.Text = Text;
            this.X = X;
            this.Y = Y;
            this.Red = color.R;
            this.Green = color.G;
            this.Blue = color.B;
            this.Fontfile = Fontfile;
            this.Fontsize = Fontsize;
        }

        /// <summary>
        /// 加字幕 給Enum
        /// </summary>
        /// <param name="Text">字幕</param>
        /// <param name="Position">位置列舉</param>
        /// <param name="Color">文字顏色</param>
        /// <param name="Fontfile">C:/Windows/Fonts/ 下的 font file name</param>
        /// <param name="Fontsize">文字大小</param>
        /// <returns></returns
        public TextEffect(string Text, VideoPosition Position, int Red, int Green, int Blue, string Fontfile = "msjh.ttc", int Fontsize = 24)
        {
            this.Text = Text;
            this.Position = Position;
            this.Red = Red;
            this.Green = Green;
            this.Blue = Blue;
            this.Fontfile = Fontfile;
            this.Fontsize = Fontsize;
        }

        /// <summary>
        /// 加字幕 給Enum
        /// </summary>
        /// <param name="Text">字幕</param>
        /// <param name="Position">位置列舉</param>
        /// <param name="Color">文字顏色</param>
        /// <param name="Fontfile">C:/Windows/Fonts/ 下的 font file name</param>
        /// <param name="Fontsize">文字大小</param>
        /// <returns></returns
        public TextEffect (string Text, VideoPosition Position, Color color, string Fontfile = "msjh.ttc", int Fontsize = 24)
        {
            this.Text = Text;
            this.Position = Position;
            this.Red = color.R;
            this.Green = color.G;
            this.Blue = color.B;
            this.Fontfile = Fontfile;
            this.Fontsize = Fontsize;
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
            //result.Add(this.Color);
            result.Add(this.Fontfile);
            result.Add(this.Fontsize);

            return result;
        }

        public void Excute()
        {
            Color color = Color.FromArgb(this.Red, this.Green, this.Blue);
            if (!this.Position.HasValue)
                videoHandler.AddTextToVideo(this.Text, this.X.Value, this.Y.Value, color, this.Fontfile, this.Fontsize);
            else
                videoHandler.AddTextToVideo(this.Text, this.Position.Value, color, this.Fontfile, this.Fontsize);
            this.OutputPath = videoHandler.OutputPath;
        }
    }
       
}
