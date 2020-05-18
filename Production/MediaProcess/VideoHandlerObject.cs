﻿using Production.Enum;
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
        public string WorkPath { get => this.Guid == null ? $"{ApplicationData.Current.LocalFolder.Path}\\MediaProcess"
                                                           : $"{ApplicationData.Current.LocalFolder.Path}\\MediaProcess\\{this.Guid}"; }
        private string Guid { get; set; }

        /// <summary>
        /// 在 MediaProcess/guid下工作
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="filePath"></param>
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
                source.CopyAsync(folder).AsTask().GetAwaiter().GetResult()
                      .RenameAsync($"Source{this.FielType}").AsTask().GetAwaiter().GetResult();
                source.CopyAsync(folder).AsTask().GetAwaiter().GetResult()
                      .RenameAsync($"Output{this.FielType}").AsTask().GetAwaiter().GetResult();
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
            return true;
        }

        /// <summary>
        /// 影片剪裁
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public VideoHandlerObject CutVideo(TimeSpan begin, TimeSpan end) 
        {
            bool isExist = this.IsFileExist($"{this.WorkPath}\\Output{this.FielType}");
            Process process = this.GetVideoProcess();
            process.StartInfo.Arguments = $"-i {(isExist ? this.OutputPathOld: this.SourcePath)} " +
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
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontcolor"></param>
        /// <param name="fontsize"></param>
        /// <returns></returns>
        public VideoHandlerObject AddTextToVideo(string text, int x, int y, Color fontcolor, int fontsize = 24)
        {
            return this.AddTextToVideo(text, $"x={x.ToString()}:y={y.ToString()}:", fontcolor, fontsize);
        }

        /// <summary>
        /// 加字幕 給Enum
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontcolor"></param>
        /// <param name="fontsize"></param>
        /// <returns></returns>
        public VideoHandlerObject AddTextToVideo(string text, VideoPosition position, Color fontcolor, int fontsize = 24)
        {
            return this.AddTextToVideo(text, $"{EnumHelper.GetDescription<VideoPosition>(position.ToString())}", fontcolor, fontsize);
        }

        /// <summary>
        /// 加字幕
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontcolor"></param>
        /// <param name="fontsize"></param>
        /// <returns></returns>
        private VideoHandlerObject AddTextToVideo(string text, string position, Color fontcolor, int fontsize = 24)
        {
            bool isExist = this.IsFileExist($"{this.WorkPath}\\Output{this.FielType}");
            Process process = this.GetVideoProcess();
            process.StartInfo.Arguments = $"-i {(isExist ? this.OutputPathOld : this.SourcePath)} " +
                                          $"-vf drawtext=\"" +
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
        /// 匯出
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        public VideoHandlerObject ConcatenateVideos(string[] guids)
        {
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
