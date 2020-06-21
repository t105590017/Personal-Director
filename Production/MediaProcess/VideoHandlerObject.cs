using Production.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Production.MediaProcess
{
    public class VideoHandlerObject
    {
        public string FielType { get; private set; }
        public string SourcePath { get => $"{this.WorkPath}\\Source{this.FielType}"; }
        public string OutputPath { get => $"{this.WorkPath}\\Output{this.FielType}"; }
        private string OutputPathOld { get => $"{this.WorkPath}\\Output.old{this.FielType}"; }
        public string WorkPath
        {
            get => this.Guid == null ? $"{ApplicationData.Current.LocalFolder.Path}\\MediaProcess"
                                      : $"{ApplicationData.Current.LocalFolder.Path}\\MediaProcess\\{this.Guid}";
        }
        private string Guid { get; set; }

        /// <summary>
        /// 在 MediaProcess/guid下工作
        /// </summary>
        /// <param name="guid">資料夾guid</param>
        /// <param name="filePath">來源檔案路徑</param>
        public VideoHandlerObject(Guid guid, string filePath)
        {
            this.Guid = guid.ToString();
            this.FielType = "." + filePath.Split('.').Last();
            try
            {
                StorageFolder x = StorageFolder.GetFolderFromPathAsync(this.WorkPath).AsTask().GetAwaiter().GetResult();
            }
            catch
            {

                var folder = StorageFolder.GetFolderFromPathAsync(ApplicationData.Current.LocalFolder.Path + "\\MediaProcess").AsTask().GetAwaiter().GetResult()
                                          .CreateFolderAsync(this.Guid).AsTask().GetAwaiter().GetResult();
                var source = StorageFile.GetFileFromPathAsync(filePath).AsTask().GetAwaiter().GetResult();
                source.CopyAsync(folder, $"Source{this.FielType}").AsTask().GetAwaiter().GetResult();
                //.RenameAsync($"Source{this.FielType}").AsTask().GetAwaiter().GetResult();
                source.CopyAsync(folder, $"Output{this.FielType}").AsTask().GetAwaiter().GetResult();
                //.RenameAsync($"Output{this.FielType}").AsTask().GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// 在 MediaProcess下工作
        /// </summary>
        /// <param name="fileType"></param>
        public VideoHandlerObject(string fileType = null)
        {
            this.FielType = fileType;
        }

        private Process GetVideoProcess()
        {
            Process process = new Process();
            //process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = ApplicationData.Current.LocalFolder.Path + "\\ffmpeg.exe";
            return process;
        }

        /// <summary>
        /// 判斷檔案存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsFileExist(string path)
        {
            try
            {
                StorageFile file = StorageFile.GetFileFromPathAsync($"{this.WorkPath}\\Output{this.FielType}").AsTask().GetAwaiter().GetResult();
                file.RenameAsync($"Output.old{this.FielType}").AsTask().GetAwaiter().GetResult();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 影片剪裁
        /// </summary>
        /// <param name="begin">開始時間</param>
        /// <param name="end">結束時間</param>
        /// <returns></returns>
        public VideoHandlerObject CutVideo(TimeSpan begin, TimeSpan end)
        {
            bool isExist = this.IsFileExist($"{this.WorkPath}\\Output{this.FielType}");
            Process process = this.GetVideoProcess();
            process.StartInfo.Arguments = $"-i {(isExist ? this.OutputPathOld : this.SourcePath)} " +
                                          $"-ss {begin.ToString()} " +
                                          $"-t {(end - begin).ToString()} " +
                                          $"{this.OutputPath}";

            process.Start();
            process.WaitForExit();
            process.Close();
            if (isExist)
            {
                StorageFile.GetFileFromPathAsync($"{this.WorkPath}\\Output.old{this.FielType}").AsTask().GetAwaiter().GetResult()
                           .DeleteAsync().AsTask().GetAwaiter().GetResult();
            }

            return this;
        }

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
        public VideoHandlerObject AddTextToVideo(string text, int x, int y, Color fontcolor, string fontfile = "msjh.ttc", int fontsize = 24)
        {
            return this.AddTextToVideo(text, $"x={x.ToString()}:y={y.ToString()}:", fontcolor, fontfile, fontsize);
        }

        /// <summary>
        /// 加字幕 給Enum
        /// </summary>
        /// <param name="text">字幕</param>
        /// <param name="position">位置列舉</param>
        /// <param name="fontcolor">文字顏色</param>
        /// <param name="fontfile">C:/Windows/Fonts/ 下的 font file name</param>
        /// <param name="fontsize">文字大小</param>
        /// <returns></returns>
        public VideoHandlerObject AddTextToVideo(string text, VideoPosition position, Color fontcolor, string fontfile = "msjh.ttc", int fontsize = 24)
        {
            return this.AddTextToVideo(text, $"{EnumHelper.GetDescription<VideoPosition>(position.ToString())}", fontcolor, fontfile, fontsize);
        }

        /// <summary>
        /// 加字幕
        /// </summary>
        /// <param name="text">字幕</param>
        /// <param name="position">位置</param>
        /// <param name="fontcolor">文字顏色</param>
        /// <param name="fontfile">C:/Windows/Fonts/ 下的 font file name</param>
        /// <param name="fontsize">文字大小</param>
        /// <returns></returns>
        private VideoHandlerObject AddTextToVideo(string text, string position, Color fontcolor, string fontfile, int fontsize)
        {
            bool isExist = this.IsFileExist($"{this.WorkPath}\\Output{this.FielType}");
            Process process = this.GetVideoProcess();
            process.StartInfo.Arguments = $"-i {(isExist ? this.OutputPathOld : this.SourcePath)} " +
                                          $"-vf drawtext=\"" +
                                          $"fontfile=/Windows/Fonts/{fontfile}:" +
                                          $"fontsize = {fontsize}:" +
                                          $"fontcolor = #{fontcolor.R.ToString("X2") + fontcolor.G.ToString("X2") + fontcolor.B.ToString("X2")}:" +
                                          $"{position}" + //  x=[x]:y=[y]:
                                          $"text = '{text}'" +
                                          $"\" " +
                                          $"{this.WorkPath}\\Output{this.FielType}";

            process.Start();
            process.WaitForExit();
            process.Close();

            if (isExist)
            {
                StorageFile.GetFileFromPathAsync($"{this.WorkPath}\\Output.old{this.FielType}").AsTask().GetAwaiter().GetResult()
                           .DeleteAsync().AsTask().GetAwaiter().GetResult();
            }

            return this;
        }

        /// <summary>
        /// 修改影像速度
        /// </summary>
        /// <param name="multiple">倍率 [0.25, 4]</param>
        /// <param name="noAudio">是否禁音</param>
        /// <returns></returns>
        public VideoHandlerObject ChangeVideoSpeed(double multiple, bool noAudio = false)
        {
            if (multiple > 4 || multiple < 0.25)
            {
                throw new Exception("倍率區間 [0.25, 4]！！");
            }
            bool isExist = this.IsFileExist($"{this.WorkPath}\\Output{this.FielType}");
            Process process = this.GetVideoProcess();
            process.StartInfo.Arguments = $"-i {(isExist ? this.OutputPathOld : this.SourcePath)} " +
                                          $"{(noAudio ? "-an" : "")} " + // 是否禁音
                                          $"-filter:v  \"setpts=PTS/{multiple.ToString("0.00")}\" " +
                                          $"{this.WorkPath}\\Output{this.FielType}";

            process.Start();
            process.WaitForExit();
            process.Close();

            if (isExist)
            {
                StorageFile.GetFileFromPathAsync($"{this.WorkPath}\\Output.old{this.FielType}").AsTask().GetAwaiter().GetResult()
                           .DeleteAsync().AsTask().GetAwaiter().GetResult();
            }

            return this;
        }

        /// <summary>
        /// 修改音訊速度
        /// </summary>
        /// <param name="multiple">倍率 [0.5, 2]</param>
        /// <param name="noVideo">是否刪除影像</param>
        /// <returns></returns>
        public VideoHandlerObject ChangeAudioSpeed(double multiple, bool noVideo = false)
        {
            if (multiple > 2 || multiple < 0.5)
            {
                throw new Exception("倍率區間 [0.5, 2]！！");
            }
            bool isExist = this.IsFileExist($"{this.WorkPath}\\Output{this.FielType}");
            Process process = this.GetVideoProcess();
            process.StartInfo.Arguments = $"-i {(isExist ? this.OutputPathOld : this.SourcePath)} " +
                                          $"{(noVideo ? "-vn" : "")} " + // 是否刪除影像
                                          $"-filter:a \"atempo={multiple.ToString("0.00")}\" " +
                                          $"{this.WorkPath}\\Output{this.FielType}";

            process.Start();
            process.WaitForExit();
            process.Close();

            if (isExist)
            {
                StorageFile.GetFileFromPathAsync($"{this.WorkPath}\\Output.old{this.FielType}").AsTask().GetAwaiter().GetResult()
                           .DeleteAsync().AsTask().GetAwaiter().GetResult();
            }

            return this;

        }

        /// <summary>
        /// 匯出
        /// </summary>
        /// <param name="guids">分鏡的所有guid</param>
        /// <returns></returns>
        public VideoHandlerObject ConcatenateVideos(string[] guids)
        {
            StorageFolder.GetFolderFromPathAsync($"{ApplicationData.Current.LocalFolder.Path}\\MediaProcess").AsTask().GetAwaiter().GetResult()
                         .GetFilesAsync().AsTask().GetAwaiter().GetResult()
                         .Where(i => i.DisplayName == "Output").ToList()
                         .ForEach(i =>
                         {
                             i.DeleteAsync().AsTask().GetAwaiter().GetResult();
                         });

            StorageFolder folder = StorageFolder.GetFolderFromPathAsync($"{this.WorkPath}\\{guids.FirstOrDefault()}").AsTask().GetAwaiter().GetResult();
            this.FielType = this.FielType ?? folder.GetFilesAsync().AsTask().GetAwaiter().GetResult().FirstOrDefault().FileType;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (string guid in guids)
            {
                stringBuilder.AppendLine($"file '{this.WorkPath}\\{guid}\\Output{this.FielType}'");
            }

            StorageFile file = StorageFolder.GetFolderFromPathAsync(this.WorkPath).AsTask().GetAwaiter().GetResult()
                                            .CreateFileAsync("ConcatList.txt", CreationCollisionOption.OpenIfExists).AsTask().GetAwaiter().GetResult();

            FileIO.WriteTextAsync(file, stringBuilder.ToString()).AsTask().GetAwaiter().GetResult();

            Process process = this.GetVideoProcess();
            process.StartInfo.Arguments = $"-f concat -safe 0 -i {this.WorkPath}\\ConcatList.txt -c copy {this.OutputPath}";

            process.Start();
            process.WaitForExit();
            process.Close();

            return this;
        }

    }
}
