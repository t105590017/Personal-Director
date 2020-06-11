using Personal_Director.Models;
using Production.MediaProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Personal_Director.ViewModels
{
    public class TextEditPageViewModel
    {
        public StoryBoard StoryBoard { get; set; }

        public TextEditPageViewModel(StoryBoard storyBoard)
        {
            this.StoryBoard = storyBoard;
        }

        public string GetProcessedMediaPath(TimeSpan startTime, TimeSpan endTime, string text, Production.Enum.VideoPosition position, Color color, string font, int size)
        {
            // TODO: 字型與字體大小還未寫
            VideoHandlerObject videoHandlerObject = VideoHandler.SetSource(this.StoryBoard.Guid, this.StoryBoard.MediaSource.SourcePath)
                            .AddTextToVideo(text, position, color);

            return videoHandlerObject.OutputPath;
        }
    }
}
