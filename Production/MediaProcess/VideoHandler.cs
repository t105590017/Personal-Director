using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace Production.MediaProcess
{
    public static class VideoHandler
    {
        private static bool isInitial = false;

        public static void Initial()
        {
            var _file = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ffmpeg.exe")).AsTask().Result;
            _ = _file.CopyAsync(ApplicationData.Current.LocalFolder, "ffmpeg.exe", NameCollisionOption.ReplaceExisting).AsTask().Result;

            _ = ApplicationData.Current.LocalFolder.CreateFolderAsync("MediaProcess", CreationCollisionOption.ReplaceExisting).AsTask().Result;
        }

        private static void IsInitial()
        {
            if (!VideoHandler.isInitial)
            {
                VideoHandler.Initial();
                VideoHandler.isInitial = true;
            }
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static VideoHandlerObject SetSource(Guid guid, string fileType)
        {
            IsInitial();
            return new VideoHandlerObject(guid, fileType);
        }

        /// <summary>
        /// 匯出
        /// </summary>
        /// <param name="guids"></param>
        /// <returns></returns>
        public static VideoHandlerObject Export(string[] guids)
        {
            IsInitial();
            return (new VideoHandlerObject()).ConcatenateVideos(guids);
        }

        /// <summary>
        /// 清空暫存
        /// </summary>
        public static void ClearTemp()
        {
            StorageFolder.GetFolderFromPathAsync((new VideoHandlerObject()).WorkPath).AsTask().GetAwaiter().GetResult()
                         .DeleteAsync().AsTask().GetAwaiter().GetResult();
        }

    }
}
